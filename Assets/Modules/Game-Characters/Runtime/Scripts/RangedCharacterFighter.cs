using UnityEngine;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    internal class RangedCharacterFighter : CharacterFighter
    {
        [SerializeField] private Transform m_SpawnPoint;
        [SerializeField] private Bullet m_BulletPrefab;
        [SerializeField] private Vector3 m_TargetOffset = new Vector3(0, 1, 0);

        public override void Attack(Character shooter, Character enemy)
        {
            base.Attack(shooter, enemy);
            Vector3 direction = (enemy.transform.position + m_TargetOffset) - m_SpawnPoint.transform.position;
            Bullet bullet = Instantiate(m_BulletPrefab, m_SpawnPoint.transform.position, m_SpawnPoint.transform.rotation);
            Debug.DrawRay(m_SpawnPoint.position, direction, Color.red, 1);
            bullet.Shoot(shooter, direction);
        }
    }
}
