using Steel;

namespace SteelCustom.Spells
{
    public class HealWave : Spell
    {
        public override void Cast(Vector3 position)
        {
            Entity.Destroy();

            foreach (Hero hero in GameManager.Player.Heroes)
            {
                if (!hero.Entity.IsDestroyed() && hero.Health < hero.MaxHealth)
                {
                    hero.Heal(15);
                }
            }
        }
    }
}