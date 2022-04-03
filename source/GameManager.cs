using Steel;

namespace SteelCustom
{
    public class GameManager
    {
        public static UIMenu Menu;
        public static Map Map;
        public static Player Player;
        public static EnemyManager EnemyManager;

        public static bool SoundOn = true;
        public static Entity InGameUIRoot;

        private static bool gameStarted = false;

        public const float DEFAULT_VOLUME = 0.15f;
        
        public static void EntryPoint()
        {
            Time.FixedDeltaTime = 1.0f / 240.0f;
            Screen.Color = new Color(194, 181, 169);
            Screen.Width = 1366;
            Screen.Height = 768;
            
            Camera.Main.Height = 12f;
            Camera.Main.Entity.AddComponent<AudioListener>().Volume = DEFAULT_VOLUME;

            InGameUIRoot = UI.CreateUIElement("UI root");
            InGameUIRoot.GetComponent<RectTransformation>().AnchorMin = Vector2.Zero;
            InGameUIRoot.GetComponent<RectTransformation>().AnchorMax = Vector2.One;

            Menu = UI.CreateUIElement("Menu").AddComponent<UIMenu>();

            Entity backgroundMusic = new Entity();
            AudioSource source = backgroundMusic.AddComponent<AudioSource>();
            source.Loop = true;
            source.Play(ResourcesManager.LoadAudioTrack("background_music.wav"));
            source.Volume = 0.1f;
        }

        public static void StartGame()
        {
            if (gameStarted)
                return;
            gameStarted = true;
            
            Camera.Main.Entity.AddComponent<CameraController>();

            #region Bug fix

            {
                // TODO: remove
                Entity entity = new Entity("fix_bug_entity");
                entity.Transformation.Position = new Vector3(-1000f, -1000f);
                entity.AddComponent<SpriteRenderer>().Sprite = ResourcesManager.LoadImage("frame.png");
            }

            #endregion

            Map = new Entity().AddComponent<Map>();
            Map.Generate();

            Player = new Entity().AddComponent<Player>();
            Player.Entity.Name = "Player";
            EnemyManager = new Entity().AddComponent<EnemyManager>();
            EnemyManager.Entity.Name = "EnemyManager";
        }
    }
}