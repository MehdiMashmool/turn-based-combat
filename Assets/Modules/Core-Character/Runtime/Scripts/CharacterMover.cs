using System;
using System.Collections;
using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    internal class CharacterMover : MonoBehaviour
    {
        [SerializeField] private float m_ReachThreshold = 0.4f;
        [SerializeField] private float m_MoveSpeed = 5;

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

        internal virtual void MoveTowards(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, m_MoveSpeed * Time.deltaTime);
        }

        private IEnumerator Moving(Vector3 target)
        {
            while (!IsReachedToTarget(target))
            {
                MoveTowards(target);
                yield return null;
            }

            OnReachTarget?.Invoke();
            m_Moving = null;
        }

        private bool IsReachedToTarget(Vector3 target) =>
            Vector3.Distance(transform.position, target) <= m_ReachThreshold;
    }
}
