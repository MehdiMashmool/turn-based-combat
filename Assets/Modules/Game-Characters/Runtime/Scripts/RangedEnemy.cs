using UnityEngine;
using System.Collections.Generic;

namespace AS.Modules.GameCharacters
{
    internal class RangedEnemy : EnemyCharacter
    {
        public override void Action(IReadOnlyList<Transform> path)
        {
            AttackPlayer();
        }

        protected override void OnAttackAnimationFinish()
        {
            InvokeFinishTurn();
        }
    }
}
