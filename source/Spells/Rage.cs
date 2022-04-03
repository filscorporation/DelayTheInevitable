using Steel;

namespace SteelCustom.Spells
{
    public class Rage : Spell
    {
        public override void Cast(Vector3 position)
        {
            Hero.AttackSpeedMultiplier *= 2.0f;
            Entity.Destroy(5.0f);

            foreach (Hero hero in GameManager.Player.Heroes)
            {
                if (!hero.Entity.IsDestroyed())
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("rage_effect.aseprite", true).CreateEntityFromAsepriteData();
                    entity.Transformation.Position = hero.Transformation.Position;
                    entity.GetComponent<Animator>().Play("Idle");
                    
                    entity.Destroy(1.0f);
                }
            }
        }

        public override void OnDestroy()
        {
            Hero.AttackSpeedMultiplier /= 2.0f;
        }
    }
}