using System;
using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    internal class CharacterHealth : MonoBehaviour
    {
        [SerializeField] private CharacterHUD m_CharacterHUD;
        [SerializeField] private float m_MaxHealth = 100f;

        internal event Action OnDie;
        internal event Action<float> OnChangeHealth;

        internal float Health { private set; get; }
        internal bool IsAlive => Health > 0;

        private void Start()
        {
            Health = m_MaxHealth;
            UpdateHealth();
        }

        internal void ApplayDamage(float damageAmount)
        {
            Health -= damageAmount;
            Health = Mathf.Clamp(Health, 0, m_MaxHealth);
            OnChangeHealth?.Invoke(Health);
            CheckDie();
            UpdateHealth();
        }

        private void CheckDie()
        {
            if (Health <= 0)
            {
                OnDie?.Invoke();
            }
        }

        private void UpdateHealth()
        {
            m_CharacterHUD.UpdateBar(Health / m_MaxHealth).UpdateText(Health);
        }
    }
}
