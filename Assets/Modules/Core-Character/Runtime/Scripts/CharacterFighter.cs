using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterAnimation))]
    public class CharacterFighter : MonoBehaviour
    {
        [SerializeField] private float m_Power = 25f;

        internal float Power => m_Power;

        private CharacterAnimation m_Animation;
        private CharacterHealth m_Health;

        private void Start()
        {
            m_Animation = GetComponent<CharacterAnimation>();
            m_Health = GetComponent<CharacterHealth>();
        }

        internal void TryAttack()
        {
            m_Animation.Attack();
        }

        public virtual void Attack(Character shooter, Character enemy) { }
    }
}
