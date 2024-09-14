using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(Animator))]
    internal class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private string m_Move = "IsMoving";
        [SerializeField] private string m_Attack = "Attack";

        private Animator m_Animator;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
        }

        internal void StartMove()
        {
            m_Animator.SetBool(m_Move, true);
        }

        internal void StopMove()
        {
            m_Animator.SetBool(m_Move, false);
        }

        internal void Attack()
        {
            m_Animator.SetTrigger(m_Attack);
        }
    }
}
