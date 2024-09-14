using UnityEngine;
using AS.Modules.Stating.Core;

namespace AS.Modules.Stating.Tests
{
    public class SampleSyncState : SyncState<SampleStateTarget>
    {
        protected override void OnEnterSyncLeaf()
        {
            Debug.Log($"On Enter Sync State. Target: {Target}");
        }

        protected override void OnExitSyncLeaf()
        {
            Debug.Log($"On Exit Sync State. Target: {Target}");
        }
    }
}
