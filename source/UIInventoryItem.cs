using System;
using Steel;
using SteelCustom.Heroes;
using SteelCustom.Spells;

namespace SteelCustom
{
    public class UIInventoryItem : ScriptComponent
    {
        public HeroStorage.HeroInfo? HeroInfo;
        public SpellStorage.SpellInfo? SpellInfo;
        private Entity tooltip;

        public void Create(HeroStorage.HeroInfo heroInfo, float size)
        {
            HeroInfo = heroInfo;
            CreateInner(heroInfo.Name, heroInfo.Icon, size);
        }
        
        public void Create(SpellStorage.SpellInfo spellInfo, float size)
        {
            SpellInfo = spellInfo;
            CreateInner(spellInfo.Name, spellInfo.Icon, size);
        }

        private void CreateInner(string name, Sprite icon, float size)
        {
            GetComponent<UIButton>().TargetImage.Sprite = ResourcesManager.LoadImage("ui_frame.png");
            
            GetComponent<UIButton>().OnClick.AddCallback(
                () =>
                {
                    GameManager.Player.Inventory.OnClicked(this);
                }
            );
            
            RectTransformation rt = GetComponent<RectTransformation>();
            rt.AnchorMin = new Vector2(0.5f, 1.0f);
            rt.AnchorMax = new Vector2(0.5f, 1.0f);
            rt.Size = new Vector2(size, size);
            rt.Pivot = new Vector2(0.5f, 1.0f);

            UIText text = UI.CreateUIText(name, "Spell name", Entity);
            text.Color = Color.Black;
            RectTransformation textRT = text.RectTransform;
            textRT.AnchorMin = new Vector2(0.0f, 1.0f);
            textRT.AnchorMax = new Vector2(1.0f, 1.0f);
            textRT.Size = new Vector2(0, 32);
            textRT.Pivot = new Vector2(0.0f, 1.0f);
            textRT.OffsetMin = new Vector2(16, 0);
            textRT.AnchoredPosition = new Vector2(0, -4);

            UIImage uiIcon = UI.CreateUIImage(icon, "Spell icon", Entity);
            RectTransformation iconRT = uiIcon.RectTransform;
            iconRT.AnchorMin = Vector2.Zero;
            iconRT.AnchorMax = Vector2.One;
            iconRT.OffsetMin = new Vector2(16, 16);
            iconRT.OffsetMax = new Vector2(32, 32);
            uiIcon.ConsumeEvents = false;
        }

        public void SetY(float y)
        {
            RectTransformation rt = GetComponent<RectTransformation>();
            rt.AnchoredPosition = new Vector2(0, y);
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