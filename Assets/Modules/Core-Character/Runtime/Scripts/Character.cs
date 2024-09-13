using System.Collections;
using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterAnimation))]
    [RequireComponent(typeof(CharacterHealth))]
    [RequireComponent(typeof(CharacterMover))]
    public class Character : MonoBehaviour
    {
        private CharacterHealth m_Health;
        private CharacterAnimation m_Animation;
        private CharacterMover m_Mover;

        private void Start()
        {
            m_Health = GetComponent<CharacterHealth>();
            m_Animation = GetComponent<CharacterAnimation>();
            m_Mover = GetComponent<CharacterMover>();
        }

        [SerializeField] private Transform m_Target;
        [ContextMenu("S")]
        public void S()
        {
            Move(m_Target.position);
        }

        public virtual void Move(Vector3 target)
        {
            m_Mover.OnReachTarget -= OnReachTarget;
            m_Mover.OnReachTarget += OnReachTarget;

            m_Mover.Move(target);
            m_Animation.StartMove();
        }

        private void OnReachTarget()
        {
            m_Mover.OnReachTarget -= OnReachTarget;

            m_Animation.StopMove();
        }
    }
}
