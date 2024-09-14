using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterHealth))]
    [RequireComponent(typeof(AudioSource))]
    public class CharacterSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip m_Die;
        [SerializeField] private AudioClip m_Hit;

        private CharacterHealth m_Health;
        private AudioSource m_Source;

        protected virtual void Awake()
        {
            m_Health = GetComponent<CharacterHealth>();
            m_Source = GetComponent<AudioSource>();
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

        protected void PlayerAudio(AudioClip audio)
        {
            m_Source.clip = audio;
            m_Source.Play();
        }

        private void OnChangeHealth(float amount)
        {
            PlayerAudio(m_Hit);
        }

        private void OnDie()
        {
            PlayerAudio(m_Die);
        }
    }
}
