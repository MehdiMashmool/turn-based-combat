using UnityEngine;
using System.Collections;

namespace AS.Modules.CoreCharacter
{
    internal class Bullet : MonoBehaviour
    {
        [SerializeField] private float m_Speed = 50f;

        internal void Shoot(Vector3 direction)
        {
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
            IBulletTarget target = other.GetComponent<IBulletTarget>();

            if (target != null)
            {
                target.Target.ApplayDamageWithAttack();
            }

            Destroy(this.gameObject);
        }
    }
}
