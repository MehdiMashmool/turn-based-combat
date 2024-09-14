using UnityEngine;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    [RequireComponent(typeof(CharacterFighter))]
    internal class RangedCharacterVFX : CharacterVFX
    {
        [SerializeField] private ParticleSystem m_Attack;

        private CharacterFighter m_Fighter;

        protected override void Awake()
        {
            base.Awake();
            m_Fighter = GetComponent<CharacterFighter>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Fighter.OnAttack += OnAttack;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_Fighter.OnAttack -= OnAttack;
        }

        private void OnAttack()
        {
            m_Attack.Play(true);
        }
    }
}
