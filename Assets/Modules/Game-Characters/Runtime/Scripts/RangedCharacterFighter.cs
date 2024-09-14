using UnityEngine;
using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    internal class RangedCharacterFighter : CharacterFighter
    {
        [SerializeField] private Transform m_SpawnPoint;
        [SerializeField] private Bullet m_BulletPrefab;

        public override void Attack(Character shooter, Character enemy)
        {
            Vector3 direction = enemy.transform.position - shooter.transform.position;
            Bullet bullet = Instantiate(m_BulletPrefab, m_SpawnPoint);
            bullet.Shoot(shooter, direction);
        }
    }
}
