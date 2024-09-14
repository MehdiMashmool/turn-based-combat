using System;
using System.Collections.Generic;
using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterAnimation))]
    [RequireComponent(typeof(CharacterHealth))]
    [RequireComponent(typeof(CharacterMover))]
    [RequireComponent(typeof(CharacterFighter))]
    [DefaultExecutionOrder(-1)]
    public class Character : MonoBehaviour, IAttackTarget
    {
        public event Action<Character> OnFinishTurn;
        public event Action OnDie
        {
            add => m_Health.OnDie += value;
            remove => m_Health.OnDie -= value;
        }
        public event Action<Character> OnReachTarget;

        public Character Target => this;
        public float AttackPower => m_Fighter.Power;
        public float Health => m_Health.Health;
        public bool IsAlive => m_Health.IsAlive;

        private CharacterHealth m_Health;
        private CharacterAnimation m_Animation;
        private CharacterMover m_Mover;
        private CharacterFighter m_Fighter;

        private Character m_AttackTarget;

        private void Awake()
        {
            Initialize();
        }

        public void Move(Vector3 target)
        {
            m_Mover.OnReachTarget -= OnReachedToTarget;
            m_Mover.OnReachTarget += OnReachedToTarget;

            m_Mover.Move(target);
            m_Animation.StartMove();
        }

        public void Attack(Character target)
        {
            m_AttackTarget = target;
            m_Fighter.TryAttack();
        }

        public void ApplayDamage(float damage) =>
            m_Health.ApplayDamage(damage);

        /// <summary>
        /// Action in turn.
        /// </summary>
        public virtual void Action(IReadOnlyList<Transform> path) { }

        internal void InvokeFinishTurn()
        {
            OnFinishTurn?.Invoke(this);
        }

        protected virtual void OnAttackAnimationFinish() 
        {
            InvokeFinishTurn();
        }

        private void Initialize()
        {
            m_Health = GetComponent<CharacterHealth>();
            m_Animation = GetComponent<CharacterAnimation>();
            m_Mover = GetComponent<CharacterMover>();
            m_Fighter = GetComponent<CharacterFighter>();
        }

        private void OnAttackAnimationEvent()
        {
            m_Fighter.Attack(this, m_AttackTarget);
        }

        private void OnAttackAnimationFinished()
        {
            OnAttackAnimationFinish();
        }

        private void OnReachedToTarget()
        {
            m_Mover.OnReachTarget -= OnReachedToTarget;

            OnReachTarget?.Invoke(this);
            m_Animation.StopMove();
        }
    }
}
