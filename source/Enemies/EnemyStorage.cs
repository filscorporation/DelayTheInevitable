using System;
using System.Collections.Generic;
using Steel;

namespace SteelCustom.Enemies
{
    public static class EnemyStorage
    {
        public enum EnemyType
        {
            Goblin = 0,
            Orc = 1,
            Ogre = 2,
            Hunter = 3,
        }

        public struct EnemyInfo
        {
            public Func<Enemy> Spawner;
        }

        public static readonly List<EnemyInfo> Units = new List<EnemyInfo>
        {
            new EnemyInfo // Goblin
            {
                Spawner = () =>
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("goblin.aseprite").CreateEntityFromAsepriteData();
                    Enemy unit = entity.AddComponent<Enemy>();

                    unit.EnemyWeight = 1;
                    unit.MaxHealth = 20;
                    unit.Health = 20;
                    unit.AttackDamage = 3;
                    unit.AttackDelay = 1.2f;
                    unit.AttackRange = 0.6f;
                    unit.ViewRange = 3.0f;
                    unit.Speed = 0.45f;

                    unit.AttackKnockBack = 5f;
                    unit.Mass = 1.0f;
                    unit.Radius = 0.25f;

                    return unit;
                }
            },
            new EnemyInfo // Orc
            {
                Spawner = () =>
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("orc.aseprite").CreateEntityFromAsepriteData();
                    Enemy unit = entity.AddComponent<Enemy>();

                    unit.EnemyWeight = 2;
                    unit.MaxHealth = 80;
                    unit.Health = 80;
                    unit.AttackDamage = 7;
                    unit.AttackDelay = 2.2f;
                    unit.AttackRange = 0.9f;
                    unit.ViewRange = 3.0f;
                    unit.Speed = 0.2f;

                    unit.AttackKnockBack = 15f;
                    unit.Mass = 1.5f;
                    unit.Radius = 0.3f;

                    return unit;
                }
            },
            new EnemyInfo // Ogre
            {
                Spawner = () =>
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("ogre.aseprite").CreateEntityFromAsepriteData();
                    Orge unit = entity.AddComponent<Orge>();

                    unit.EnemyWeight = 5;
                    unit.MaxHealth = 300;
                    unit.Health = 300;
                    unit.AttackDamage = 10;
                    unit.AttackDelay = 3.0f;
                    unit.AttackNormalTime = 1.0f;
                    unit.AttackRange = 1.2f;
                    unit.ViewRange = 2.5f;
                    unit.Speed = 0.1f;

                    unit.AttackKnockBack = 50f;
                    unit.Mass = 3.0f;
                    unit.Radius = 0.5f;

                    return unit;
                }
            },
            new EnemyInfo // Hunter
            {
                Spawner = () =>
                {
                    Entity entity = ResourcesManager.LoadAsepriteData("hunter.aseprite").CreateEntityFromAsepriteData();
                    Hunter unit = entity.AddComponent<Hunter>();

                    unit.EnemyWeight = 2;
                    unit.MaxHealth = 1;
                    unit.Health = 1;
                    unit.AttackDamage = 50;
                    unit.AttackDelay = 0.0f;
                    unit.AttackNormalTime = 0.0f;
                    unit.AttackRange = 0.9f;
                    unit.ViewRange = 4.0f;
                    unit.Speed = 1.2f;

                    unit.AttackKnockBack = 50f;
                    unit.Mass = 0.5f;
                    unit.Radius = 0.2f;

                    return unit;
                }
            },
        };
    }
}