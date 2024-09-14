using UnityEngine;
using AS.Modules.Stating.Core;
using System.Collections.Generic;

namespace AS.Modules.Stating.Tests
{
    public class SampleAsyncState : AsyncState<SampleStateTarget>
    {
        protected override void AddSubStates(List<State<SampleStateTarget>> states) { }

        protected override void OnEnterAsyncLeaf()
        {
            Debug.Log($"On Enter Async State. Target: {Target}");
        }

        protected override void OnExitAsyncLeaf()
        {
            Debug.Log($"On Exit Async State. Target: {Target}");
        }
    }
}
