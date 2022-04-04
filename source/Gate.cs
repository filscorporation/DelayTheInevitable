using System.Collections;
using Steel;

namespace SteelCustom
{
    public class Gate : ScriptComponent
    {
        public float MaxToughness = 20;
        public float Toughness = 20;

        private bool dead = false;

        private UIHealth uiHealth;
        
        public override void OnCreate()
        {
            GetComponent<Animator>().Play("Idle");
            
            uiHealth = UIHealth.Create();
            RectTransformation healthRT = uiHealth.GetComponent<RectTransformation>();
            healthRT.AnchorMin = new Vector2(1.0f, 0.0f);
            healthRT.AnchorMax = new Vector2(1.0f, 0.0f);
            healthRT.AnchoredPosition = new Vector2(-320, 4);
            healthRT.Size = new Vector2(200, 20);
            healthRT.Pivot = new Vector2(0.0f, 0.0f);

            uiHealth.Set(Toughness, MaxToughness);
        }

        public void Attack(int weight)
        {
            Camera.Main.Entity.GetComponent<CameraController>().Shake(weight * 0.6f);
            
            if (dead)
                return;
            
            Toughness -= weight;

            if (Toughness <= 0.0f)
            {
                Toughness = 0.0f;
                Destroy();
            }
            
            uiHealth.Set(Toughness, MaxToughness);

            StartCoroutine(DamagedRoutine());
        }

        private IEnumerator DamagedRoutine()
        {
            Entity.AddComponent<AudioSource>().Play(ResourcesManager.LoadAudioTrack("gate_hurt.wav"));
            
            GetComponent<Animator>().Play("Damaged");

            yield return new WaitForSeconds(0.4f);

            if (dead)
            {
                GetComponent<Animator>().Play("Dead");
                
                Entity.AddComponent<AudioSource>().Play(ResourcesManager.LoadAudioTrack("gate_dead.wav"));
            }
            else
                GetComponent<Animator>().Play("Idle");
        }

        private void Destroy()
        {
            dead = true;
            GameManager.Player.GameLost();
        }
    }
}