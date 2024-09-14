using UnityEngine;
using AS.Modules.Stating.Core;
using System.Collections.Generic;
using AS.Modules.CoreCharacter;
using System;
using Random = UnityEngine.Random;

namespace AS.Modules.GameStates
{
    internal class SpawnGameState : GameState
    {
        [SerializeField] private Transform[] m_SpawnPoints;
        [SerializeField] private List<Path> m_Paths;
        [SerializeField] private Character[] m_Enemies;
        [SerializeField] private Character m_Player;
        [SerializeField] private int m_MaxSpawnCount = 3;

        internal event Action OnFinish;

        private int m_ReachedEnemyCount = 0;

        protected override void AddSubStates(List<State<Game>> states) { }

        protected override void OnEnterAsyncLeaf()
        {
            m_ReachedEnemyCount = 0;

            Target.SetPlayer(m_Player);
            for (int i = 0; i < m_Paths.Count; i++)
            {
                Target.AddPath(m_Paths[i]);
            }

            int spawnCount = Random.Range(1, m_MaxSpawnCount + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                Character enemy = m_Enemies[Random.Range(0, m_Enemies.Length)];
                Transform point = m_SpawnPoints[i];
                Character createdEnemy = Instantiate(enemy, point.position, point.rotation);
                //createdEnemy.Initialize();
                createdEnemy.OnReachTarget += OnReachTarget;
                createdEnemy.Move(Target.Paths[i].Paths[0].position);
                Target.AddEnemy(createdEnemy);
            }
        }

        private void OnReachTarget(Character enemy)
        {
            enemy.OnReachTarget -= OnReachTarget;

            m_ReachedEnemyCount++;

            if (m_ReachedEnemyCount >= Target.Enemies.Count)
            {
                OnFinish?.Invoke();
            }
        }

        protected override void OnExitAsyncLeaf() { }
    }
}
