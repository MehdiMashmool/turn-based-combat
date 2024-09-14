using AS.Modules.CoreCharacter;

namespace AS.Modules.GameCharacters
{
    public class MeleeCharacterFighter : CharacterFighter
    {
        public override void Attack(Character shooter, Character enemy)
        {
            base.Attack(shooter, enemy);
            enemy.ApplayDamage(shooter.AttackPower);
        }
    }
}
