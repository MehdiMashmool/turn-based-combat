using System;
using UnityEngine;
using AS.Modules.CoreCharacter;
using AS.Modules.Stating.Core;
using System.Collections.Generic;

namespace AS.Modules.GameStates
{
    internal class Game : Target 
    {
        internal IReadOnlyList<Character> Enemies => m_Enemies;
        internal IReadOnlyList<Path> Paths => m_Paths;
        internal Character Player => m_Player;

        private List<Character> m_Enemies = new List<Character>();
        private List<Path> m_Paths = new List<Path>();
        private Character m_Player;

        internal void AddEnemy(Character enemy)
        {
            m_Enemies.Add(enemy);
        }

        internal void RemoveEnemy(Character enemy)
        {
            m_Enemies.Remove(enemy);
        }

        internal void SetPlayer(Character player)
        {
            m_Player = player;
        }

        internal void AddPath(Path path)
        {
            m_Paths.Add(path);
        }
    }

    [Serializable]
    internal class Path
    {
        [SerializeField] internal Transform[] Paths;
    }
}
