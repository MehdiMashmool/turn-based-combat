using System;
using System.Collections.Generic;
using AS.Modules.Stating.Core;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameStates
{
    internal class TurnGameState : GameState
    {
        internal event Action OnAllEnemiesDie;
        internal event Action OnPlayerDie;

        private int m_EnemayTurnIndex = 0;
        private int m_DiedEnemyCount = 0;

        protected override void AddSubStates(List<State<Game>> states) { }

        protected override void OnEnterAsyncLeaf()
        {
            m_EnemayTurnIndex = 0;
            m_DiedEnemyCount = 0;

            Target.Player.OnDie += OnPlayerDeath;
            for (int i = 0; i < Target.Enemies.Count; i++)
            {
                Target.Enemies[i].OnDie += OnEnemyDie;
            }

            Target.Player.OnFinishTurn += OnFinishPlayerTurn;
            Target.Player.Action();
        }

        protected override void OnExitAsyncLeaf()
        {
            Target.Player.OnFinishTurn -= OnFinishPlayerTurn;
            Target.Player.OnDie -= OnPlayerDeath;

            for (int i = 0; i < Target.Enemies.Count; i++)
            {
                Target.Enemies[i].OnFinishTurn -= OnFinishEnemyTurn;
                Target.Enemies[i].OnDie -= OnEnemyDie;
            }
        }

        private void OnFinishPlayerTurn(Character player)
        {
            Target.Player.OnFinishTurn -= OnFinishPlayerTurn;

            HandleEnemyTurn();
        }

        private void HandleEnemyTurn()
        {
            Character enemy = Target.Enemies[m_EnemayTurnIndex];
            enemy.OnFinishTurn += OnFinishEnemyTurn;
            enemy.Action();
        }

        private void OnFinishEnemyTurn(Character enemy)
        {
            enemy.OnFinishTurn -= OnFinishEnemyTurn;

            m_EnemayTurnIndex++;

            if(m_EnemayTurnIndex >= Target.Enemies.Count)
            {
                m_EnemayTurnIndex = 0;

                Target.Player.OnFinishTurn += OnFinishPlayerTurn;
                Target.Player.Action();
            }
            else
            {
                HandleEnemyTurn();
            }
        }

        private void OnEnemyDie()
        {
            m_DiedEnemyCount++;

            if(m_DiedEnemyCount >= Target.Enemies.Count)
            {
                OnAllEnemiesDie?.Invoke();
            }
        }

        private void OnPlayerDeath()
        {
            OnPlayerDie?.Invoke();
        }
    }
}
