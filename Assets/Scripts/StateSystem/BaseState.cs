using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace EEA.FSM
{
    public abstract class BaseState : MonoBehaviour
    {
        protected bool isActiveState;
        public virtual void EnterState(BaseStateMachine stateMachine)
        {
            isActiveState = true;
        }
        public abstract UniTask<BaseState> ExecuteState(BaseStateMachine stateMachine, CancellationToken token = default);
        public virtual void ExitState(BaseStateMachine stateMachine)
        {
            isActiveState = false;
        }
    }
}