using System.Collections.Generic;
using Steel;
using SteelCustom.Heroes;
using SteelCustom.Spells;

namespace SteelCustom
{
    public class Player : ScriptComponent
    {
        public int Gold = 50;
        public int ReRollCost = 10;
        
        public float TimePassed = 0.0f;
        
        public UIInventory Inventory;
        public UIShop Shop;
        public Gate Gate;
        public List<Hero> Heroes = new List<Hero>();

        private bool isAlive = true;

        private HeroStorage.HeroInfo? currentPlacedHero;
        private SpellStorage.SpellInfo? currentCastedSpell;
        private UIText goldText;
        private UIText timeText;
        private Entity placedIcons = null;

        public override void OnCreate()
        {
            //Time.TimeScale = 5.0f;
            
            Inventory = UI.CreateUIImage(null, "Inventory", GameManager.InGameUIRoot).Entity.AddComponent<UIInventory>();
            Inventory.AddItem(HeroStorage.Heroes[0]);
            //Inventory.AddItem(HeroStorage.Heroes[0]);
            Inventory.AddItem(HeroStorage.Heroes[1]);
            //Inventory.AddItem(HeroStorage.Heroes[2]);
            Inventory.AddItem(SpellStorage.Spells[(int)SpellStorage.SpellType.Meteor]);
            //Inventory.AddItem(SpellStorage.Spells[(int)SpellStorage.SpellType.Blizzard]);
            
            Shop = UI.CreateUIImage(null, "Shop", GameManager.InGameUIRoot).Entity.AddComponent<UIShop>();
            Shop.ReRoll();

            Entity gateEntity = ResourcesManager.LoadAsepriteData("gate.aseprite", true).CreateEntityFromAsepriteData();
            Gate = gateEntity.AddComponent<Gate>();
            Gate.Entity.Name = "Gate";
            Gate.Transformation.Position = new Vector3(0.0f, -5.3f, 2.0f);

            goldText = UI.CreateUIText($"Gold: {Gold}", "Gold", Shop.Entity);
            goldText.TextAlignment = AlignmentType.CenterRight;
            goldText.Color = Color.Black;
            goldText.TextSize = 32;
            goldText.RectTransform.AnchorMin = new Vector2(1.0f, 1.0f);
            goldText.RectTransform.AnchorMax = new Vector2(1.0f, 1.0f);
            goldText.RectTransform.AnchoredPosition = new Vector2(0, 0);
            goldText.RectTransform.Pivot = new Vector2(1.0f, 0.0f);
            goldText.RectTransform.Size = new Vector2(200, 40);

            timeText = UI.CreateUIText($"Time passed: {(int)TimePassed}", "Time passed", GameManager.InGameUIRoot);
            timeText.TextAlignment = AlignmentType.CenterLeft;
            timeText.Color = Color.Black;
            timeText.TextSize = 32;
            timeText.RectTransform.AnchorMin = new Vector2(0.0f, 1.0f);
            timeText.RectTransform.AnchorMax = new Vector2(0.0f, 1.0f);
            timeText.RectTransform.AnchoredPosition = new Vector2(200, 0);
            timeText.RectTransform.Pivot = new Vector2(0.0f, 1.0f);
            timeText.RectTransform.Size = new Vector2(300, 40);
        }

        public override void OnUpdate()
        {
            if (!isAlive)
                return;
            
            TimePassed += Time.DeltaTime;
            timeText.Text = $"Time passed: {(int)TimePassed}";
            
            if ((currentPlacedHero.HasValue || currentCastedSpell.HasValue) && placedIcons != null && !placedIcons.IsDestroyed())
            {
                placedIcons.Transformation.Position = Camera.Main.ScreenToWorldPoint(Input.MousePosition);
                placedIcons.Transformation.Position = placedIcons.Transformation.Position.SetZ(2.0f);
            }
            
            if (!UI.IsPointerOverUI() && Input.IsMouseJustPressed(MouseCodes.ButtonLeft) && currentPlacedHero.HasValue)
            {
                SpawnHero(Camera.Main.ScreenToWorldPoint(Input.MousePosition));
            }
            if (!UI.IsPointerOverUI() && Input.IsMouseJustPressed(MouseCodes.ButtonLeft) && currentCastedSpell.HasValue)
            {
                CastSpell(Camera.Main.ScreenToWorldPoint(Input.MousePosition));
            }
            if (Input.IsKeyJustPressed(KeyCode.Tab))
            {
                GameLost();
            }
        }

        public void GameLost()
        {
            if (!isAlive)
                return;
            
            isAlive = false;
            
            StopPlacing();
            GameManager.Menu.OpenOnLoseScreen();
        }

        public bool StopPlacing()
        {
            if (currentPlacedHero.HasValue)
            {
                currentPlacedHero = null;
                if (placedIcons != null)
                    placedIcons.Destroy();
                // TODO: remove select from inventory?
                
                return true;
            }
            if (currentCastedSpell.HasValue)
            {
                currentCastedSpell = null;
                if (placedIcons != null)
                    placedIcons.Destroy();
                
                return true;
            }

            return false;
        }

        public void GainGold(int gold)
        {
            Gold += gold;
            goldText.Text = $"Gold: {Gold}";
        }

        public void SpendGold(int gold)
        {
            Gold -= gold;
            goldText.Text = $"Gold: {Gold}";
        }

        private void SpawnHero(Vector3 position)
        {
            if (!currentPlacedHero.HasValue)
                return;
            
            Hero hero = currentPlacedHero.Value.Spawner.Invoke();
            Inventory.RemoveSelected();
            
            hero.Entity.Name = "Hero " + currentPlacedHero.Value.Name;
            hero.Transformation.Position = position.SetZ(1.0f);
            hero.Root = position;

            hero.Entity.AddComponent<AudioSource>();
            
            RigidBody body = hero.Entity.AddComponent<RigidBody>();
            body.RigidBodyType = RigidBodyType.Dynamic;
            body.Mass = hero.Mass;
            body.GravityScale = 0.0f;
            body.FixedRotation = true;
            CircleCollider collider = hero.Entity.AddComponent<CircleCollider>();
            collider.Radius = hero.Radius;

            hero.Entity.AddComponent<Animator>();
            
            Heroes.Add(hero);
            currentPlacedHero = null;
            placedIcons.Destroy();
        }

        public void TryPlaceHero(HeroStorage.HeroInfo heroInfo)
        {
            StopPlacing();
            
            currentPlacedHero = heroInfo;

            CreatePlaceIcon(heroInfo.MovementRange);
        }

        private void CreatePlaceIcon(float range)
        {
            if (placedIcons != null)
                placedIcons.Destroy();
            
            placedIcons = new Entity();
            
            Entity icon1 = new Entity("Icon1", placedIcons);
            icon1.AddComponent<SpriteRenderer>().Sprite = ResourcesManager.LoadImage("range.png");
            float scale = range * 32f / 64f;
            icon1.Transformation.Scale = new Vector3(scale, scale, scale);
            
            Entity icon2 = new Entity("Icon2", placedIcons);
            icon2.AddComponent<SpriteRenderer>().Sprite = ResourcesManager.LoadImage("placement.png");
        }

        private void CreateCastIcon()
        {
            if (placedIcons != null)
                placedIcons.Destroy();
            
            placedIcons = new Entity();
            
            Entity icon = new Entity("Icon", placedIcons);
            icon.AddComponent<SpriteRenderer>().Sprite = ResourcesManager.LoadImage("cast_icon.png");
        }

        public void TryCastSpell(SpellStorage.SpellInfo spellInfo)
        {
            StopPlacing();
            
            currentCastedSpell = spellInfo;
            CreateCastIcon();
            
            if (!spellInfo.NeedPosition)
            {
                CastSpell(Vector3.Zero);
            }
        }

        private void CastSpell(Vector3 position)
        {
            if (!currentCastedSpell.HasValue)
                return;
            
            Spell spell = currentCastedSpell.Value.Spawner.Invoke();
            Inventory.RemoveSelected();
            
            spell.Cast(position);
            
            currentCastedSpell = null;
            placedIcons.Destroy();
        }
    }
}