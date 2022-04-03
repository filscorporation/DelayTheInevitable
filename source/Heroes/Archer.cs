using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steel;

namespace SteelCustom.Heroes
{
    public class Archer : Hero
    {
        private bool canUseLevel3Skill = true;

        protected override AudioTrack AttackAudio => ResourcesManager.LoadAudioTrack("arrow_hit.wav");
        
        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();
        }

        public override void OnMouseExit()
        {
            base.OnMouseExit();
        }

        public override void OnMouseOver()
        {
            base.OnMouseOver();
        }

        protected override void TryAttack()
        {
            if (!canAttack)
            {
                // TODO: try to move from enemies
            }
            
            base.TryAttack();
        }

        protected override void DoAttack()
        {
            Vector2 force = (Target.Transformation.Position - Transformation.Position).Normalize() * AttackKnockBack;
            Entity target = Target.Entity;

            Entity entity = new Entity("Arrow");
            entity.AddComponent<SpriteRenderer>().Sprite = ResourcesManager.LoadImage("arrow.png");
            entity.Transformation.Position = Transformation.Position;
            Vector2 vector = Target.Transformation.Position - Transformation.Position;
            float angle = -Math.Atan2(vector.X, vector.Y);
            entity.Transformation.Rotation = new Vector3(0, 0, angle);
            
            entity.AddComponent<Projectile>().Init(Target.Transformation.Position, 10,
                () =>
                {
                    if (!target.IsDestroyed())
                    {
                        Target.TakeDamage(AttackDamage, force);
                        
                        GetComponent<AudioSource>().Play(AttackAudio);
                    }
                });
            
            GainExpFromAttack();
        }

        protected override void TryUseSkill()
        {
            if (Level >= 3 && canUseLevel3Skill && canAttack)
            {
                StartCoroutine(UseLevel3Skill());
            }
        }

        private IEnumerator UseLevel3Skill()
        {
            canAttack = false;
            canUseLevel3Skill = false;

            Entity entity = ResourcesManager.LoadAsepriteData("piercing_arrow.aseprite").CreateEntityFromAsepriteData();
            Vector3 vector = (Target.Transformation.Position - Transformation.Position).SetZ(0.0f).Normalize();
            entity.Transformation.Position = Transformation.Position + vector * 4 + new Vector3(0, 0, 1.0f);
            Vector3 farTarget = Transformation.Position + vector * 8;
            float angle = -Math.Atan2(vector.X, vector.Y);
            entity.Transformation.Rotation = new Vector3(0, 0, angle);
            entity.GetComponent<Animator>().Play("Idle");
            entity.Destroy(1.0f);

            yield return new WaitForSeconds(0.2f);

            Vector2 force = (Target.Transformation.Position - Transformation.Position).Normalize() * AttackKnockBack;
            List<RayCastHit> hits = Physics.LineCast(Transformation.Position, farTarget).ToList();
            foreach (RayCastHit hit in hits)
            {
                if (hit.Entity.HasComponent<Enemy>())
                {
                    hit.Entity.GetComponent<Enemy>().TakeDamage(AttackDamage * 1.5f, force);
                }
            }
            
            GetComponent<AudioSource>().Play(ResourcesManager.LoadAudioTrack("pierce_shot.wav"));
            
            yield return new WaitForSeconds(1.5f);
            canAttack = true;
            
            yield return new WaitForSeconds(8f);
            canUseLevel3Skill = true;
        }
    }
}