using System;
using System.Collections.Generic;
using UnityEngine;
using AS.Modules.CoreCharacter;
using AS.Modules.GameStates;

namespace AS.Modules.Gameplay
{
    [DefaultExecutionOrder(0)]
    internal class GameplayController : MonoBehaviour
    {
        [SerializeField] private Game m_Game;

        internal static GameplayController Instance { private set; get; }

        internal IReadOnlyList<Character> Enemies => m_Game.Enemies;
        internal event Action OnGameStart;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        internal void InvokeStartGame()
        {
            OnGameStart?.Invoke();
        }

    }
}
