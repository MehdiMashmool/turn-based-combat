using System.Collections.Generic;
using UnityEngine;
using AS.Modules.Stating.Core;

namespace AS.Modules.GameStates
{
    internal class FinishGameState : GameState
    {
        protected override void AddSubStates(List<State<Game>> states) { }

        protected override void OnEnterAsyncLeaf()
        {
            if (Target.Player.IsAlive)
            {
                Debug.Log("Win");
            }
            else
            {
                Debug.Log("Lose");
            }
        }

        protected override void OnExitAsyncLeaf() { }
    }
}
