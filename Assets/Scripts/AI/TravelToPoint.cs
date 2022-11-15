using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
namespace EEA.Enemy
{
    public class TravelToPoint : MonoBehaviour
    {
        [SerializeField] private float reachedTarget;
        [SerializeField] private float minAngleForMaxTurnSpeed = 30;

        public bool Travel(ShipMovement shipMovement, Vector3 point)
        {
            shipMovement.SetSail(1);

            Vector3 diff = (point - shipMovement.transform.position);
            Vector3 targetVector = diff.normalized;

            float angleDiff = Vector3.SignedAngle(shipMovement.transform.forward, targetVector, Vector3.up);

            float wheel = Mathf.Sign(angleDiff) * Mathf.Clamp01(Mathf.Abs(angleDiff).Remap(0.0f, minAngleForMaxTurnSpeed, .0f, 1.0f));
            
            //Debug.Log("angleDiff: " + angleDiff + ", wheelTurnAmount: " + wheel);

            shipMovement.SetWheel(wheel);

            return diff.magnitude <= reachedTarget;
        }

        public void SetAngle(ShipMovement shipMovement, Vector3 angleVector)
        {
            shipMovement.SetSail(0);

            Vector3 targetVector = angleVector;

            float angleDiff = Vector3.SignedAngle(shipMovement.transform.forward, targetVector, Vector3.up);

            float wheel = Mathf.Sign(angleDiff) * Mathf.Clamp01(Mathf.Abs(angleDiff).Remap(0.0f, minAngleForMaxTurnSpeed, .0f, 1.0f));

            shipMovement.SetWheel(wheel);

        }

        private bool IsPathBlocked(Vector3 startPos, Vector3 endPos)
        {
            return Physics.Linecast(startPos, endPos);   
        }
    }
}