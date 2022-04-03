using System;
using System.Collections.Generic;
using Steel;

namespace SteelCustom.Spells
{
    public static class SpellStorage
    {
        public enum SpellType
        {
            Meteor = 0,
            Blizzard = 1,
            HealWave = 2,
            Rage = 4,
        }
        
        public struct SpellInfo
        {
            public Func<Spell> Spawner;
            public string Name;
            public string Description;
            public Sprite Icon;
            public int Cost;
            public bool NeedPosition;
        }

        public static readonly List<SpellInfo> Spells = new List<SpellInfo>
        {
            new SpellInfo
            {
                Spawner = () =>
                {
                    Meteor spell = new Entity().AddComponent<Meteor>();

                    return spell;
                },
                Name = "Meteor",
                Description = "Deal high damage in small area",
                Icon = ResourcesManager.LoadImage("fireball_icon.png"),
                Cost = 60,
                NeedPosition = true,
            },
            new SpellInfo
            {
                Spawner = () =>
                {
                    Blizzard spell = new Entity().AddComponent<Blizzard>();

                    return spell;
                },
                Name = "Blizzard",
                Description = "Freeze all enemies on the map",
                Icon = ResourcesManager.LoadImage("blizzard_icon.png"),
                Cost = 35,
                NeedPosition = false,
            },
            new SpellInfo
            {
                Spawner = () =>
                {
                    HealWave spell = new Entity().AddComponent<HealWave>();

                    return spell;
                },
                Name = "Healing wave",
                Description = "Heal all allied units on the map",
                Icon = ResourcesManager.LoadImage("heal_wave_icon.png"),
                Cost = 30,
                NeedPosition = false,
            },
            new SpellInfo
            {
                Spawner = () =>
                {
                    Rage spell = new Entity().AddComponent<Rage>();

                    return spell;
                },
                Name = "Rage",
                Description = "Increase attack speed of all allied units on the map for short duration",
                Icon = ResourcesManager.LoadImage("rage_icon.png"),
                Cost = 50,
                NeedPosition = false,
            },
        };
    }
}