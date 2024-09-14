using System.Collections.Generic;
using UnityEngine;
using AS.Modules.CoreCharacter;
using TMPro;

namespace AS.Modules.Gameplay
{
    [DefaultExecutionOrder(1)]
    internal class CoinCollectController : MonoBehaviour
    {
        [SerializeField] private int m_CoinRewardAmout = 100;
        [SerializeField] private CoinRewrad m_CoinRewardPrefab;
        [SerializeField] private TextMeshProUGUI m_CoinAmountText;
        [SerializeField] private Transform m_MainCanvas;
        [SerializeField] private Transform m_Target;

        private GameplayController GameplayController => GameplayController.Instance;

        private int m_CoinAmount = 0;

        private List<Character> m_DeadEnemies = new List<Character>();

        private void Start()
        {
            GameplayController.OnGameStart += OnGameStart;

            m_CoinAmountText.text = m_CoinAmount.ToString();
        }

        private void OnDestroy()
        {
            GameplayController.OnGameStart -= OnGameStart;
        }

        private void OnGameStart()
        {
            GameplayController.OnGameStart -= OnGameStart;

            for (int i = 0; i < GameplayController.Enemies.Count; i++)
            {
                GameplayController.Enemies[i].OnDie += OnDie;
            }
        }

        private void OnDie()
        {
            Character enemy = GetDiedEnemy();
            CoinRewrad coinReward = Instantiate(m_CoinRewardPrefab, m_MainCanvas);
            coinReward.OnReach += OnReach;
            Vector3 from = Camera.main.WorldToScreenPoint(enemy.transform.position);
            Vector3 to = m_Target.position;
            coinReward.SetReward(m_CoinRewardAmout).MoveTo(from, to);
            m_DeadEnemies.Add(enemy);
        }

        private void OnReach(CoinRewrad coinReward)
        {
            coinReward.OnReach -= OnReach;

            m_CoinAmount += m_CoinRewardAmout;
            m_CoinAmountText.text = m_CoinAmount.ToString();
        }

        private Character GetDiedEnemy()
        {
            for (int i = 0; i < GameplayController.Enemies.Count; i++)
            {
                if (!GameplayController.Enemies[i].IsAlive)
                {
                    if (!m_DeadEnemies.Contains(GameplayController.Enemies[i]))
                    {
                        return GameplayController.Instance.Enemies[i];
                    }
                }
            }

            return null;
        }
    }
}
