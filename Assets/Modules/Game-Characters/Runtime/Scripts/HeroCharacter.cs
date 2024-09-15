using System.Collections.Generic;
using UnityEngine;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    public class HeroCharacter : Character
    {
        private Collider[] m_Results = new Collider[10];

        private List<EnemyCharacter> m_Enemies = new List<EnemyCharacter>();
        private List<RangedEnemy> m_RangedEnemies = new List<RangedEnemy>();
        private List<MeleeEnemy> m_MeleeEnemies = new List<MeleeEnemy>();

        public override void Action(IReadOnlyList<Transform> path)
        {
            FindEnemies();
            bool haveMeleeEnemay = HaveMeleeEnemay(out m_MeleeEnemies);
            bool haveRangedEnemy = HaveRangedEnemy(out m_RangedEnemies);

            OnFinishAttack += OnFinishAttackHero;

            if (haveMeleeEnemay && haveRangedEnemy)
            {
                if (HaveReadyEnemyToAttack(m_MeleeEnemies))
                {
                    Attack(FindClosestMeleeEnemies(m_MeleeEnemies));
                }
                else
                {
                    Attack(FindLowestHealth(m_Enemies));
                }
            }
            else if (haveRangedEnemy)
            {
                Attack(FindLowestHealth(m_RangedEnemies));
            }
            else if (haveMeleeEnemay)
            {
                Attack(FindClosestMeleeEnemies(m_MeleeEnemies));
            }
        }

        private void OnFinishAttackHero()
        {
            OnFinishAttack -= OnFinishAttackHero;

            InvokeFinishTurn();
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

                EnemyCharacter enemyCharacter = m_Results[i].GetComponent<EnemyCharacter>();

                if (enemyCharacter != null && enemyCharacter.IsAlive)
                {
                    m_Enemies.Add(enemyCharacter);
                }
            }
        }

        private bool HaveRangedEnemy(out List<RangedEnemy> rangedEnemies)
        {
            bool result = false;
            rangedEnemies = new List<RangedEnemy>();

            for (int i = 0; i < m_Enemies.Count; i++)
            {
                if (m_Enemies[i] is RangedEnemy rangedEnemy && rangedEnemy.IsAlive)
                {
                    result = true;
                    rangedEnemies.Add(rangedEnemy);
                }
            }

            return result;
        }

        private bool HaveMeleeEnemay(out List<MeleeEnemy> meleeEnemies)
        {
            bool result = false;
            meleeEnemies = new List<MeleeEnemy>();

            for (int i = 0; i < m_Enemies.Count; i++)
            {
                if (m_Enemies[i] is MeleeEnemy meleeEnemy && meleeEnemy.IsAlive)
                {
                    result = true;
                    meleeEnemies.Add(meleeEnemy);
                }
            }

            return result;
        }

        private bool HaveReadyEnemyToAttack(List<MeleeEnemy> meleeEnemies)
        {
            for (int i = 0; i < meleeEnemies.Count; i++)
            {
                if (meleeEnemies[i].CanAttackInNextTurn)
                {
                    return true;
                }
            }

            return false;
        }

        private T FindLowestHealth<T>(List<T> enemies) where T : Character
        {
            float lowestHealth = float.MaxValue;
            T result = null;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Health < lowestHealth)
                {
                    lowestHealth = enemies[i].Health;
                    result = enemies[i];
                }
            }

            return result;
        }

        private EnemyCharacter FindClosestMeleeEnemies(List<MeleeEnemy> enemies)
        {
            EnemyCharacter result = null;

            int minStep = int.MaxValue;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].TurnCount < minStep)
                {
                    minStep = enemies[i].TurnCount;
                    result = enemies[i];
                }
            }

            return result;
        }
    }
}
