using System;
using System.Collections.Generic;
using Steel;

namespace SteelCustom.Heroes
{
    public static class HeroStorage
    {
        public struct HeroInfo
        {
            public Func<Hero> Spawner;
            public string Name;
            public string Description;
            public Sprite Icon;
            public int Level;
            public int Cost;
            public float MovementRange;
        }
        
        public static readonly List<HeroInfo> Heroes = new List<HeroInfo>
        {
            new HeroInfo
            {
                Spawner = () =>
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("warrior.aseprite").CreateEntityFromAsepriteData();
                    Warrior hero = entity.AddComponent<Warrior>();

                    hero.Name = "Warrior";
                    hero.MaxHealth = 20;
                    hero.Health = 20;
                    hero.AttackDamage = 5;
                    hero.AttackDelay = 1.2f;
                    hero.AttackRange = 0.8f;
                    hero.MovementRange = 2.5f;
                    hero.ViewRange = 4.0f;
                    hero.Speed = 0.3f;

                    hero.AttackKnockBack = 50.0f;
                    hero.Mass = 1.0f;
                    hero.Radius = 0.25f;

                    return hero;
                },
                Name = "Warrior",
                Description = "Hero with good health, descent damage and low speed and view range. Can taunt enemies when upgraded.",
                Icon = ResourcesManager.LoadImage("warrior.aseprite"),
                Level = 1,
                Cost = 20,
                MovementRange = 2.5f,
            },
            new HeroInfo
            {
                Spawner = () =>
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("archer.aseprite").CreateEntityFromAsepriteData();
                    Archer hero = entity.AddComponent<Archer>();

                    hero.Name = "Archer";
                    hero.MaxHealth = 5;
                    hero.Health = 5;
                    hero.AttackDamage = 3;
                    hero.AttackDelay = 2.0f;
                    hero.AttackRange = 4.0f;
                    hero.MovementRange = 4.0f;
                    hero.ViewRange = 6.0f;
                    hero.Speed = 0.6f;

                    hero.AttackKnockBack = 35.0f;
                    hero.Mass = 0.8f;
                    hero.Radius = 0.25f;

                    return hero;
                },
                Name = "Archer",
                Description = "Hero with low health, but the ability to shoot enemies from the distance. Fires piercing shot when when upgraded.",
                Icon = ResourcesManager.LoadImage("archer.aseprite"),
                Level = 1,
                Cost = 20,
                MovementRange = 4.0f,
            },
            new HeroInfo
            {
                Spawner = () =>
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("priest.aseprite").CreateEntityFromAsepriteData();
                    Priest hero = entity.AddComponent<Priest>();

                    hero.Name = "Priest";
                    hero.MaxHealth = 10;
                    hero.Health = 10;
                    hero.HealAmount = 3;
                    hero.AttackDelay = 3.0f;
                    hero.AttackRange = 3.0f;
                    hero.AttackDamage = 0.0f;
                    hero.MovementRange = 4.0f;
                    hero.ViewRange = 5.0f;
                    hero.Speed = 0.2f;

                    hero.AttackKnockBack = 0.0f;
                    hero.Mass = 0.8f;
                    hero.Radius = 0.25f;

                    return hero;
                },
                Name = "Priest",
                Description = "Slow hero that can heal allies. Can cast heal wave at higher level.",
                Icon = ResourcesManager.LoadImage("priest.aseprite"),
                Level = 1,
                Cost = 40,
                MovementRange = 4.0f,
            },
        };
    }
}