using Codice.CM.Common;
using System;
using System.Collections;
using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    internal class CharacterMover : MonoBehaviour
    {
        [SerializeField] private float m_ReachThreshold = 0.05f;
        [SerializeField] private float m_MoveSpeed = 5;
        [SerializeField] private float m_RotationSpeed = 120;

        internal event Action OnReachTarget;

        private Coroutine m_Moving;

        internal void Move(Vector3 target)
        {
            if (m_Moving != null)
            {
                StopCoroutine(m_Moving);
            }

            m_Moving = StartCoroutine(Moving(target));
        }

        protected virtual void MoveTowards(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, m_MoveSpeed * Time.deltaTime);
        }

        internal virtual void UpdateRotation(Vector3 target)
        {
            Vector3 direction = target - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
            }
        }

        private IEnumerator Moving(Vector3 target)
        {
            while (!IsReachedToTarget(target))
            {
                MoveTowards(target);
                UpdateRotation(target);
                yield return null;
            }

            OnReachTarget?.Invoke();
            m_Moving = null;
        }

        private bool IsReachedToTarget(Vector3 target) =>
            Vector3.Distance(transform.position, target) <= m_ReachThreshold;
    }
}
