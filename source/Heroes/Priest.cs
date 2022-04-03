using System.Collections;
using Steel;
using SteelCustom.Spells;

namespace SteelCustom.Heroes
{
    public class Priest : Hero
    {
        public float HealAmount;
        
        private bool canUseLevel3Skill = true;
        
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

        protected override void FindTarget()
        {
            Hero target = null;
            float minHealth = float.MaxValue;
            foreach (Hero hero in GameManager.Player.Heroes)
            {
                float dist = Vector3.Distance(hero.Transformation.Position, Transformation.Position);
                if (dist < ViewRange)
                {
                    if (hero.Health < hero.MaxHealth && hero.Health < minHealth)
                    {
                        minHealth = hero.Health;
                        target = hero;
                    }
                }
            }

            Target = target;
        }

        protected override bool IsTargetValid()
        {
            return base.IsTargetValid() && Target.Health < Target.MaxHealth;
        }

        protected override void DoAttack()
        {
            Target.Heal(HealAmount);
            
            GainExpFromAttack();
        }

        protected override void LevelUp()
        {
            HealAmount *= 1.5f;
            
            base.LevelUp();
        }

        protected override void TryUseSkill()
        {
            if (canUseLevel3Skill && canAttack)
            {
                StartCoroutine(UseLevel3Skill());
            }
        }

        private IEnumerator UseLevel3Skill()
        {
            canAttack = false;
            canUseLevel3Skill = false;

            Spell spell = SpellStorage.Spells[(int)SpellStorage.SpellType.HealWave].Spawner.Invoke();
            spell.Cast(Vector3.Zero);

            yield return new WaitForSeconds(1.0f);
            canAttack = true;

            yield return new WaitForSeconds(14f);
            canUseLevel3Skill = true;
        }
    }
}