using UnityEngine;
using System.Collections.Generic;

namespace AS.Modules.GameCharacters
{
    internal class MeleeEnemy : EnemyCharacter
    {
        internal bool CanAttackInNextTurn => m_TurnCount >= 3;
        internal int TurnCount => m_TurnCount;

        private int m_TurnCount = 0;

        public override void Action(IReadOnlyList<Transform> path)
        {
            if (m_TurnCount >= 3)
            {
                AttackPlayer();
            }
            else
            {
                Move(path[m_TurnCount].position);
                m_TurnCount++;
            }
        }
    }
}
