using UnityEngine;
using System.Collections;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    internal class Bullet : MonoBehaviour
    {
        [SerializeField] private float m_Speed = 50f;

        private Character m_Shooter;

        internal void Shoot(Character shooter, Vector3 direction)
        {
            m_Shooter = shooter;
            StartCoroutine(Moving(direction));
        }

        private IEnumerator Moving(Vector3 direction)
        {
            while (true)
            {
                transform.position += direction * m_Speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
                yield return null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IAttackTarget target = other.GetComponent<IAttackTarget>();

            if (target != null)
            {
                target.Target.ApplayDamage(m_Shooter.AttackPower);
            }

            Destroy(this.gameObject);
        }
    }
}
