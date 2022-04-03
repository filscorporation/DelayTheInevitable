using System.Collections.Generic;
using Steel;
using SteelCustom.Heroes;
using SteelCustom.Spells;

namespace SteelCustom
{
    public class UIShop : ScriptComponent
    {
        private List<UIShopItem> items = new List<UIShopItem>();
        private UIButton rerollButton;
        
        private const float ITEM_SIZE = 110f;
        private const float ITEM_OFFSET = 4f;
        private const float BORDER_OFFSET = 8f;

        public override void OnCreate()
        {
            RectTransformation rt = GetComponent<RectTransformation>();
            rt.AnchorMin = new Vector2(1.0f, 0.0f);
            rt.AnchorMax = new Vector2(1.0f, 0.0f);
            rt.Pivot = new Vector2(1.0f, 0.0f);
            rt.AnchoredPosition = new Vector2(-10, 60);
            rt.Size = new Vector2(ITEM_SIZE * 3 + ITEM_OFFSET * 2 + BORDER_OFFSET * 2, ITEM_SIZE + BORDER_OFFSET * 2);
            
            Sprite sprite = ResourcesManager.LoadImage("ui_frame.png");
            sprite.SetAs9Sliced(3);
            sprite.PixelsPerUnit = 32;
            GetComponent<UIImage>().Sprite = sprite;
            
            UIText label = UI.CreateUIText("Shop", "Label", Entity);
            label.TextSize = 32;
            label.Color = Color.Black;
            label.RectTransform.AnchorMin = new Vector2(0.0f, 1.0f);
            label.RectTransform.AnchorMax = new Vector2(0.0f, 1.0f);
            label.RectTransform.AnchoredPosition = new Vector2(0, 0);
            label.RectTransform.Pivot = new Vector2(0.0f, 0.0f);
            label.RectTransform.Size = new Vector2(100, 40);

            rerollButton = UI.CreateUIButton(ResourcesManager.LoadImage("ui_reroll.aseprite"), "reroll", Entity);
            rerollButton.RectTransform.AnchorMin = new Vector2(1.0f, 0.0f);
            rerollButton.RectTransform.AnchorMax = new Vector2(1.0f, 0.0f);
            rerollButton.RectTransform.AnchoredPosition = new Vector2(0, -10);
            rerollButton.RectTransform.Pivot = new Vector2(1.0f, 1.0f);
            rerollButton.RectTransform.Size = new Vector2(32, 32);

            rerollButton.Entity.AddComponent<UIInfo>().TooltipInfo = "Reroll shop for 10 gold";
            
            rerollButton.OnClick.AddCallback(
                () =>
                {
                    if (GameManager.Player.Gold >= GameManager.Player.ReRollCost)
                    {
                        GameManager.Player.SpendGold(GameManager.Player.ReRollCost);
                        ReRoll();
                    }
                }
            );

            Entity.AddComponent<UIInfo>().TooltipInfo =
                "You can buy anything from the shop, or reroll for more choices";
        }
        
        public void OnClicked(UIShopItem item)
        {
            if (item.HeroInfo.HasValue && GameManager.Player.Gold >= item.HeroInfo.Value.Cost)
            {
                GameManager.Player.SpendGold(item.HeroInfo.Value.Cost);
                GameManager.Player.Inventory.AddItem(item.HeroInfo.Value);
                
                ReRoll();
            }
            if (item.SpellInfo.HasValue && GameManager.Player.Gold >= item.SpellInfo.Value.Cost)
            {
                GameManager.Player.SpendGold(item.SpellInfo.Value.Cost);
                GameManager.Player.Inventory.AddItem(item.SpellInfo.Value);
                
                ReRoll();
            }
        }

        public void ReRoll()
        {
            foreach (UIShopItem item in items)
            {
                item.Entity.Destroy();
            }
            items.Clear();
            
            for (int i = 0; i < 3; i++)
            {
                items.Add(CreateRandomItem());
            }
        }

        private UIShopItem CreateRandomItem()
        {
            const float SPELL_CHANCE = 0.15f;
            //const float LEVEL_UP_CHANCE = 0.1f;

            UIShopItem item;
            
            if (Random.NextFloat(0.0f, 1.0f) <= SPELL_CHANCE)
            {
                SpellStorage.SpellInfo spellInfo = SpellStorage.Spells[Random.NextInt(0, SpellStorage.Spells.Count - 1)];
                
                item = UI.CreateUIButton().Entity.AddComponent<UIShopItem>();
                item.Entity.Parent = Entity;
                item.Create(spellInfo, BORDER_OFFSET + items.Count * (ITEM_SIZE + ITEM_OFFSET), ITEM_SIZE);
            }
            else
            {
                HeroStorage.HeroInfo heroInfo = HeroStorage.Heroes[Random.NextInt(0, HeroStorage.Heroes.Count - 1)];
                
                item = UI.CreateUIButton().Entity.AddComponent<UIShopItem>();
                item.Entity.Parent = Entity;
                item.Create(heroInfo, BORDER_OFFSET + items.Count * (ITEM_SIZE + ITEM_OFFSET), ITEM_SIZE);
            }

            return item;
        }
    }
}