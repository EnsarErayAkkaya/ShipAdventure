using Cysharp.Threading.Tasks;
using EEA.FSM;
using EEA.General;
using EEA.Ship;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace EEA.Enemy
{
    public class AttackState : BaseState
    {
        [SerializeField] private SearchTargetState searchTarget;
        [SerializeField] private float maxAttackDistance;
        [SerializeField] private float minAttackAngle;
        [SerializeField] private ShipMovement ship;
        [SerializeField] private ShipCannonShoot shipCannonShoot;
        [SerializeField] private TravelToPoint travelToPoint;

        private IAITarget target;
        private Vector3 diff;
        private Vector3 dir;
        private float dist;

        private Vector3 attackPos;
        private Vector3 attackDir;
        public async override UniTask<BaseState> ExecuteState(BaseStateMachine stateMachine, CancellationToken token = default)
        {
            if (target == null || !target.IsTargetValid(ship))
            {
                return searchTarget;
            }
            CalculateAttackPos();
            await UniTask.Yield();
            return null;
        }

        private void CalculateAttackPos()
        {
            diff = target.GetAITargetTranform.position - ship.transform.position;
            dir = diff.normalized;
            dist = diff.magnitude;

            if(dist < maxAttackDistance) // already in range
            {
                // calculate angle
                CalculateAttackAngle();

                float angleDiffBetweenAttackAngle = Mathf.Abs(Vector3.SignedAngle(ship.transform.forward, attackDir, Vector3.up));
                
                //Debug.DrawLine(ship.transform.position + attackDir.normalized + Vector3.up, ship.transform.position + (attackDir.normalized * 2) + Vector3.up, Color.magenta);
                //Debug.Log("angleDiffBetweenAttackAngle: " + angleDiffBetweenAttackAngle);
                
                if (angleDiffBetweenAttackAngle < minAttackAngle) // if close to designated attack angle
                {
                    float angle = Vector3.SignedAngle(ship.transform.forward, dir, Vector3.up);
                    //Debug.Log("angle: " + angle);

                    if (angle < 0) // shoot left cannons
                    {
                        shipCannonShoot.ShootLeftCannons();
                    }
                    else // shoot right cannons
                    {
                        shipCannonShoot.ShootRightCannons();
                    }
                }
            }
            else // outside range
            {
                // get in range
                attackPos = target.GetAITargetTranform.position - (dir * maxAttackDistance);
                travelToPoint.Travel(ship, attackPos);
                //Debug.Log("AttackState: Travel to point");
            }
        }

        private void CalculateAttackAngle()
        {
            // calculate both angles
            Vector3 leftAttackDir = Quaternion.AngleAxis(90, Vector3.up) * dir;
            Vector3 rightAttackDir = Quaternion.AngleAxis(-90, Vector3.up) * dir;
            
            // set aattack angle to closer one
            if(Vector3.SignedAngle(leftAttackDir, transform.forward, Vector3.up) < Vector3.SignedAngle(rightAttackDir, transform.forward, Vector3.up))
            {
                attackDir = leftAttackDir;
            }
            else
            {
                attackDir = rightAttackDir;
            }

            travelToPoint.SetAngle(ship, attackDir);

            //Debug.Log("AttackState: Rotate to angle");
        }

        public void SetTarget(IAITarget ship)
        {
            target = ship;
        }

        private void OnDrawGizmos()
        {
            if (isActiveState)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(attackPos, Vector3.one);
            }
        }
    }
}