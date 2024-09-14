using System.Collections.Generic;

namespace AS.Modules.Stating.Core
{
    public abstract class AsyncState<T> : State<T> where T : Target
    {
        private List<State<T>> m_States = new List<State<T>>();

        protected sealed override void OnInitialize()
        {
            AddSubStates(m_States);

            for (int i = 0; i < m_States.Count; i++)
            {
                m_States[i].Initialize(Target);
            }

            OnInitializeAsyncState();
        }

        protected sealed override void OnEnter()
        {
            OnEnterAsyncLeaf();
            for (int i = 0; i < m_States.Count; i++)
            {
                m_States[i].Enter();
            }
        }

        protected sealed override void OnExit()
        {
            OnExitAsyncLeaf();
            for (int i = 0; i < m_States.Count; i++)
            {
                m_States[i].Exit();
            }
        }

        protected virtual void OnInitializeAsyncState() { }

        protected abstract void AddSubStates(List<State<T>> states);

        protected abstract void OnEnterAsyncLeaf();

        protected abstract void OnExitAsyncLeaf();
    }
}
