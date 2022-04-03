using Steel;

namespace SteelCustom.Spells
{
    public class Meteor : Spell
    {
        public override void Cast(Vector3 position)
        {
            Entity entity = ResourcesManager.LoadAsepriteData("meteor.aseprite", true).CreateEntityFromAsepriteData();
            Vector3 start = Camera.Main.ScreenToWorldPoint(
                new Vector2(Screen.Width, Screen.Height)
            );
            entity.Transformation.Position = start;
            entity.Transformation.Position = entity.Transformation.Position.SetZ(2.5f);
            
            Vector2 vector = position - start;
            float angle = -Math.Atan2(vector.X, vector.Y) - Math.Pi * 0.75f;
            entity.Transformation.Rotation = new Vector3(0, 0, angle);
            
            MeteorEffect effect = entity.AddComponent<MeteorEffect>();
            effect.Start = start;
            effect.Target = position;
            effect.Duration = 0.1f * Vector3.Distance(position, start);
            entity.GetComponent<Animator>().Play("Idle");
            
            Entity.Destroy();
        }
    }
}