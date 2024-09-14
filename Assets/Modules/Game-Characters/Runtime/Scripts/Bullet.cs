using System;
using UnityEngine;
using System.Collections;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    internal class Bullet : MonoBehaviour
    {
        [SerializeField] private float m_Speed = 50f;

        internal event Action<Bullet> OnCollisionTarget;

        protected Character Shooter { private set; get; }
        protected Character Target { private set; get; }

        private Coroutine m_Moving;

        internal void Shoot(Character shooter, Character target, Vector3 direction)
        {
            Shooter = shooter;
            Target = target;

            if (m_Moving != null)
            {
                StopCoroutine(m_Moving);
            }

            m_Moving = StartCoroutine(Moving(direction));
        }

        private IEnumerator Moving(Vector3 direction)
        {
            while (true)
            {
                transform.position += direction * m_Speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
                yield return new WaitForFixedUpdate();
            }
        }

        protected void InvokeCollisionTarget()
        {
            OnCollisionTarget?.Invoke(this);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            IAttackTarget target = other.GetComponent<IAttackTarget>();

            if (target != null && (IAttackTarget)Target == target)
            {
                target.Target.ApplayDamage(Shooter.AttackPower);
                InvokeCollisionTarget();
                Destroy(this.gameObject);
            }
        }
    }
}
