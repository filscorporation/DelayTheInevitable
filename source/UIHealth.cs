using Steel;

namespace SteelCustom
{
    public class UIHealth : ScriptComponent
    {
        private UIImage healthImage;
        private UIText healthText;
        
        public static UIHealth Create()
        {
            Sprite sprite = ResourcesManager.LoadImage("ui_health_frame.png");
            sprite.SetAs9Sliced(1);
            sprite.PixelsPerUnit = 32;
            UIImage image = UI.CreateUIImage(sprite, "Gate health", GameManager.InGameUIRoot);
            UIHealth result = image.Entity.AddComponent<UIHealth>();
            result.healthImage = UI.CreateUIImage(ResourcesManager.LoadImage("ui_health.png"), "Health", image.Entity);
            result.healthImage.RectTransform.AnchorMin = Vector2.Zero;
            result.healthImage.RectTransform.AnchorMax = Vector2.One;
            result.healthImage.RectTransform.OffsetMin = new Vector2(4, 4);
            result.healthImage.RectTransform.OffsetMax = new Vector2(4, 4);

            result.healthImage.ConsumeEvents = false;

            result.healthText = UI.CreateUIText($"Gate health: {0}/{0}", "Label", image.Entity);
            result.healthText.Color = Color.Black;
            result.healthText.TextSize = 32;
            result.healthText.TextAlignment = AlignmentType.CenterMiddle;
            result.healthText.RectTransform.AnchorMin = new Vector2(0.5f, 1.0f);
            result.healthText.RectTransform.AnchorMax = new Vector2(0.5f, 1.0f);
            result.healthText.RectTransform.Size = new Vector2(300, 36);
            result.healthText.RectTransform.Pivot = new Vector2(0.5f, 0.0f);
            result.healthText.RectTransform.AnchoredPosition = new Vector2(0, -4);
            
            image.Entity.AddComponent<UIInfo>().TooltipInfo =
                "Toughness of the gates to the city. When it reaches zero - you lose(";

            return result;
        }
        
        public override void OnCreate()
        {
            
        }

        public void Set(float health, float maxHealth)
        {
            healthText.Text = $"Gate health: {health}/{maxHealth}";
            
            healthImage.RectTransform.AnchorMin = Vector2.Zero;
            healthImage.RectTransform.AnchorMax = new Vector2(health / maxHealth, 1.0f);
        }
    }
}