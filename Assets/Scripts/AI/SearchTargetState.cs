using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EEA.FSM;
using Cysharp.Threading.Tasks;
using System.Threading;
using EEA.General;
using EEA.Ship;
using EEA.Managers;
using EEA.Pathfinding;
using System.Linq;

namespace EEA.Enemy
{
    public class SearchTargetState : BaseState
    {
        [SerializeField] private AttackState attackState;
        [SerializeField] private LootState lootState;

        [SerializeField] private ShipMovement ship;

        [Header("Search Settings")]
        [SerializeField] private int searchFrameInterval = 7;
        [SerializeField] private float checkRadius;
        [SerializeField] private LayerMask checkLayer;

        [SerializeField] private TravelToPoint travel;

        private Sea.SeaPos target;
        private Vector3[] path;
        private int pathIndex = 0;
        private Sea sea;
        private bool reachedTarget = true;
        private void Start()
        {
            sea = FindObjectOfType<Sea>(); 
        }

        public override void EnterState(BaseStateMachine stateMachine)
        {
            path = new Vector3[0];
            reachedTarget = true;
            pathIndex = 0;

            base.EnterState(stateMachine);
            if(ship.Anchor)
            {
                ship.ToggleAnchor();
            }
        }
        public override async UniTask<BaseState> ExecuteState(BaseStateMachine stateMachine, CancellationToken token = default)
        {
            var searchPerimeterResult = SearchPerimeter();

            if(searchPerimeterResult != this)
            {
                return searchPerimeterResult;
            }

            if (path != null && pathIndex < path.Length)
            {
                reachedTarget = travel.Travel(ship, path[pathIndex]);
            }

            if (reachedTarget)
            {
                pathIndex++;
                if(path == null || pathIndex >= path.Length)
                {
                    target = sea.GetRandomAreaOnSea();
                    if(target.success)
                    {
                        pathIndex = 0;
                        PathRequestManager.RequestPath(ship.transform.position, target.pos, OnPathFound);
                        reachedTarget = false;
                    }
                }
            }

            await UniTask.Yield();

            return null;
        }

        private void OnPathFound(Vector3[] path, bool success)
        {
            if(success)
            {
                this.path = path;
            }
        }

        private BaseState SearchPerimeter()
        {
            if (Time.frameCount % searchFrameInterval == 0)
            {
                var colliders = CheckPerimeter.Check(ship.Collider, ship.transform, checkRadius, checkLayer);

                if (colliders.Count > 0)
                {
                    IAITarget closest = null;
                    float closestDistance = float.MaxValue;

                    foreach (var collider in colliders)
                    {
                        float _dist = Vector3.Distance(collider.transform.position, ship.transform.position);
                        if (collider.TryGetComponent<IAITarget>(out IAITarget _target) && (_dist < closestDistance))
                        {
                            closestDistance = _dist;
                            closest = _target;
                        }
                    }

                    if (closest != null && closest.GetAITargetTranform != null && closest.IsTargetValid(ship))
                    {
                        switch (closest.TargetType)
                        {
                            case AITargetType.Ship:
                            {
                                Debug.Log("found target: " + closest.GetAITargetTranform.name);
                                attackState.SetTarget(closest);
                                return attackState;
                            }
                            case AITargetType.Island:
                            {
                                Debug.Log("found target: " + closest.GetAITargetTranform.name);
                                lootState.SetTarget(closest);
                                return lootState;
                            }
                            case AITargetType.Loot:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return this;
        }

        private void OnDrawGizmos()
        {
            if (isActiveState && target.success)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(target.pos, Vector3.one);
                if (path != null)
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
}