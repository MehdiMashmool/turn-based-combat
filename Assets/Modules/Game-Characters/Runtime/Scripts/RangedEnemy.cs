using UnityEngine;
using System.Collections.Generic;

namespace AS.Modules.GameCharacters
{
    internal class RangedEnemy : EnemyCharacter
    {
        public override void Action(IReadOnlyList<Transform> path)
        {
            if (IsAlive)
            {
                AttackPlayer();
            }
            else
            {
                InvokeFinishTurn();
            }
        }

        protected override void OnAttackAnimationFinish()
        {
            InvokeFinishTurn();
        }
    }
}
