using Cysharp.Threading.Tasks;
using EEA.FSM;
using EEA.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace EEA.Enemy.Kraken
{
    public class KrakenAttackState : BaseState
    {
        [SerializeField] private GameObject attackAreaBlocker;
        [SerializeField] private float height;
        [SerializeField] private float riseSpeed;

        [SerializeField] private KrakenTentacle[] tentacles;
        public override void EnterState(BaseStateMachine stateMachine)
        {
            base.EnterState(stateMachine);

            attackAreaBlocker.SetActive(true);

            for (int i = 0; i < tentacles.Length; i++)
            {
                tentacles[i].AttackToPosition(tentacles[i].transform.forward * 3);
            }
        }

        public override async UniTask<BaseState> ExecuteState(BaseStateMachine stateMachine, CancellationToken token = default)
        {
            if(transform.position.y < height)
            {
                transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            }

            await UniTask.Yield(token);

            return null;
        }

        public override void ExitState(BaseStateMachine stateMachine)
        {
            base.ExitState(stateMachine);

            attackAreaBlocker.SetActive(false);
        }
    }
}