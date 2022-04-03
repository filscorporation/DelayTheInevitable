using Steel;

namespace SteelCustom.Spells
{
    public class Blizzard : Spell
    {
        public override void Cast(Vector3 position)
        {
            const float FREEZE_DURATION = 5.0f;
            
            Enemy.IsFrozen.Add(true);
            Entity.Destroy(FREEZE_DURATION);

            foreach (Enemy unit in GameManager.EnemyManager.Units)
            {
                if (!unit.Entity.IsDestroyed())
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("freeze_effect.aseprite").CreateEntityFromAsepriteData();
                    entity.Transformation.Position = unit.Transformation.Position.SetZ(2.0f);
                    
                    entity.Destroy(FREEZE_DURATION);
                }
            }
        }

        public override void OnDestroy()
        {
            Enemy.IsFrozen.RemoveAt(0);
        }
    }
}