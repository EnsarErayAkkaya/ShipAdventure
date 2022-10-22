using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EEA.FSM;
using Cysharp.Threading.Tasks;
using System.Threading;
using EEA.General;
using EEA.Ship;
using EEA.Managers;

namespace EEA.Enemy
{
    public class SearchTargetState : BaseState
    {
        [SerializeField] private AttackState attackState;

        [SerializeField] private Collider shipCollider;
        [SerializeField] private ShipMovement ship;

        [Header("Search Settings")]
        [SerializeField] private int searchFrameInterval = 7;
        [SerializeField] private float checkRadius;
        [SerializeField] private LayerMask checkLayer;

        [SerializeField] private TravelToPoint travel;

        private Sea.SeaPos target;
        private Sea sea;
        private bool reachedTarget;
        private int searchFrameCounter = 0;
        private void Start()
        {
            sea = FindObjectOfType<Sea>();
            Sea.SeaPos seaPos = sea.GetRandomAreaOnSea();
        }

        public override void EnterState(BaseStateMachine stateMachine)
        {
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

            if(target.success)
            {
                reachedTarget = travel.Travel(ship, target.pos);
            }

            if (reachedTarget)
            {
                target = sea.GetRandomAreaOnSea();
            }

            await UniTask.Yield();

            return this;
        }

        private BaseState SearchPerimeter()
        {
            if (searchFrameCounter == searchFrameInterval)
            {
                var colliders = CheckPerimeter.Check(shipCollider, ship.transform, checkRadius, checkLayer);

                if (colliders.Count > 0)
                {
                    ShipMovement closest = null;
                    float closestDistance = float.MaxValue;

                    foreach (var collider in colliders)
                    {
                        float _dist = Vector3.Distance(collider.transform.position, ship.transform.position);
                        if (collider.TryGetComponent<ShipMovement>(out ShipMovement _ship) && (_dist < closestDistance))
                        {
                            closestDistance = _dist;
                            closest = _ship;
                        }
                    }
                    if (closest != null)
                    {
                        Debug.Log("found target: " + closest.name);
                        attackState.SetTarget(closest);
                        return attackState;
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
            }
        }
    }
}