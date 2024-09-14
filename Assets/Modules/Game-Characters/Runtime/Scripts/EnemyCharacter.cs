using UnityEngine;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    internal class EnemyCharacter : Character 
    {
        private Collider[] m_Results = new Collider[10];

        protected void AttackPlayer()
        {
            Physics.OverlapSphereNonAlloc(transform.position, float.MaxValue, m_Results);

            for (int i = 0; i < m_Results.Length; i++)
            {
                if (m_Results[i] == null)
                {
                    continue;
                }

                HeroCharacter heroCharacter = m_Results[i].GetComponent<HeroCharacter>();
                if (heroCharacter != null)
                {
                    Attack(heroCharacter);
                }
            }
        }
    }
}
