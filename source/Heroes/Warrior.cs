using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steel;

namespace SteelCustom.Heroes
{
    public class Warrior : Hero
    {
        private bool canUseLevel3Skill = true;

        protected override AudioTrack AttackAudio => ResourcesManager.LoadAudioTrack("hit.wav");

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

        protected override void TryUseSkill()
        {
            if (Level >= 3 && canUseLevel3Skill && canAttack)
            {
                StartCoroutine(UseLevel3Skill());
            }
        }

        private IEnumerator UseLevel3Skill()
        {
            List<Enemy> targets = new List<Enemy>();

            foreach (Enemy enemy in GameManager.EnemyManager.Units)
            {
                if (Vector3.Distance(enemy.Transformation.Position, enemy.Transformation.Position) <= ViewRange)
                {
                    if (enemy.ForcedTarget == null)
                    {
                        targets.Add(enemy);
                        enemy.SetForcedTarget(this);
                    }
                }
            }
            
            if (!targets.Any())
                yield break;
            
            canAttack = false;
            canUseLevel3Skill = false;

            Entity entity = ResourcesManager.LoadAsepriteData("taunt_effect.aseprite").CreateEntityFromAsepriteData();
            entity.Transformation.Position = Transformation.Position.SetZ(1.5f);
            entity.GetComponent<Animator>().Play("Idle");
            entity.Destroy(1.0f);

            yield return new WaitForSeconds(1.0f);
            canAttack = true;
            
            yield return new WaitForSeconds(2.0f);

            foreach (Enemy enemy in targets)
            {
                enemy.ForcedTarget = null;
            }

            yield return new WaitForSeconds(14f);
            canUseLevel3Skill = true;
        }
    }
}