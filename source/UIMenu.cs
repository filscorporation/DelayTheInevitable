using Steel;

namespace SteelCustom
{
    public class UIMenu : ScriptComponent
    {
        private bool menuOpened;

        private Entity menu;
        private UIButton playButton;
        private UIButton continueButton;
        private UIButton soundButton;
        private bool lostMenuOpened = false;

        public override void OnCreate()
        {
            GetComponent<RectTransformation>().AnchorMin = Vector2.Zero;
            GetComponent<RectTransformation>().AnchorMax = Vector2.One;
            
            menu = UI.CreateUIImage(ResourcesManager.LoadImage("ui_dim.png"), "Menu", Entity).Entity;
            RectTransformation menuRT = menu.GetComponent<RectTransformation>();
            menuRT.AnchorMin = Vector2.Zero;
            menuRT.AnchorMax = Vector2.One;

            playButton = CreateMenuButton("Play", 120);
            playButton.OnClick.AddCallback(Play);
            continueButton = CreateMenuButton("Continue", 120);
            continueButton.OnClick.AddCallback(CloseMenu);
            continueButton.Entity.IsActiveSelf = false;
            CreateMenuButton("Exit", 0).OnClick.AddCallback(Exit);
            soundButton = CreateSoundButton();
            CreateAbout();
            
            {
                UIText uiText = UI.CreateUIText($"How to play:\n" +
                                                $"Buy heroes and spells from the shop (bottom right) and place or cast them " +
                                                $"to delay enemy invasion as much as possible", "Text", menu);
                uiText.Color = Color.Black;
                uiText.TextSize = 32;
                uiText.TextAlignment = AlignmentType.CenterLeft;
                uiText.TextOverflowMode = OverflowMode.WrapByWords;

                uiText.RectTransform.AnchorMin = new Vector2(0.0f, 0.0f);
                uiText.RectTransform.AnchorMax = new Vector2(0.0f, 0.0f);
                uiText.RectTransform.Size = new Vector2(500, 200);
                uiText.RectTransform.Pivot = new Vector2(0.0f, 0.0f);
                uiText.RectTransform.AnchoredPosition = new Vector2(10, 10);
            }
        }

        public override void OnUpdate()
        {
            if (Input.IsKeyJustPressed(KeyCode.Escape))
            {
                if (menuOpened)
                    CloseMenu();
                else if (GameManager.Player != null && !GameManager.Player.Entity.IsDestroyed())
                {
                    if (!GameManager.Player.StopPlacing())
                    {
                        OpenMenu();
                    }
                }
            }
        }

        public void OpenOnLoseScreen()
        {
            //Time.TimeScale = 0.0f;

            lostMenuOpened = true;
            
            menu?.Destroy();
            
            menu = UI.CreateUIImage(ResourcesManager.LoadImage("ui_dim.png"), "Menu", Entity).Entity;
            RectTransformation menuRT = menu.GetComponent<RectTransformation>();
            menuRT.AnchorMin = Vector2.Zero;
            menuRT.AnchorMax = Vector2.One;

            {
                UIText uiText = UI.CreateUIText("You lost!", "Text", menu);
                uiText.Color = Color.Black;
                uiText.TextSize = 64;
                uiText.TextAlignment = AlignmentType.CenterMiddle;

                uiText.RectTransform.AnchorMin = new Vector2(0.5f, 0.5f);
                uiText.RectTransform.AnchorMax = new Vector2(0.5f, 0.5f);
                uiText.RectTransform.Size = new Vector2(400, 100);
                uiText.RectTransform.AnchoredPosition = new Vector2(0, 300);
            }
            {
                UIText uiText = UI.CreateUIText($"You delayed the invasion for {(int)GameManager.Player.TimePassed} second(s) " +
                                                $"and reached wave {GameManager.EnemyManager.CurrentWave}!", "Text", menu);
                uiText.Color = Color.Black;
                uiText.TextSize = 32;
                uiText.TextAlignment = AlignmentType.CenterLeft;
                uiText.TextOverflowMode = OverflowMode.WrapByWords;

                uiText.RectTransform.AnchorMin = new Vector2(0.5f, 0.5f);
                uiText.RectTransform.AnchorMax = new Vector2(0.5f, 0.5f);
                uiText.RectTransform.Size = new Vector2(400, 200);
                uiText.RectTransform.AnchoredPosition = new Vector2(0, 160);
            }
            {
                UIText uiText = UI.CreateUIText($"To restart you need to restart the application (yes :c)", "Text", menu);
                uiText.Color = Color.Black;
                uiText.TextSize = 32;
                uiText.TextAlignment = AlignmentType.CenterLeft;
                uiText.TextOverflowMode = OverflowMode.WrapByWords;

                uiText.RectTransform.AnchorMin = new Vector2(0.5f, 0.5f);
                uiText.RectTransform.AnchorMax = new Vector2(0.5f, 0.5f);
                uiText.RectTransform.Size = new Vector2(400, 200);
                uiText.RectTransform.AnchoredPosition = new Vector2(100, -100);
            }

            CreateMenuButton("Exit", 0).OnClick.AddCallback(Exit);
            CreateAbout();
        }

        private void OpenMenu()
        {
            if (lostMenuOpened)
                return;
            
            Time.TimeScale = 0.0f;
            
            menuOpened = true;
            menu.IsActiveSelf = true;
        }

        private void CloseMenu()
        {
            if (lostMenuOpened)
                return;
            
            Time.TimeScale = 1.0f;
            
            menuOpened = false;
            menu.IsActiveSelf = false;
        }

        private void Play()
        {
            continueButton.Entity.IsActiveSelf = true;
            playButton.Entity.IsActiveSelf = true;

            CloseMenu();
            
            GameManager.StartGame();
        }

        private void Exit()
        {
            Application.Quit();
        }

        private void ChangeSound()
        {
            if (GameManager.SoundOn)
            {
                GameManager.SoundOn = false;
                Camera.Main.Entity.AddComponent<AudioListener>().Volume = 0.0f;
                soundButton.TargetImage.Sprite = ResourcesManager.LoadImage("ui_sound_off.png");
            }
            else
            {
                GameManager.SoundOn = true;
                Camera.Main.Entity.AddComponent<AudioListener>().Volume = GameManager.DEFAULT_VOLUME;
                soundButton.TargetImage.Sprite = ResourcesManager.LoadImage("ui_sound_on.png");
            }
        }

        private UIButton CreateMenuButton(string text, float y)
        {
            Sprite sprite = ResourcesManager.LoadImage("ui_frame.png");
            sprite.SetAs9Sliced(3);
            sprite.PixelsPerUnit = 32;
            UIButton button = UI.CreateUIButton(sprite, "Menu button", menu);
            button.RectTransform.AnchorMin = new Vector2(0.5f, 0.5f);
            button.RectTransform.AnchorMax = new Vector2(0.5f, 0.5f);
            button.RectTransform.Size = new Vector2(200, 100);
            button.RectTransform.AnchoredPosition = new Vector2(0, y);

            UIText uiText = UI.CreateUIText(text, "Label", button.Entity);
            uiText.Color = Color.Black;
            uiText.TextSize = 32;
            uiText.TextAlignment = AlignmentType.CenterMiddle;
            uiText.RectTransform.AnchorMin = Vector2.Zero;
            uiText.RectTransform.AnchorMax = Vector2.One;

            return button;
        }

        private UIButton CreateSoundButton()
        {
            Sprite sprite = ResourcesManager.LoadImage("ui_sound_on.png");
            sprite.PixelsPerUnit = 64;
            UIButton button = UI.CreateUIButton(sprite, "Sound button", menu);
            button.RectTransform.AnchorMin = new Vector2(0.5f, 0.5f);
            button.RectTransform.AnchorMax = new Vector2(0.5f, 0.5f);
            button.RectTransform.Size = new Vector2(64, 64);
            button.RectTransform.AnchoredPosition = new Vector2(170, -70);
            
            button.OnClick.AddCallback(ChangeSound);

            return button;
        }

        private void CreateAbout()
        {
            UIText text = UI.CreateUIText("Created in 48 hours for LD50 using Steel Engine", "About", menu);
            text.Color = Color.Black;
            text.TextSize = 32;
            text.RectTransform.AnchorMin = new Vector2(0.5f, 0.0f);
            text.RectTransform.AnchorMax = new Vector2(1.0f, 0.0f);
            text.RectTransform.Pivot = new Vector2(0.0f, 0.0f);
            text.RectTransform.Size = new Vector2(0, 40);
        }
    }
}