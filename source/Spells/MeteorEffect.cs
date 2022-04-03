using Steel;

namespace SteelCustom.Spells
{
    public class MeteorEffect : ScriptComponent
    {
        public Vector3 Start;
        public Vector3 Target;
        public float Duration;

        private float timer = 0.0f;

        public override void OnUpdate()
        {
            timer += Time.DeltaTime;

            Transformation.Position = Math.Lerp(Start, Target, timer / Duration).SetZ(2.5f);

            if (timer >= Duration)
            {
                Entity entity = ResourcesManager.LoadAsepriteData("explosion.aseprite").CreateEntityFromAsepriteData();
                entity.GetComponent<Animator>().Play("Idle");
                entity.Transformation.Position = Transformation.Position.SetZ(1.5f);
                entity.Destroy(2.0f);
                
                entity.AddComponent<AudioSource>().Play(ResourcesManager.LoadAudioTrack("explosion.wav"));
                
                Camera.Main.GetComponent<CameraController>().Shake(0.7f);

                const float RADIUS = 1.2f;
                const float DAMAGE = 50;

                foreach (Entity hit in Physics.AABBCast(Target, new Vector2(RADIUS, RADIUS)))
                {
                    if (hit.HasComponent<Enemy>())
                    {
                        Enemy enemy = hit.GetComponent<Enemy>();
                        enemy.TakeDamage(DAMAGE, Vector2.Zero);
                    }
                }
                
                Entity.Destroy();
            }
        }
    }
}