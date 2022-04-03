using System.Collections.Generic;
using System.Linq;
using Steel;

namespace SteelCustom.Enemies
{
    public class Orge : Enemy
    {
        protected override AudioTrack AttackAudio => ResourcesManager.LoadAudioTrack("big_hit.wav");

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void DoAttack()
        {
            List<Hero> heroes = GameManager.Player.Heroes.ToList();
            foreach (Hero playerHero in heroes)
            {
                Vector2 force = (playerHero.Transformation.Position - Transformation.Position).Normalize() * AttackKnockBack;
                if (Vector3.Distance(playerHero.Transformation.Position, Transformation.Position) <= AttackRange)
                    playerHero.TakeDamage(AttackDamage, force);
            }
            
            GetComponent<AudioSource>().Play(AttackAudio);
        }
    }
}