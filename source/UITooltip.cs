using System.Collections;
using Steel;
using SteelCustom.Heroes;
using SteelCustom.Spells;

namespace SteelCustom
{
    public class UITooltip : ScriptComponent
    {
        private static Entity current = null;

        public static Entity ShowTooltip(string info, float height = 40f)
        {
            UIImage image = UI.CreateUIImage(ResourcesManager.LoadImage("ui_frame.png"), "Tooltip", null);
            image.RectTransform.AnchorMin = Vector2.One;
            image.RectTransform.AnchorMax = Vector2.One;
            image.RectTransform.Pivot = Vector2.One;
            image.RectTransform.AnchoredPosition = new Vector2(-8, -8);
            image.RectTransform.Size = new Vector2(200, height);

            UIText text = UI.CreateUIText(info, "Text", image.Entity);
            text.Color = Color.Black;
            text.TextAlignment = AlignmentType.TopLeft;
            text.TextOverflowMode = OverflowMode.WrapByWords;
            text.RectTransform.AnchorMin = Vector2.Zero;
            text.RectTransform.AnchorMax = Vector2.One;
            text.RectTransform.OffsetMin = new Vector2(12, 12);
            text.RectTransform.OffsetMax = new Vector2(12, 12);
            
            FinishShow(image.Entity);

            return image.Entity;
        }

        public static Entity ShowTooltip(Hero hero)
        {
            string text = $"{hero.Name}\n" +
                          $"Health {hero.Health}/{hero.MaxHealth}\n" +
                          $"Level {hero.Level}\n" +
                          $"Experience {hero.Experience}{(hero.ExperienceForNextLevel < 0 ? "" : "/" + hero.ExperienceForNextLevel)}\n" +
                          $"Attack {hero.AttackDamage}\n" +
                          $"Range {hero.AttackRange}\n" +
                          $"Speed {hero.Speed}";
            return ShowTooltip(text, 240);
        }

        public static Entity ShowTooltip(HeroStorage.HeroInfo heroInfo)
        {
            string text = $"{heroInfo.Name}\n" +
                          $"Cost {heroInfo.Cost}\n" +
                          $"Level {heroInfo.Level}\n" +
                          $"\n" +
                          $"{heroInfo.Description}";
            return ShowTooltip(text, 160);
        }

        public static Entity ShowTooltip(SpellStorage.SpellInfo spellInfo)
        {
            string text = $"{spellInfo.Name}\n" +
                          $"Cost {spellInfo.Cost}\n" +
                          $"\n" +
                          $"{spellInfo.Description}";
            return ShowTooltip(text, 120);
        }

        private static void FinishShow(Entity tooltip)
        {
            tooltip.StartCoroutine(HideTooltipRoutine(tooltip));

            if (current != null)
                HideTooltip(current);
            current = tooltip;
        }

        public static void HideTooltip(Entity tooltip)
        {
            if (tooltip == null || tooltip.IsDestroyed() || current != tooltip)
                return;
            
            current.Destroy();
            current = null;
        }

        private static IEnumerator HideTooltipRoutine(Entity tooltip)
        {
            yield return new WaitForSeconds(7.0f);
            
            HideTooltip(tooltip);
        }
    }
}