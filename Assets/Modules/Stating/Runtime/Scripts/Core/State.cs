using UnityEngine;

namespace AS.Modules.Stating.Core
{
    public abstract class State<T> : MonoBehaviour, IRunnable where T : Target
    {
        protected T Target { get; private set; }

        private bool m_IsInitialized = false;

        public void Initialize(T target)
        {
            if(m_IsInitialized) return;

            Target = target;
            m_IsInitialized = true;
            OnInitialize();
        }

        public void Enter()
        {
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        protected abstract void OnEnter();

        protected abstract void OnExit();
    }
}
