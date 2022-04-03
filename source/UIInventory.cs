using System.Collections.Generic;
using Steel;
using SteelCustom.Heroes;
using SteelCustom.Spells;

namespace SteelCustom
{
    public class UIInventory : ScriptComponent
    {
        private List<UIInventoryItem> items = new List<UIInventoryItem>();

        private UIInventoryItem currentSelectedItem;
        private Entity contentNode;

        private const float ITEM_SIZE = 110f;
        private const float ITEM_OFFSET = 4f;
        private const float BORDER_OFFSET = 8f;

        public override void OnCreate()
        {
            RectTransformation rt = GetComponent<RectTransformation>();
            rt.AnchorMin = new Vector2(0.0f, 0.0f);
            rt.AnchorMax = new Vector2(0.0f, 1.0f);
            rt.Pivot = new Vector2(0.0f, 0.5f);
            rt.AnchoredPosition = new Vector2(30, 0);
            rt.OffsetMin = new Vector2(0, 40);
            rt.OffsetMax = new Vector2(0, 40);
            rt.Size = new Vector2(ITEM_SIZE + BORDER_OFFSET * 2, 0);

            Entity contentParent = UI.CreateUIElement("Content", Entity);
            contentParent.GetComponent<RectTransformation>().AnchorMin = Vector2.Zero;
            contentParent.GetComponent<RectTransformation>().AnchorMax = Vector2.One;
            contentParent.GetComponent<RectTransformation>().OffsetMin = new Vector2(8, 8);
            contentParent.GetComponent<RectTransformation>().OffsetMax = new Vector2(8, 8);

            contentNode = UI.CreateUIElement("Content", contentParent);
            contentNode.GetComponent<RectTransformation>().AnchorMin = Vector2.Zero;
            contentNode.GetComponent<RectTransformation>().AnchorMax = Vector2.One;
            
            contentParent.AddComponent<UIClipping>();

            Sprite sprite = ResourcesManager.LoadImage("ui_frame.png");
            sprite.SetAs9Sliced(3);
            sprite.PixelsPerUnit = 32;
            GetComponent<UIImage>().Sprite = sprite;
            
            UIText label = UI.CreateUIText("Inventory", "Label", Entity);
            label.TextSize = 32;
            label.Color = Color.Black;
            RectTransformation labelRT = label.RectTransform;
            labelRT.AnchorMin = new Vector2(0.0f, 1.0f);
            labelRT.AnchorMax = new Vector2(0.0f, 1.0f);
            labelRT.AnchoredPosition = new Vector2(0, 0);
            labelRT.Pivot = new Vector2(0.0f, 0.0f);
            labelRT.Size = new Vector2(ITEM_SIZE + BORDER_OFFSET * 2, 40);
            
            Entity.AddComponent<UIInfo>().TooltipInfo =
                "When you buy units or spells from the shop - they will end up here. Click anything to place or cast. " +
                "You can also use number keys as shortcuts.";
        }

        public void OnClicked(UIInventoryItem item)
        {
            if (currentSelectedItem != null)
            {
                // ??
            }
            
            currentSelectedItem = item;
            
            if (item.HeroInfo.HasValue)
                GameManager.Player.TryPlaceHero(item.HeroInfo.Value);
            if (item.SpellInfo.HasValue)
                GameManager.Player.TryCastSpell(item.SpellInfo.Value);
        }

        public override void OnUpdate()
        {
            if (Input.IsKeyJustPressed(KeyCode.D1))
            {
                TryOnClicked(1);
            }
            if (Input.IsKeyJustPressed(KeyCode.D2))
            {
                TryOnClicked(2);
            }
            if (Input.IsKeyJustPressed(KeyCode.D3))
            {
                TryOnClicked(3);
            }
            if (Input.IsKeyJustPressed(KeyCode.D4))
            {
                TryOnClicked(4);
            }
            if (Input.IsKeyJustPressed(KeyCode.D5))
            {
                TryOnClicked(5);
            }
            if (Input.IsKeyJustPressed(KeyCode.D6))
            {
                TryOnClicked(6);
            }
            if (Input.IsKeyJustPressed(KeyCode.D7))
            {
                TryOnClicked(7);
            }
            if (Input.IsKeyJustPressed(KeyCode.D8))
            {
                TryOnClicked(8);
            }
            if (Input.IsKeyJustPressed(KeyCode.D9))
            {
                TryOnClicked(9);
            }
        }

        private void TryOnClicked(int index)
        {
            if (index < 1 || items.Count < index)
                return;
            
            OnClicked(items[index - 1]);
        }

        public void RemoveSelected()
        {
            bool update = false;
            
            for (int i = 0; i < items.Count; i++)
            {
                if (update)
                {
                    items[i].SetY(-(i - 1) * (ITEM_SIZE + ITEM_OFFSET));
                }
                
                if (items[i] == currentSelectedItem)
                {
                    currentSelectedItem.Entity.Destroy();
                    update = true;
                }
            }

            if (update)
            {
                items.Remove(currentSelectedItem);
                currentSelectedItem = null;
            }
        }
        
        public void AddItem(HeroStorage.HeroInfo heroInfo)
        {
            UIInventoryItem item = UI.CreateUIButton().Entity.AddComponent<UIInventoryItem>();
            item.Entity.Parent = contentNode;
            item.Create(heroInfo, ITEM_SIZE);
            item.SetY(-items.Count * (ITEM_SIZE + ITEM_OFFSET));
            
            items.Add(item);
        }

        public void AddItem(SpellStorage.SpellInfo spellInfo)
        {
            UIInventoryItem item = UI.CreateUIButton().Entity.AddComponent<UIInventoryItem>();
            item.Entity.Parent = contentNode;
            item.Create(spellInfo, ITEM_SIZE);
            item.SetY(-items.Count * (ITEM_SIZE + ITEM_OFFSET));
            
            items.Add(item);
        }
    }
}