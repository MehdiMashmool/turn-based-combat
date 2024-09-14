using System.Collections;
using UnityEngine;
using TMPro;
using System;

namespace AS.Modules.Gameplay
{
    internal class CoinRewrad : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Reward;
        [SerializeField] private float m_Speed = 250f;

        internal event Action<CoinRewrad> OnReach;

        private Coroutine m_Moving;

        internal CoinRewrad SetReward(int amount)
        {
            m_Reward.text = amount.ToString();
            return this;
        }

        internal CoinRewrad MoveTo(Vector3 from, Vector3 to)
        {
            if(m_Moving != null)
            {
                StopCoroutine(m_Moving);
            }

            StartCoroutine(Moving(from, to));

            return this;
        }

        private IEnumerator Moving(Vector3 from, Vector3 to)
        {
            transform.position = from;
            while (transform.position != to)
            {
                transform.position = Vector3.MoveTowards(transform.position, to, m_Speed * Time.deltaTime);
                yield return null;
            }

            m_Moving = null;
            OnReach?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
