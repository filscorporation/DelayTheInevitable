using Steel;
using SteelCustom.Heroes;
using SteelCustom.Spells;

namespace SteelCustom
{
    public class UIShopItem : ScriptComponent
    {
        public HeroStorage.HeroInfo? HeroInfo;
        public SpellStorage.SpellInfo? SpellInfo;
        private Entity tooltip;

        public void Create(HeroStorage.HeroInfo heroInfo, float x, float size)
        {
            HeroInfo = heroInfo;
            CreateInner(heroInfo.Name, heroInfo.Cost, heroInfo.Icon, x, size);
        }
        
        public void Create(SpellStorage.SpellInfo spellInfo, float x, float size)
        {
            SpellInfo = spellInfo;
            CreateInner(spellInfo.Name, spellInfo.Cost, spellInfo.Icon, x, size);
        }

        private void CreateInner(string name, int cost, Sprite icon, float x, float size)
        {
            GetComponent<UIButton>().TargetImage.Sprite = ResourcesManager.LoadImage("ui_frame.png");
            
            GetComponent<UIButton>().OnClick.AddCallback(
                () =>
                {
                    GameManager.Player.Shop.OnClicked(this);
                }
            );
            
            RectTransformation rt = GetComponent<RectTransformation>();
            rt.AnchorMin = new Vector2(0.0f, 0.5f);
            rt.AnchorMax = new Vector2(0.0f, 0.5f);
            rt.Size = new Vector2(size, size);
            rt.Pivot = new Vector2(0.0f, 0.5f);
            rt.AnchoredPosition = new Vector2(x, 0);

            UIText nameText = UI.CreateUIText(name, "Name", Entity);
            nameText.Color = Color.Black;
            RectTransformation textRT = nameText.RectTransform;
            textRT.AnchorMin = new Vector2(0.0f, 1.0f);
            textRT.AnchorMax = new Vector2(1.0f, 1.0f);
            textRT.Size = new Vector2(0, 32);
            textRT.Pivot = new Vector2(0.0f, 1.0f);
            textRT.OffsetMin = new Vector2(16, 0);
            textRT.AnchoredPosition = new Vector2(0, -4);

            UIImage uiIcon = UI.CreateUIImage(icon, "Icon", Entity);
            RectTransformation iconRT = uiIcon.RectTransform;
            iconRT.AnchorMin = Vector2.Zero;
            iconRT.AnchorMax = Vector2.One;
            iconRT.OffsetMin = new Vector2(16, 32);
            iconRT.OffsetMax = new Vector2(48, 32);
            uiIcon.ConsumeEvents = false;

            UIText costText = UI.CreateUIText("Cost " + cost, "Cost", Entity);
            costText.Color = Color.Black;
            textRT = costText.RectTransform;
            textRT.AnchorMin = new Vector2(0.0f, 0.0f);
            textRT.AnchorMax = new Vector2(1.0f, 0.0f);
            textRT.Size = new Vector2(0, 32);
            textRT.Pivot = new Vector2(0.0f, 0.0f);
            textRT.OffsetMin = new Vector2(16, 0);
            textRT.AnchoredPosition = new Vector2(0, 4);
        }

        public override void OnMouseEnterUI()
        {
            if (HeroInfo.HasValue)
                tooltip = UITooltip.ShowTooltip(HeroInfo.Value);
            if (SpellInfo.HasValue)
                tooltip = UITooltip.ShowTooltip(SpellInfo.Value);
        }

        public override void OnMouseExitUI()
        {
            UITooltip.HideTooltip(tooltip);
        }
    }
}