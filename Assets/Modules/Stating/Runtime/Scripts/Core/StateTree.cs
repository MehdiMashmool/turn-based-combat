using AS.Modules.Stating.Core;
using System.Collections.Generic;

namespace AS.Modules.Stating
{
    public abstract class StateTree<T> : State<T> where T : Target
    {
        private StateMachine m_Machine;
        private List<State<T>> m_SubStates = new List<State<T>>();

        protected State<T> CurrentState { private set; get; }

        protected sealed override void OnInitialize()
        {
            AddSubStates(m_SubStates);

            for (int i = 0; i < m_SubStates.Count; i++)
            {
                m_SubStates[i].Initialize(Target);
            }

            OnInitializeStateTree();
        }

        protected sealed override void OnEnter()
        {
            OnEnterTree();
        }

        protected sealed override void OnExit()
        {
            OnExitTree();
        }

        protected void Switch(State<T> subState)
        {
            if(CurrentState == subState)
            {
                return;
            }

            m_Machine.SwitchState(subState);
            CurrentState = subState;
        }

        protected virtual void OnInitializeStateTree() { }

        protected abstract void AddSubStates(List<State<T>> states);

        protected abstract void OnEnterTree();

        protected abstract void OnExitTree();
    }
}
