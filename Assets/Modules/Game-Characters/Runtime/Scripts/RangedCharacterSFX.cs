using UnityEngine;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    [RequireComponent(typeof(CharacterFighter))]
    public class RangedCharacterSFX : CharacterSFX
    {
        [SerializeField] private AudioClip m_Attack;

        private CharacterFighter m_Fighter;

        protected override void Awake()
        {
            base.Awake();
            m_Fighter =  GetComponent<CharacterFighter>();
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
            PlayerAudio(m_Attack);
        }
    }
}
