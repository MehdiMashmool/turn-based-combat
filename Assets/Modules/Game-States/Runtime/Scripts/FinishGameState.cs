using System;
using System.Collections.Generic;
using UnityEngine;
using AS.Modules.Stating.Core;

namespace AS.Modules.GameStates
{
    public class FinishGameState : GameState
    {
        public event Action<bool> OnFinish;

        protected override void AddSubStates(List<State<Game>> states) { }

        protected override void OnEnterAsyncLeaf()
        {
            OnFinish?.Invoke(Target.Player.IsAlive);

            if (Target.Player.IsAlive)
            {
                Debug.Log("[FinishGameState] Win");
            }
            else
            {
                Debug.Log("[FinishGameState] Lose");
            }
        }

        protected override void OnExitAsyncLeaf() { }
    }
}
