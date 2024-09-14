using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterAnimation))]
    public class CharacterFighter : MonoBehaviour
    {
        [SerializeField] private float m_Power = 25f;

        internal float Power => m_Power;

        private CharacterAnimation m_Animation;
        private CharacterMover m_Mover;

        private void Start()
        {
            m_Animation = GetComponent<CharacterAnimation>();
            m_Mover = GetComponent<CharacterMover>();
        }

        internal void TryAttack()
        {
            m_Animation.Attack();
        }

        public virtual void Attack(Character shooter, Character enemy) 
        {
            m_Mover.UpdateRotation(enemy.transform.position);
        }
    }
}
