using Cysharp.Threading.Tasks;
using EEA.FSM;
using EEA.General;
using EEA.Managers;
using EEA.Pathfinding;
using EEA.Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
namespace EEA.Enemy
{
    public class LootState : BaseState
    {
        [SerializeField] private SearchTargetState searchState;
        [SerializeField] private ShipMovement ship;
        [SerializeField] private TravelToPoint travelToPoint;

        private Vector3[] path;
        private IAITarget target;
        private int pathIndex = 0;
        private bool reachedTarget = false;

        public override void EnterState(BaseStateMachine stateMachine)
        {
            base.EnterState(stateMachine);

            path = new Vector3[0];
            pathIndex = 0;
            reachedTarget = false;

            PathRequestManager.RequestPath(ship.transform.position, target.GetAITargetTranform.position, OnPathFound);

            if (ship.Anchor)
            {
                ship.ToggleAnchor();
            }
        }

        public override async UniTask<BaseState> ExecuteState(BaseStateMachine stateMachine, CancellationToken token = default)
        {
            if(path.Length > 0)
            {
                // go to destination
                if (path != null && pathIndex < path.Length)
                {
                    reachedTarget = travelToPoint.Travel(ship, path[pathIndex]);
                }
            }

            if (reachedTarget)
            {
                pathIndex++;
                if (path == null || pathIndex >= path.Length)
                {
                    // reached loot
                    await UniTask.Delay(1000);
                    return searchState;
                }
            }

            await UniTask.Yield();

            return null;
        }

        private void OnPathFound(Vector3[] path, bool success)
        {
            if (success)
            {
                this.path = path;
            }
        }

        public void SetTarget(IAITarget target)
        {
            this.target = target;
        }

        private void OnDrawGizmos()
        {
            if (isActiveState && path != null)
            {
                for (int i = pathIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);
                }
            }  
        }
    }
}