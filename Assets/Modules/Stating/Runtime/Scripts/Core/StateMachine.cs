using System;

namespace AS.Modules.Stating.Core
{
    public struct StateMachine
    {
        public event Action<IRunnable, IRunnable> OnChangeState;

        private IRunnable m_CurrentState;

        public void SwitchState(IRunnable runnable)
        {
            if(runnable == m_CurrentState)
            {
                return;
            }

            m_CurrentState?.Exit();
            runnable?.Enter();
            m_CurrentState = runnable;
        }
    }
}
