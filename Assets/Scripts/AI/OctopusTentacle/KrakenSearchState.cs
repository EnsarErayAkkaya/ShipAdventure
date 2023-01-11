using Cysharp.Threading.Tasks;
using EEA.FSM;
using EEA.General;
using EEA.Managers;
using EEA.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace EEA.Enemy.Kraken
{
    public class KrakenSearchState : BaseState
    {
        [SerializeField] private KrakenAI krakenAI;

        [SerializeField] private float reachedTargetDist;
        [SerializeField] private float speed;
        [SerializeField] private float height;
        [SerializeField] private float startAttackDuration;

        [SerializeField] private KrakenAttackState krakenAttackState;

        private float remainingAttackDuration;
        private Sea.SeaPos target;
        private Sea sea;
        private bool reachedTarget = true;

        private void Start()
        {
            sea = FindObjectOfType<Sea>();
            remainingAttackDuration = startAttackDuration;
        }

        public override async UniTask<BaseState> ExecuteState(BaseStateMachine stateMachine, CancellationToken token = default)
        {
            var state = CheckAnyShipInsideAttackArea();

            if(state != null)
            {
                return state;
            }

            if (target.success)
            {
                transform.position += (target.pos - transform.position).normalized * speed * Time.deltaTime;

                if ((target.pos - transform.position).sqrMagnitude < (reachedTargetDist * reachedTargetDist))
                {
                    reachedTarget = true;
                }
            }

            if (reachedTarget)
            {
                reachedTarget = false;
                target = sea.GetRandomAreaOnSea();
                target.pos.y = height;
            }

            await UniTask.Yield(token);

            return null;
        }

        private BaseState CheckAnyShipInsideAttackArea()
        {
            bool shipInside = krakenAI.CheckAnyShipInsideAttackArea();

            if (shipInside)
            {
                remainingAttackDuration -= Time.deltaTime;

                if (remainingAttackDuration <= 0)
                {
                    return krakenAttackState;
                }
            }
            else
            {
                remainingAttackDuration = startAttackDuration;
            }

            return null;
        }
    }
}