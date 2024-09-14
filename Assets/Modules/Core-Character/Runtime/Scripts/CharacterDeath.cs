using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterHealth))]
    internal class CharacterDeath : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_DeactiveObjects;
        [SerializeField] private float m_DestoryDelay = 3;

        private CharacterHealth m_Health;

        private void Awake()
        {
            m_Health = GetComponent<CharacterHealth>();
        }

        private void OnEnable()
        {
            m_Health.OnDie += OnDie;
        }

        private void OnDisable()
        {
            m_Health.OnDie -= OnDie;
        }

        private void OnDie()
        {
            for (int i = 0; i < m_DeactiveObjects.Length; i++)
            {
                m_DeactiveObjects[i].SetActive(false);
            }

            Destroy(gameObject, m_DestoryDelay);
        }
    }
}
