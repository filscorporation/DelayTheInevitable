using System.Linq;
using Steel;

namespace SteelCustom
{
    public abstract class Unit : ScriptComponent
    {
        public float Speed = 0.3f;
        public float ViewRange = 5.0f;
        public float AttackDamage = 1.5f;
        public float AttackKnockBack = 50f;
        public float Mass = 1.0f;
        public float Radius = 0.25f;
        public float AttackRange = 0.75f;
        public float AttackDelay = 2.0f;
        public float AttackNormalTime = 0.35f;
        public float MaxHealth = 20f;
        public float Health = 20f;

        protected bool Alive = true;
        protected Unit Target;

        protected RigidBody Body;
        private Vector2 targetVelocity;
        private float retargetTimer = 0.0f;

        protected virtual AudioTrack AttackAudio => null;

        protected void ActiveUpdate()
        {
            UpdateVelocity();
            
            if (!Alive)
                return;

            retargetTimer += Time.DeltaTime;

            if (!IsTargetValid())
            {
                FindTarget();
                retargetTimer = 0.0f;
            }

            if (!IsTargetValid())
            {
                NoTarget();
                return;
            }

            Rotate(Target.Transformation.Position);

            if (CanAttack())
            {
                SetVelocity(Vector2.Zero);
                TryAttack();
                return;
            }
            
            MoveToTarget();
        }

        protected virtual bool IsTargetValid()
        {
            return Target != null && !Target.Entity.IsDestroyed();
        }

        protected bool CanAttack()
        {
            return Vector3.Distance(Transformation.Position, Target.Transformation.Position) <= AttackRange;
        }

        protected abstract void FindTarget();

        protected abstract void NoTarget();

        protected abstract void TryAttack();

        protected virtual void MoveToTarget()
        {
            SetVelocity((Target.Transformation.Position - Transformation.Position).Normalize() * Speed);
            if (retargetTimer >= 1.0f)
            {
                FindTarget();
                retargetTimer = 0.0f;
            }
        }

        private void UpdateVelocity()
        {
            Body.Velocity = Math.Lerp(Body.Velocity, targetVelocity, Time.DeltaTime * 10);
        }

        protected void SetVelocity(Vector2 velocity)
        {
            targetVelocity = velocity;
        }
        
        protected void Rotate(Vector3 target)
        {
            Vector2 vector = target - Transformation.Position;
            float angle = -Math.Atan2(vector.X, vector.Y);
            float current = Transformation.Rotation.Z;

            Transformation.Rotation = new Vector3(0.0f, 0.0f, Math.LerpAngle(current, angle, Time.DeltaTime * 5f));
        }

        public void TakeDamage(float damage, Vector2 force)
        {
            Health -= damage;
            Body.ApplyForce(force);

            if (Health <= 0.0f)
            {
                Health = 0.0f;
                Die();
            }
        }

        public void Heal(float heal)
        {
            Health = Math.Min(Health + heal, MaxHealth);

            AsepriteData data = ResourcesManager.LoadAsepriteData("heal_effect.aseprite");
            data.Animations.First().EndWithEmptyFrame();
            Entity entity = data.CreateEntityFromAsepriteData();
            entity.Parent = Entity;
            entity.Transformation.Position = Transformation.Position.SetZ(2.0f);
            entity.GetComponent<Animator>().Play("Idle");
            
            entity.Destroy(3.0f);
        }

        protected virtual void Die()
        {
            GetComponent<Animator>().Play("Dead");
            SetVelocity(Vector2.Zero);
            Entity.Destroy(1.0f);
            Alive = false;
        }
    }
}