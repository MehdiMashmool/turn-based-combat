using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterHealth))]
    public class CharacterVFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_Die;
        [SerializeField] private ParticleSystem m_Hit;

        private CharacterHealth m_Health;

        protected virtual void Awake()
        {
            m_Health = GetComponent<CharacterHealth>();
        }

        protected virtual void OnEnable()
        {
            m_Health.OnDie += OnDie;
            m_Health.OnChangeHealth += OnChangeHealth;
        }

        protected virtual void OnDisable()
        {
            m_Health.OnDie -= OnDie;
            m_Health.OnChangeHealth -= OnChangeHealth;
        }

        private void OnDie()
        {
            m_Die.Play(true);
        }

        private void OnChangeHealth(float amount)
        {
            m_Hit.Play(true);
        }
    }
}
