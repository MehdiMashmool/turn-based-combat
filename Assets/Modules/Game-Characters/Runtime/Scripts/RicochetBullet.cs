using System.Collections.Generic;
using UnityEngine;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    internal class RicochetBullet : Bullet
    {
        [SerializeField] private float m_Chance = 20f;
        [SerializeField] private Vector3 m_TargetOffset = new Vector3(0, 1, 0);

        private Collider[] m_Results = new Collider[10];
        private List<EnemyCharacter> m_Enemies = new List<EnemyCharacter>();
        private bool m_IsRicocheted = false;

        protected override void OnTriggerEnter(Collider other)
        {
            IAttackTarget target = other.GetComponent<IAttackTarget>();

            if (target != null && (IAttackTarget)Target == target)
            {
                target.Target.ApplayDamage(Shooter.AttackPower);

                if (!m_IsRicocheted)
                {
                    if (!TryChance())
                    {
                        Cleanup();
                    }
                    else
                    {
                        m_IsRicocheted = true;
                    }
                }
                else
                {
                    Cleanup();
                }
            }
        }

        private void Cleanup()
        {
            InvokeCollisionTarget();
            Destroy(gameObject);
        }

        private bool TryChance()
        {
            float chance = Random.Range(0f, 100f);
            bool result = false;

            if (chance <= m_Chance)
            {
                FindEnemies();

                if (m_Enemies.Count > 0)
                {
                    Debug.Log("[RicochetBullet] Ricocheted!");
                    Ricochet();
                    result = true;
                }
            }

            return result;
        }

        private void Ricochet()
        {
            Character enemy = m_Enemies[Random.Range(0, m_Enemies.Count)];
            Vector3 direction = (enemy.transform.position + m_TargetOffset) - transform.transform.position;
            Shoot(Shooter, enemy, direction);
        }

        private void FindEnemies()
        {
            m_Enemies.Clear();
            Physics.OverlapSphereNonAlloc(transform.position, float.MaxValue, m_Results);

            for (int i = 0; i < m_Results.Length; i++)
            {
                if (m_Results[i] == null)
                {
                    continue;
                }

                EnemyCharacter enemy = m_Results[i].GetComponent<EnemyCharacter>();

                if (enemy != null && enemy != Target)
                {
                    m_Enemies.Add(enemy);
                }
            }
        }
    }
}
