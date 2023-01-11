using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Visuals
{
    public class FlockUnit : MonoBehaviour
    {
        private Transform mTransform;
        private Vector3 currentVelocity;
        private float speed;

        public Transform Transform => mTransform;
        public Vector3 CurrentVelocity { get => currentVelocity; set => currentVelocity = value; }
        public float CurrentSpeed { get => speed; set => speed = value; }
        private void Awake()
        {
            mTransform = transform;
        }

        public void InitializeSpeed(float speed)
        {
            this.speed = speed;
        }

        /*public void MoveUnit()
        {
            FindNeighbours();
            CalculateSpeed();
            Vector3 cohesionVector = CalculateCohesionVector() * assignedFlock.CohesionWeight;
            Vector3 avoidanceVector = CalculateAvoidanceVector() * assignedFlock.AvoidanceWeight;
            Vector3 alignmentVector = CalculateAlignmentVector() * assignedFlock.AlignmentWeight;
            Vector3 boundsVector = CalculateBoundsVector() * assignedFlock.BoundsWeight;

            Vector3 moveVector = cohesionVector + avoidanceVector + alignmentVector + boundsVector;

            moveVector = Vector3.SmoothDamp(mTransform.forward, moveVector, ref currentVelocity, smoothDamp);
            moveVector = moveVector.normalized * speed;
            
            if(moveVector == Vector3.zero)
                moveVector = transform.forward;

            mTransform.forward = moveVector;
            mTransform.position += moveVector * Time.deltaTime;
        }

        private Vector3 CalculateBoundsVector()
        {
            Vector3 centerVector = assignedFlock.transform.position - mTransform.position;
            bool isNearCenter = centerVector.sqrMagnitude < (assignedFlock.BoundsDistance * assignedFlock.BoundsDistance);

            return isNearCenter ? Vector3.zero : centerVector;
        }

        private Vector3 CalculateAlignmentVector()
        {
            Vector3 alignmentVector = mTransform.forward;
            if(alignementNeighbours.Count == 0)
            {
                return alignmentVector;
            }

            int neighboursInFov = 0;
            for (int i = 0; i < alignementNeighbours.Count; i++)
            {
                if (IsInFov(alignementNeighbours[i].mTransform.position))
                {
                    neighboursInFov++;
                    alignmentVector += alignementNeighbours[i].mTransform.forward;
                }
            }

            if (neighboursInFov == 0)
                return alignmentVector;

            alignmentVector /= neighboursInFov;
            return alignmentVector.normalized;
        }

        private Vector3 CalculateAvoidanceVector()
        {
            Vector3 avoidanceVector = mTransform.forward;
            if (alignementNeighbours.Count == 0)
            {
                return avoidanceVector;
            }

            int neighboursInFov = 0;
            for (int i = 0; i < alignementNeighbours.Count; i++)
            {
                if (IsInFov(alignementNeighbours[i].mTransform.position))
                {
                    neighboursInFov++;
                    avoidanceVector += mTransform.position - alignementNeighbours[i].mTransform.position;
                }
            }

            if (neighboursInFov == 0)
                return avoidanceVector;

            avoidanceVector /= neighboursInFov;
            return avoidanceVector.normalized;
        }

        private void FindNeighbours()
        {
            cohesionNeighbours.Clear();
            alignementNeighbours.Clear();
            avoidanceNeighbours.Clear();

            FlockUnit[] allUnits = assignedFlock.AllUnits;

            for (int i = 0; i < allUnits.Length; i++)
            {
                FlockUnit unit = allUnits[i];
                if(unit != this)
                {
                    float currentNeighbourDistance = Vector3.SqrMagnitude(unit.mTransform.position - mTransform.position);
                    if(currentNeighbourDistance <= assignedFlock.CohesionDistance * assignedFlock.CohesionDistance)
                    {
                        cohesionNeighbours.Add(unit);
                    }

                    if (currentNeighbourDistance <= assignedFlock.AlignmentDistance * assignedFlock.AlignmentDistance)
                    {
                        alignementNeighbours.Add(unit);
                    }

                    if (currentNeighbourDistance <= assignedFlock.AvoidanceDistance * assignedFlock.AvoidanceDistance)
                    {
                        avoidanceNeighbours.Add(unit);
                    }
                }
            }
        }

        private void CalculateSpeed()
        {
            if (cohesionNeighbours.Count == 0)
                return;

            speed = 0;
            for (int i = 0; i < cohesionNeighbours.Count; i++)
            {
                speed += cohesionNeighbours[i].speed;
            }

            speed /= Math.Max(cohesionNeighbours.Count, 1);
            speed = Mathf.Clamp(speed, assignedFlock.MinSpeed, assignedFlock.MaxSpeed);
        }

        private Vector3 CalculateCohesionVector()
        {
            Vector3 cohesionVector = Vector3.zero;

            if (cohesionNeighbours.Count == 0)
                return cohesionVector;

            int neighbourInFov = 0;

            for (int i = 0; i < cohesionNeighbours.Count; i++)
            {
                if (IsInFov(cohesionNeighbours[i].mTransform.position))
                {
                    neighbourInFov++;
                    cohesionVector += cohesionNeighbours[i].mTransform.position;
                }
            }
            if (neighbourInFov == 0)
                return cohesionVector;

            cohesionVector /= neighbourInFov;
            cohesionVector -= mTransform.position;
            return cohesionVector.normalized;
        }

        private bool IsInFov(Vector3 position)
        {
            return Vector3.Angle(mTransform.forward, position - mTransform.position) <= fovAngle;
        }*/
    }
}