using UnityEngine;

namespace AS.Modules.CoreCharacter
{
    [RequireComponent(typeof(CharacterAnimation))]
    internal class CharacterFighter : MonoBehaviour
    {
        [SerializeField] private float m_Power = 25f;
        [SerializeField] private Transform m_SpawnPoint;
        [SerializeField] private Bullet m_BulletPrefab;

        private CharacterAnimation m_Animation;
        private CharacterHealth m_Health;

        private void Start()
        {
            m_Animation = GetComponent<CharacterAnimation>();
            m_Health = GetComponent<CharacterHealth>();
        }

        internal void Attack(Character shooter, Character enemy)
        {
            m_Animation.Attack();
            Vector3 direction = enemy.transform.position - shooter.transform.position;
            Debug.DrawRay(shooter.transform.position, direction, Color.red, 10);
            Bullet bullet = Instantiate(m_BulletPrefab, m_SpawnPoint);
            bullet.Shoot(direction);
        }

        internal void ApplayDamageWithAttack()
        {
            m_Health.ApplayDamage(m_Power);
        }
    }
}
