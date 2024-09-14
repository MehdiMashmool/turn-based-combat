using System;

namespace AS.Modules.Stating.Core
{
    public abstract class SyncState<T> : State<T> where T : Target
    {
        protected sealed override void OnInitialize()
        {
            OnInitializeSyncState();
        }

        protected virtual void OnInitializeSyncState() { }

        protected override void OnEnter()
        {
            OnEnterSyncLeaf();
        }

        protected override void OnExit()
        {
            OnExitSyncLeaf();
        }

        protected abstract void OnEnterSyncLeaf();

        protected abstract void OnExitSyncLeaf();
    }
}
