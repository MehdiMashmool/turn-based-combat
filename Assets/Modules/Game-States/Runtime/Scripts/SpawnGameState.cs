using UnityEngine;
using AS.Modules.Stating.Core;
using System.Collections.Generic;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameStates
{
    internal class SpawnGameState : GameState
    {
        [SerializeField] private Transform[] m_SpawnPoints;
        [SerializeField] private List<Path> m_Paths;
        [SerializeField] private Character[] m_Enemies;
        [SerializeField] private Character m_Player;
        [SerializeField] private int m_MaxSpawnCount = 3;

        protected override void AddSubStates(List<State<Game>> states) { }

        protected override void OnEnterAsyncLeaf()
        {
            Target.SetPlayer(m_Player);
            for (int i = 0; i < m_Paths.Count; i++)
            {
                Target.AddPath(m_Paths[i]);
            }

            int spawnCount = Random.Range(1, m_MaxSpawnCount + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                Character enemay = m_Enemies[Random.Range(0, m_Enemies.Length)];
                Transform point = m_SpawnPoints[i];
                Target.AddEnemy(Instantiate(enemay, point.position, point.rotation));
            }
        }

        protected override void OnExitAsyncLeaf() { }
    }
}
