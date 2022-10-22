using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Ship
{
    public class ShipBodyMovement : MonoBehaviour
    {
        [SerializeField] private ShipMovement shipMovement;
        [SerializeField] private Transform mesh;
        [SerializeField] private float angleLimit;
        [SerializeField] private float damping = 1;

        private float speed;
        private float acceleration;
        private float resultRotation;

        private void FixedUpdate()
        {
            float bodyAngle = transform.localRotation.eulerAngles.z % 360;
            if (bodyAngle > 180)
            {
                bodyAngle -= 360;
            }

            acceleration = (shipMovement.WheelAngle * (shipMovement.Anchor ? 0 : 1)) ; // acceleration will drasticly fall when ship becomes straight

            speed = (acceleration * damping) + acceleration;

            resultRotation = bodyAngle + speed;
            resultRotation = Mathf.Clamp(resultRotation, -angleLimit, angleLimit);

            mesh.localRotation = Quaternion.Euler(0, 0, resultRotation);
        }
    }
}