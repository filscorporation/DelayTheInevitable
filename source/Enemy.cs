using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steel;

namespace SteelCustom
{
    public class Enemy : Unit
    {
        public static readonly List<bool> IsFrozen = new List<bool>();

        public int EnemyWeight = 1;
        
        private bool canAttack = true;

        public Hero ForcedTarget;

        protected override AudioTrack AttackAudio => ResourcesManager.LoadAudioTrack("small_hit.wav");
        
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

        protected override void MoveToTarget()
        {
            if (IsFrozen.Any())
            {
                SetVelocity(Vector2.Zero);
                return;
            }
            
            //TODO: avoid others and move from the side
            
            base.MoveToTarget();
        }

        protected override void FindTarget()
        {
            if (ForcedTarget != null && !ForcedTarget.Entity.IsDestroyed())
                return;
            
            Hero target = null;
            float minDist = ViewRange;
            foreach (Hero playerHero in GameManager.Player.Heroes)
            {
                float dist = Vector3.Distance(playerHero.Transformation.Position, Transformation.Position);
                if (dist < minDist)
                {
                    target = playerHero;
                    minDist = dist;
                }
            }

            Target = target;
        }

        public void SetForcedTarget(Hero forcedTarget)
        {
            ForcedTarget = forcedTarget;
            Target = ForcedTarget;
        }

        protected override void NoTarget()
        {
            if (IsFrozen.Any())
            {
                SetVelocity(Vector2.Zero);
                return;
            }
            
            SetVelocity((GameManager.Player.Gate.Transformation.Position - Transformation.Position).Normalize() * Speed);
            Rotate(GameManager.Player.Gate.Transformation.Position);

            if (Vector3.Distance(GameManager.Player.Gate.Transformation.Position.SetZ(Transformation.Position.Z), Transformation.Position) < 1.1f)
            {
                AttackGate();
            }
        }

        private void AttackGate()
        {
            SetVelocity(Vector2.Zero);
            
            Entity.Destroy(0.2f);
            Alive = false;

            Entity entity = ResourcesManager.LoadAsepriteData("enter_gate_effect.aseprite")
                .CreateEntityFromAsepriteData();
            entity.Transformation.Position = Transformation.Position + new Vector3(0, 0, 0.5f);
            entity.GetComponent<Animator>().Play("Idle");
            entity.Destroy(0.5f);

            GameManager.EnemyManager.Units.Remove(this);

            GameManager.Player.Gate.Attack(Math.Min(EnemyWeight, 10));
        }

        protected override void TryAttack()
        {
            if (IsFrozen.Any())
                return;
            
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
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
            
            yield return new WaitForSeconds(AttackNormalTime);
            
            GetComponent<Animator>().Play("Idle");
            
            if (!IsTargetValid())
            {
                canAttack = true;
                yield break;
            }

            DoAttack();
            
            yield return new WaitForSeconds(AttackDelay - AttackNormalTime);

            canAttack = true;
        }

        protected virtual void DoAttack()
        {
            Vector2 force = (Target.Transformation.Position - Transformation.Position).Normalize() * AttackKnockBack;
            Target.TakeDamage(AttackDamage, force);
            
            GetComponent<AudioSource>().Play(AttackAudio);
        }

        protected override void Die()
        {
            base.Die();

            GameManager.EnemyManager.Units.Remove(this);
            GameManager.Player.GainGold(EnemyWeight * 2);
        }
    }
}