using Steel;

namespace SteelCustom.Enemies
{
    public class Hunter : Enemy
    {
        protected override AudioTrack AttackAudio => ResourcesManager.LoadAudioTrack("small_explosion.wav");

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
            base.DoAttack();
            
            Die();
        }
    }
}