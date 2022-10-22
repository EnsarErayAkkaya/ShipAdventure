using Cysharp.Threading.Tasks;
using EEA.Utils;
using System.Threading;
//using System.Threading;
using UnityEngine;
namespace EEA.FSM
{
    public class BaseStateMachine : MonoBehaviour
    {
        [SerializeField] protected BaseState initialState;

        protected BaseState currentState;
        protected bool working;

        protected CancellationTokenSource cancellationTokenSource;

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            MachineOnEnable();   
        }

        private void OnDisable()
        {
            MachineOnDisable();
        }
        protected virtual void MachineOnEnable()
        {

        }
        protected virtual void MachineOnDisable()
        {
            StopMachine();
        }
        protected virtual void Init()
        {
            currentState = initialState;
            currentState.EnterState(this);

            TaskUtil.RefreshToken(ref cancellationTokenSource, this.GetCancellationTokenOnDestroy());

            working = true;
            RunStateMachine();
        }

        /// <summary>
        /// if Machine working execute next state
        /// </summary>
        private async void RunStateMachine()
        {
            try
            {
                while (working)
                {
                    BaseState nextState = await currentState.ExecuteState(this, cancellationTokenSource.Token);

                    if (nextState != null)
                    {
                        SwitchState(nextState);
                    }
                    await UniTask.Yield(cancellationTokenSource.Token);
                }
            }
            catch(System.Exception e)
            {
                Debug.Log(e);
            }
        }

        /// <summary>
        /// Switches to next state
        /// </summary>
        /// <param name="nextState"></param>
        private void SwitchState(BaseState nextState)
        {
            currentState?.ExitState(this);
            currentState = nextState;
            currentState?.EnterState(this);
        }
        public virtual void StopMachine()
        {
            TaskUtil.RefreshToken(ref cancellationTokenSource, this.GetCancellationTokenOnDestroy());
            working = false;
        }
        public virtual void StartMachine(BaseState state = null)
        {
            if(state != null)
            {
                currentState = state;
                currentState.EnterState(this);
            }

            working = true;
            RunStateMachine();
        }

        public virtual void ForceSwitchState(BaseState state)
        {
            TaskUtil.RefreshToken(ref cancellationTokenSource, this.GetCancellationTokenOnDestroy());
            SwitchState(state);
        }
    }
}