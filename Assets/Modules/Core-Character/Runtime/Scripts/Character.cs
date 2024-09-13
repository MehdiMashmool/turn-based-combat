using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterAnimation))]
    [RequireComponent(typeof(CharacterHealth))]
    [RequireComponent(typeof(CharacterMover))]
    [RequireComponent(typeof(CharacterFighter))]
    public class Character : MonoBehaviour, IBulletTarget
    {
        public Character Target => this;

        private CharacterHealth m_Health;
        private CharacterAnimation m_Animation;
        private CharacterMover m_Mover;
        private CharacterFighter m_Fighter;

        private void Start()
        {
            m_Health = GetComponent<CharacterHealth>();
            m_Animation = GetComponent<CharacterAnimation>();
            m_Mover = GetComponent<CharacterMover>();
            m_Fighter = GetComponent<CharacterFighter>();
        }

        public void Move(Vector3 target)
        {
            m_Mover.OnReachTarget -= OnReachTarget;
            m_Mover.OnReachTarget += OnReachTarget;

            m_Mover.Move(target);
            m_Animation.StartMove();
        }

        public void Attack(Character target) =>
            m_Fighter.Attack(this, target);

        public void ApplayDamageWithAttack() =>
            m_Fighter.ApplayDamageWithAttack();

        /// <summary>
        /// Action in turn.
        /// </summary>
        public virtual void Action() { }

        private void OnReachTarget()
        {
            m_Mover.OnReachTarget -= OnReachTarget;

            m_Animation.StopMove();
        }
    }
}
