using System.Collections.Generic;
using UnityEngine;
using AS.Modules.Stating.Core;

namespace AS.Modules.GameStates
{
    internal class GeneralGameState : GameStateTree
    {
        [SerializeField] private SpawnGameState m_SpawnGameState;
        [SerializeField] private TurnGameState m_TurnGameState;
        [SerializeField] private FinishGameState m_FinishGameState;

        protected override void AddSubStates(List<State<Game>> states)
        {
            states.Add(m_SpawnGameState);
            states.Add(m_TurnGameState);
            states.Add(m_FinishGameState);
        }

        protected override void OnEnterTree()
        {
            m_SpawnGameState.OnFinish += OnFinishSpawn;
            Switch(m_SpawnGameState);

        }

        private void OnFinishSpawn()
        {
            m_SpawnGameState.OnFinish -= OnFinishSpawn;

            m_TurnGameState.OnAllEnemiesDie += OnAllEnemiesDie;
            m_TurnGameState.OnPlayerDie += OnPlayerDie;

            Switch(m_TurnGameState);

        }

        private void OnPlayerDie()
        {
            Switch(m_FinishGameState);
        }

        private void OnAllEnemiesDie()
        {
            Switch(m_FinishGameState);
        }

        protected override void OnExitTree()
        {
            m_SpawnGameState.OnFinish -= OnFinishSpawn;
            m_TurnGameState.OnAllEnemiesDie -= OnAllEnemiesDie;
            m_TurnGameState.OnPlayerDie -= OnPlayerDie;
        }
    }
}
