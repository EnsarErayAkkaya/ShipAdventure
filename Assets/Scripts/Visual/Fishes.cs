using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

using UnityEngine.Jobs;
using EEA.Managers;
using Unity.Jobs;
using random = Unity.Mathematics.Random;
using math = Unity.Mathematics.math;
using Unity.Burst;
using System;

namespace EEA.Visuals
{
    public class Fishes : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private Sea sea;
        [SerializeField] private GameObject fishPrefab;
        [SerializeField] private int flockCount;
        [SerializeField] private int row;
        [SerializeField] private int increasePerRow;
        [SerializeField] private float rowDist;

        [Header("Fish Settings")]
        public float swimSpeed;
        public float turnSpeed;
        public int swimChangeFrequency;


        private TransformAccessArray transforms;
        private NativeArray<Vector3> velocities;
        private NativeArray<int> velocityIndexes;

        private PositionUpdateJob positionUpdateJob;
        private JobHandle positionUpdateJobHandle;

        private void Start()
        {
            int fishCountPerFlock = 1;

            for (int i = 0; i < row; i++)
            {
                fishCountPerFlock += fishCountPerFlock + increasePerRow;
            }

            velocities = new NativeArray<Vector3>(flockCount, Allocator.Persistent);
            velocityIndexes = new NativeArray<int>(flockCount * fishCountPerFlock, Allocator.Persistent);
            transforms = new TransformAccessArray(flockCount);


            int totalIndex = 0;
            for (int i = 0; i < flockCount; i++)
            {
                Sea.SeaPos pos;
                do
                {
                    pos = sea.GetRandomAreaOnSea();
                }
                while (!pos.success);

                int fishCount = 1;
                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < fishCount; k++)
                    {
                        var offset = (Vector3.right * (k - ((fishCount - 1) * .5f))) + (Vector3.down * .5f) + (Vector3.back * rowDist * j) ;

                        Transform t = Instantiate(fishPrefab, pos.pos + offset, Quaternion.identity).transform;
                        t.SetParent(transform);
                        t.position += Vector3.back * rowDist;
                        transforms.Add(t);
                        velocityIndexes[totalIndex] = i;

                        totalIndex++;
                    }

                    fishCount += increasePerRow;
                }
            }
        }

        private void Update()
        {
            positionUpdateJob = new PositionUpdateJob()
            {
                objectVelocities = velocities,
                jobDeltaTime = Time.deltaTime,
                swimSpeed = this.swimSpeed,
                turnSpeed = this.turnSpeed,
                time = Time.time,
                swimChangeFrequency = this.swimChangeFrequency,
                center = sea.transform.position,
                bounds = sea.GetBounds(),
                seed = System.DateTimeOffset.Now.Millisecond,
                velocityIndexes = velocityIndexes
            };

            positionUpdateJobHandle = positionUpdateJob.Schedule(transforms);
        }
        private void LateUpdate()
        {
            positionUpdateJobHandle.Complete();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(sea.transform.position + new Vector3(sea.GetBounds().x * .5f, 0, 0), Vector3.one);
            Gizmos.DrawCube(sea.transform.position + new Vector3(sea.GetBounds().z * .5f, 0, 0), Vector3.one);
            Gizmos.DrawCube(sea.transform.position + new Vector3(sea.GetBounds().z * -.5f, 0, 0), Vector3.one);
            Gizmos.DrawCube(sea.transform.position + new Vector3(sea.GetBounds().x * -.5f, 0, 0), Vector3.one);
        }

        [BurstCompile]
        struct DoubleInt
        {
            public int first;
            public int second;
        }

        [BurstCompile]
        struct PositionUpdateJob : IJobParallelForTransform
        {
            public NativeArray<Vector3> objectVelocities;
            public NativeArray<int> velocityIndexes;

            public Vector3 bounds;
            public Vector3 center;

            public float jobDeltaTime;
            public float time;
            public float swimSpeed;
            public float turnSpeed;

            public float seed;

            public int swimChangeFrequency;

            public void Execute(int i, TransformAccess transform)
            {
                int x = velocityIndexes[i];
                Vector3 currentVelocity = objectVelocities[x];
                random randomGen = new random((uint)(i * time + 1 + seed));

                transform.position += transform.localToWorldMatrix.MultiplyVector(new Vector3(0, 0, 1)) * swimSpeed * jobDeltaTime * randomGen.NextFloat(0.3f, 1.0f);

                if (currentVelocity != Vector3.zero)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentVelocity), turnSpeed * jobDeltaTime);
                }

                Vector3 currentPosition = transform.position;

                bool randomise = true;
                if (currentPosition.x > center.x + bounds.x / 2 || currentPosition.x < center.x - bounds.x / 2 || currentPosition.z > center.z + bounds.z / 2 || currentPosition.z < center.z - bounds.z / 2)
                {
                    Vector3 internalPosition = new Vector3(center.x + randomGen.NextFloat(-bounds.x / 2, bounds.x / 2) / 1.3f, 0, center.z + randomGen.NextFloat(-bounds.z / 2, bounds.z / 2) / 1.3f);
                    currentVelocity = (internalPosition - currentPosition).normalized;
                    objectVelocities[i] = currentVelocity;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentVelocity), turnSpeed * jobDeltaTime * 2);
                    randomise = false;
                }

                if (randomise)
                {
                    if (randomGen.NextInt(0, swimChangeFrequency) <= 2)
                    {
                        objectVelocities[i] = new Vector3(randomGen.NextFloat(-1f, 1f), 0, randomGen.NextFloat(-1f, 1f));
                    }
                }
            }
        }
    }
}