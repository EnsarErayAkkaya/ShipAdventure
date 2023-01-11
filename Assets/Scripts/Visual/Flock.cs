using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

namespace EEA.Visuals
{
    public class Flock : MonoBehaviour
    {
        [SerializeField] private Transform[] flockTransforms;

        [Header("Spawn Variables")]
        [SerializeField] private FlockUnit flockUnitPrefab;
        [SerializeField] private int flockSize;
        [SerializeField] private float spawnRadius;

        [Header("Unit Settings")]
        [SerializeField] private float fovAngle;
        [SerializeField] private float smoothDamp;
        [SerializeField, Range(0, 10)] private float minSpeed;
        [SerializeField, Range(0, 10)] private float maxSpeed;

        [Header("Detection Distances")]
        [SerializeField, Range(0,10)] private float cohesionDistance;
        [SerializeField, Range(0,10)] private float avoidanceDistance;
        [SerializeField, Range(0,10)] private float alignmentDistance;
        [SerializeField, Range(0,10)] private float boundsDistance;
        [Header("Detection Weights")]
        [SerializeField, Range(0,10)] private float cohesionWeight;
        [SerializeField, Range(0,10)] private float avoidanceWeight;
        [SerializeField, Range(0,10)] private float alignmentWeight;
        [SerializeField, Range(0,10)] private float boundsWeight;
        
        private FlockUnit[] allUnits;

        private int flockCount;
        private int allUnitsCount;

        public float MinSpeed => minSpeed;
        public float MaxSpeed => maxSpeed;

        public float CohesionDistance => cohesionDistance;
        public float AvoidanceDistance => avoidanceDistance;
        public float AlignmentDistance => alignmentDistance;
        public float BoundsDistance => boundsDistance;

        public float CohesionWeight => cohesionWeight;
        public float AvoidanceWeight => avoidanceWeight;
        public float AlignmentWeight => alignmentWeight;
        public float BoundsWeight => boundsWeight;

        public FlockUnit[] AllUnits => allUnits;

        private NativeArray<Vector3> flockPositions;

        private NativeArray<Vector3> unitForwardDirections;
        private NativeArray<Vector3> unitCurrentVelocities;
        private NativeArray<Vector3> unitPositions;

        private NativeArray<Vector3> cohesionNeighbours;
        private NativeArray<Vector3> avoidanceNeighbours;
        private NativeArray<Vector3> alignmentNeighbours;
        private NativeArray<Vector3> alignmentNeighboursDirections;

        private NativeArray<float>   allUnitsSpeeds;
        private NativeArray<float>   neighbourSpeeds;

        private void Start()
        {
            flockCount = flockTransforms.Length;
            allUnitsCount = flockCount * flockSize;
            GenerateUnits();
        }

        private void Update()
        {
            flockPositions = new NativeArray<Vector3>(flockCount, Allocator.TempJob);

            unitForwardDirections = new NativeArray<Vector3>(allUnitsCount, Allocator.TempJob);
            unitCurrentVelocities = new NativeArray<Vector3>(allUnitsCount, Allocator.TempJob);

            unitPositions = new NativeArray<Vector3>(allUnitsCount, Allocator.TempJob);

            cohesionNeighbours = new NativeArray<Vector3>(allUnitsCount, Allocator.TempJob);
            avoidanceNeighbours = new NativeArray<Vector3>(allUnitsCount, Allocator.TempJob);
            alignmentNeighbours = new NativeArray<Vector3>(allUnitsCount, Allocator.TempJob);
            alignmentNeighboursDirections = new NativeArray<Vector3>(allUnitsCount, Allocator.TempJob);

            allUnitsSpeeds = new NativeArray<float>(allUnitsCount, Allocator.TempJob);
            neighbourSpeeds = new NativeArray<float>(allUnitsCount, Allocator.TempJob);

            for (int i = 0; i < allUnitsCount; i++)
            {
                unitForwardDirections[i] = allUnits[i].Transform.forward;
                unitCurrentVelocities[i] = allUnits[i].CurrentVelocity;

                unitPositions[i] = allUnits[i].Transform.position;

                cohesionNeighbours[i] = Vector3.zero;
                avoidanceNeighbours[i] = Vector3.zero;
                alignmentNeighbours[i] = Vector3.zero;
                alignmentNeighboursDirections[i] = Vector3.zero;

                allUnitsSpeeds[i] = allUnits[i].CurrentSpeed;
                neighbourSpeeds[i] = 0f;

            }
            for (int i = 0; i < flockTransforms.Length; i++)
            {
                flockPositions[i] = flockTransforms[i].position;
            }

            FlockMoveJob moveJob = new FlockMoveJob
            {
                unitForwardDirections = unitForwardDirections,
                unitCurrentVelocities = unitCurrentVelocities,

                unitPositions = unitPositions,

                cohesionNeighbours = cohesionNeighbours,
                avoidanceNeighbours = avoidanceNeighbours,
                alignmentNeighbours = alignmentNeighbours,
                alignmentNeighboursDirections = alignmentNeighboursDirections,

                allUnitsSpeeds = allUnitsSpeeds,
                neighbourSpeeds = neighbourSpeeds,

                cohesionDistance = cohesionDistance,
                alignmentDistance = alignmentDistance,
                avoidanceDistance = avoidanceDistance,
                boundsDistance = boundsDistance,

                cohesionWeight = cohesionWeight,
                alignmentWeight = alignmentWeight,
                avoidanceWeight = avoidanceWeight,
                boundsWeight = boundsWeight,

                fovAngle = fovAngle,
                minSpeed = minSpeed,
                maxSpeed = maxSpeed,
                smoothDamp = smoothDamp,
                deltaTime = Time.deltaTime,

                flockPositions = flockPositions,

                flockSize = flockSize
            };

            JobHandle handle = moveJob.Schedule(allUnitsCount, 5);

            handle.Complete();

            for (int i = 0; i < allUnitsCount; i++)
            {
                allUnits[i].Transform.forward = unitForwardDirections[i];
                allUnits[i].Transform.position = unitPositions[i];
                allUnits[i].CurrentVelocity = unitCurrentVelocities[i];
                allUnits[i].CurrentSpeed = allUnitsSpeeds[i];
            }

            flockPositions.Dispose();

            unitForwardDirections.Dispose();
            unitCurrentVelocities.Dispose();

            unitPositions.Dispose();

            cohesionNeighbours.Dispose();
            avoidanceNeighbours.Dispose();
            alignmentNeighbours.Dispose();
            alignmentNeighboursDirections.Dispose();

            allUnitsSpeeds.Dispose();
            neighbourSpeeds.Dispose();
        }

        private void GenerateUnits()
        {
            allUnits = new FlockUnit[allUnitsCount];
            for (int fIndex = 0; fIndex < flockCount; fIndex++)
            {
                for (int i = 0; i < flockSize; i++)
                {
                    Vector3 randomVector = UnityEngine.Random.insideUnitSphere * spawnRadius;

                    Vector3 spawnPos = flockTransforms[fIndex].position + randomVector;
                    Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

                    allUnits[(fIndex * flockSize) + i] = Instantiate(flockUnitPrefab, spawnPos, rotation);

                    allUnits[(fIndex * flockSize) + i].transform.SetParent(flockTransforms[fIndex]);
                    allUnits[(fIndex * flockSize) + i].InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
                }
            }
        }
    }
    [BurstCompile]
    public struct FlockMoveJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> unitForwardDirections;
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> unitCurrentVelocities;
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> unitPositions;
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> cohesionNeighbours;
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> avoidanceNeighbours;
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> alignmentNeighbours;
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> alignmentNeighboursDirections;
        [NativeDisableParallelForRestriction]
        public NativeArray<float> allUnitsSpeeds;
        [NativeDisableParallelForRestriction]
        public NativeArray<float> neighbourSpeeds;

        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> flockPositions;

        public float cohesionDistance;
        public float avoidanceDistance;
        public float alignmentDistance;
        public float boundsDistance;

        public float cohesionWeight;
        public float avoidanceWeight;
        public float alignmentWeight;
        public float boundsWeight;
        
        public float minSpeed;
        public float maxSpeed;
        
        public float fovAngle;

        public float smoothDamp;
        
        public float deltaTime;

        public int flockSize;

        public void Execute(int index)
        {
            // find neighbours
            int cohesionIndex = 0;
            int avoidanceIndex = 0;
            int alignmentIndex = 0;

            Vector3 currentUnitPosition = unitPositions[index];

            int flockIndex = index / flockSize;
            int indexOffset = flockIndex * flockSize;

            for (int i = 0; i < flockSize; i++)
            {
                Vector3 currentNeighbourPosition = unitPositions[indexOffset + i];

                if (currentUnitPosition != currentNeighbourPosition)
                {
                    float currentDistanceToNeighbourSqr = Vector3.SqrMagnitude(currentUnitPosition - currentNeighbourPosition);

                    if (currentDistanceToNeighbourSqr <= cohesionDistance * cohesionDistance)
                    {
                        cohesionNeighbours[cohesionIndex] = currentNeighbourPosition;
                        neighbourSpeeds[cohesionIndex] = allUnitsSpeeds[indexOffset + i];
                        cohesionIndex++;
                    }

                    if (currentDistanceToNeighbourSqr <= alignmentDistance * alignmentDistance)
                    {
                        alignmentNeighbours[alignmentIndex] = currentNeighbourPosition;
                        alignmentNeighboursDirections[alignmentIndex] = unitForwardDirections[indexOffset + i];
                        alignmentIndex++;
                    }

                    if (currentDistanceToNeighbourSqr <= avoidanceDistance * avoidanceDistance)
                    {
                        avoidanceNeighbours[avoidanceIndex] = currentNeighbourPosition;
                        avoidanceIndex++;
                    }
                }
            }

            // calculate speed
            float speed = 0;
            if (cohesionNeighbours.Length != 0)
            {
                for (int i = 0; i < cohesionNeighbours.Length; i++)
                {      
                    speed += neighbourSpeeds[i];
                }
                speed /= cohesionNeighbours.Length;
            }
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            // calculate cohesion
            Vector3 cohesionVector = Vector3.zero;
            if (cohesionNeighbours.Length > 0)
            {
                int cohesionNeighboursInFov = 0;
                for (int i = 0; i < cohesionIndex; i++)
                {
                    if (IsInFov(unitForwardDirections[index], currentUnitPosition, cohesionNeighbours[i], fovAngle) && cohesionNeighbours[i] != Vector3.zero)
                    {
                        cohesionNeighboursInFov++;
                        cohesionVector += cohesionNeighbours[i];
                    }
                }

                cohesionVector /= cohesionNeighboursInFov;
                cohesionVector -= unitPositions[index];
                cohesionVector = cohesionVector.normalized * cohesionWeight;
            }

            // calculate avoidance
            Vector3 avoidanceVector = Vector3.zero;
            if (avoidanceNeighbours.Length > 0)
            {
                int avoidanceNeighboursInFov = 0;
                for (int i = 0; i < avoidanceNeighbours.Length; i++)
                {
                    if (IsInFov(unitForwardDirections[index], currentUnitPosition, avoidanceNeighbours[i], fovAngle) && avoidanceNeighbours[i] != Vector3.zero)
                    {
                        avoidanceNeighboursInFov++;
                        avoidanceVector += (currentUnitPosition - avoidanceNeighbours[i]);
                    }
                }

                avoidanceVector /= avoidanceNeighboursInFov;
                avoidanceVector = avoidanceVector.normalized * avoidanceWeight;
            }

            // calculate alignment
            Vector3 alignmentVector = Vector3.zero;
            if (alignmentNeighbours.Length > 0)
            {
                int alignmentNeighboursInFov = 0;
                for (int i = 0; i < alignmentNeighbours.Length; i++)
                {
                    if (IsInFov(unitForwardDirections[index], currentUnitPosition, alignmentNeighbours[i], fovAngle) && alignmentNeighbours[i] != Vector3.zero)
                    {
                        alignmentNeighboursInFov++;
                        alignmentVector += alignmentNeighboursDirections[i];
                    }
                }

                alignmentVector /= alignmentNeighboursInFov;
                alignmentVector = alignmentVector.normalized * alignmentWeight;
            }

            // calculate bounds
            Vector3 distanceToCenter = flockPositions[(int)(index / flockSize)] - currentUnitPosition;
            Vector3 boundsVector = (distanceToCenter.sqrMagnitude < boundsDistance * boundsDistance) ? Vector3.zero : distanceToCenter.normalized * boundsWeight;

            // move unit
            Vector3 currentVelocity = unitCurrentVelocities[index];
            Vector3 moveVector = cohesionVector + avoidanceVector + alignmentVector + boundsVector;
            moveVector = Vector3.SmoothDamp(unitForwardDirections[index], moveVector, ref currentVelocity, smoothDamp, 1000, deltaTime);
            moveVector = moveVector.normalized * speed;
            if(moveVector == Vector3.zero)
            {
                moveVector = unitForwardDirections[index];
            }

            unitPositions[index] = unitPositions[index] + moveVector * deltaTime;
            unitForwardDirections[index] = moveVector.normalized;
            allUnitsSpeeds[index] = speed;
            unitCurrentVelocities[index] = currentVelocity;
        }

        private bool IsInFov(Vector3 forward, Vector3 unitPos, Vector3 targetPos, float angle)
        {
            return Vector3.Angle(forward, targetPos - unitPos) <= angle;
        }
    }
}