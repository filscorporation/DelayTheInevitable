using System.Collections;
using Steel;

namespace SteelCustom
{
    public class Hero : Unit
    {
        public static float AttackSpeedMultiplier = 1.0f;
        
        public string Name;

        public float MovementRange = 5.0f;
        public float ExpFromAttack = 1.0f;
        public float Experience = 0.0f;
        public int Level = 1;

        public const int LEVEL2_EXP = 10;
        public const int LEVEL3_EXP = 30;

        public Vector3 Root;
        
        protected bool canAttack = true;
        private Entity tooltip;

        public int ExperienceForNextLevel
        {
            get
            {
                switch (Level)
                {
                    case 1:
                        return LEVEL2_EXP;
                    case 2:
                        return LEVEL3_EXP;
                    default:
                        return -1;
                }
            }
        }
        
        public override void OnCreate()
        {
            Body = Entity.AddComponent<RigidBody>();
            
            Animator animator = Entity.AddComponent<Animator>();
            animator.Play("Idle");
        }

        public override void OnUpdate()
        {
            ActiveUpdate();
        }

        public override void OnMouseExit()
        {
            UITooltip.HideTooltip(tooltip);
        }

        public override void OnMouseOver()
        {
            UITooltip.HideTooltip(tooltip);
            tooltip = UITooltip.ShowTooltip(this);
        }

        protected override void MoveToTarget()
        {
            if (Vector3.Distance(Target.Entity.Transformation.Position, Root) > MovementRange)
            {
                Rotate(Root);
                SetVelocity(Vector2.Zero);
                return;
            }
            
            base.MoveToTarget();
        }
        
        protected override void NoTarget()
        {
            SetVelocity((Root - Transformation.Position).Normalize() * Speed);
            Rotate(Root);
        }

        protected override void TryAttack()
        {
            TryUseSkill();
            
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }

        protected virtual void TryUseSkill()
        {
            
        }

        protected override void FindTarget()
        {
            Enemy target = null;
            float minDist = ViewRange;
            foreach (Enemy unit in GameManager.EnemyManager.Units)
            {
                float dist = Vector3.Distance(unit.Transformation.Position, Transformation.Position);
                if (dist < minDist)
                {
                    minDist = dist;
                    target = unit;
                }
            }

            Target = target;
        }

        private IEnumerator Attack()
        {
            if (!IsTargetValid())
            {
                canAttack = true;
                yield break;
            }

            canAttack = false;
            
            GetComponent<Animator>().Play("Attack");
            
            yield return new WaitForSeconds(AttackNormalTime / AttackSpeedMultiplier);
            
            GetComponent<Animator>().Play("Idle");
            
            if (!IsTargetValid())
            {
                canAttack = true;
                yield break;
            }
            
            DoAttack();
            
            yield return new WaitForSeconds((AttackDelay - AttackNormalTime) / AttackSpeedMultiplier);

            canAttack = true;
        }

        protected virtual void DoAttack()
        {
            Vector2 force = (Target.Transformation.Position - Transformation.Position).Normalize() * AttackKnockBack;
            Target.TakeDamage(AttackDamage, force);

            GainExpFromAttack();
            
            GetComponent<AudioSource>().Play(AttackAudio);
        }

        protected void GainExpFromAttack()
        {
            Experience += ExpFromAttack;
            if (Level == 1 && Experience >= LEVEL2_EXP)
            {
                LevelUp();
            }
            if (Level == 2 && Experience >= LEVEL3_EXP)
            {
                LevelUp();
            }
        }

        protected virtual void LevelUp()
        {
            Level++;
            MaxHealth = Math.Round(MaxHealth * 1.5f);
            AttackDamage = Math.Round(AttackDamage * 1.5f);
            Health = MaxHealth;

            Entity entity = ResourcesManager.LoadAsepriteData("level_up_effect.aseprite", true)
                .CreateEntityFromAsepriteData();
            entity.Transformation.Position = Transformation.Position + new Vector3(0, 0, 1);
            entity.GetComponent<Animator>().Play("Idle");
            entity.Destroy(2.0f);
            
            entity.AddComponent<AudioSource>().Play(ResourcesManager.LoadAudioTrack("upgrade.wav"));
        }

        protected override void Die()
        {
            base.Die();

            GameManager.Player.Heroes.Remove(this);
        }
    }
}