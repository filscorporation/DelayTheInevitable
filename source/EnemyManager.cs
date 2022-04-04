using System.Collections.Generic;
using Steel;
using SteelCustom.Enemies;

namespace SteelCustom
{
    public class EnemyManager : ScriptComponent
    {
        #region Waves

        private struct WaveSubInfo
        {
            public float GroupDelay;
            public EnemyStorage.EnemyType EnemyType;
            public int MinEnemyCount;
            public int MaxEnemyCount;
        }
        
        private struct WaveInfo
        {
            public float Duration;
            public List<WaveSubInfo> EnemyGroups;
        }

        private readonly List<WaveInfo> waves = new List<WaveInfo>
        {
            new WaveInfo
            {
                Duration = 45f,
                EnemyGroups = new List<WaveSubInfo>
                {
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 3.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 2,
                        GroupDelay = 15.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 2,
                        GroupDelay = 30.0f,
                    },
                }
            },
            new WaveInfo
            {
                Duration = 80f,
                EnemyGroups = new List<WaveSubInfo>
                {
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 3,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 4,
                        GroupDelay = 20.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 4,
                        GroupDelay = 35.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 4,
                        GroupDelay = 50.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 50.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 4,
                        GroupDelay = 65.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 65.0f,
                    },
                }
            },
            new WaveInfo
            {
                Duration = 90f,
                EnemyGroups = new List<WaveSubInfo>
                {
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 8,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 8,
                        MaxEnemyCount = 10,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 2,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 8,
                        MaxEnemyCount = 10,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 3,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 60.0f,
                    },
                }
            },
            new WaveInfo
            {
                Duration = 90f,
                EnemyGroups = new List<WaveSubInfo>
                {
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 4,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 4,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 5,
                        MaxEnemyCount = 6,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 2,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 3,
                        GroupDelay = 60.0f,
                    },
                }
            },
            new WaveInfo
            {
                Duration = 100f,
                EnemyGroups = new List<WaveSubInfo>
                {
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 15,
                        MaxEnemyCount = 20,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 1,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 3,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 20,
                        MaxEnemyCount = 25,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 3,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 4,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 20,
                        MaxEnemyCount = 30,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 3,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 5,
                        MaxEnemyCount = 6,
                        GroupDelay = 60.0f,
                    },
                }
            },
            new WaveInfo
            {
                Duration = 65f,
                EnemyGroups = new List<WaveSubInfo>
                {
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 5.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 10.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 2,
                        GroupDelay = 10.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 15.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 20.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 2,
                        GroupDelay = 20.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 25.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 2,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 35.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 6,
                        MaxEnemyCount = 12,
                        GroupDelay = 40.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 2,
                        GroupDelay = 40.0f,
                    },
                },
            },
            new WaveInfo
            {
                Duration = 90f,
                EnemyGroups = new List<WaveSubInfo>
                {
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 20,
                        MaxEnemyCount = 30,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 8,
                        MaxEnemyCount = 12,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 4,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 2,
                        GroupDelay = 0.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 25,
                        MaxEnemyCount = 35,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 9,
                        MaxEnemyCount = 14,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 4,
                        GroupDelay = 30.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 1,
                        MaxEnemyCount = 2,
                        GroupDelay = 3.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Goblin,
                        MinEnemyCount = 30,
                        MaxEnemyCount = 40,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Orc,
                        MinEnemyCount = 10,
                        MaxEnemyCount = 15,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Ogre,
                        MinEnemyCount = 3,
                        MaxEnemyCount = 4,
                        GroupDelay = 60.0f,
                    },
                    new WaveSubInfo
                    {
                        EnemyType = EnemyStorage.EnemyType.Hunter,
                        MinEnemyCount = 2,
                        MaxEnemyCount = 3,
                        GroupDelay = 60.0f,
                    },
                }
            },
        };

        #endregion
        
        public List<Enemy> Units = new List<Enemy>();

        public bool SkipNextWave = false;

        public int CurrentWave => currentWave + 1;

        private int currentWave = 0;
        private int currentWaveGroup = 0;
        private float currentWaveTimer = 0.0f;
        
        public static float WaveMultiplier = 1.0f;

        private UIText waveText;

        public override void OnCreate()
        {
            //Spawn(EnemyStorage.EnemyType.Goblin, 1, true);
            //Spawn(EnemyStorage.EnemyType.Orc, 1, true);
            //Spawn(EnemyStorage.EnemyType.Ogre, 1, true);
            //Spawn(EnemyStorage.EnemyType.Hunter, 1, false);
            
            waveText = UI.CreateUIText($"Wave: {currentWave + 1}", "Wave", GameManager.InGameUIRoot);
            waveText.TextAlignment = AlignmentType.CenterLeft;
            waveText.Color = Color.Black;
            waveText.TextSize = 16;
            waveText.RectTransform.AnchorMin = new Vector2(0.0f, 1.0f);
            waveText.RectTransform.AnchorMax = new Vector2(0.0f, 1.0f);
            waveText.RectTransform.AnchoredPosition = new Vector2(210, -32);
            waveText.RectTransform.Pivot = new Vector2(0.0f, 1.0f);
            waveText.RectTransform.Size = new Vector2(200, 20);
        }

        public override void OnUpdate()
        {
            UpdateSpawn();
        }

        private void UpdateSpawn()
        {
            currentWaveTimer += Time.DeltaTime;
                
            while (currentWaveGroup < waves[currentWave].EnemyGroups.Count
                   && currentWaveTimer >= waves[currentWave].EnemyGroups[currentWaveGroup].GroupDelay)
            {
                SpawnGroup(waves[currentWave].EnemyGroups[currentWaveGroup]);
                    
                currentWaveGroup++;
            }

            if (currentWaveTimer >= waves[currentWave].Duration || SkipNextWave)
            {
                SkipNextWave = false;
                
                if (currentWave == waves.Count - 1)
                {
                    Log.LogInfo("Last wave X");
                    WaveMultiplier += 1.0f;
                    waveText.Text = $"Wave: {currentWave + 1} (x{(int)WaveMultiplier})";
                }
                else
                {
                    Log.LogInfo("Next wave");
                    currentWave++;
                    waveText.Text = $"Wave: {currentWave + 1}";
                }

                currentWaveTimer = 0.0f;
                currentWaveGroup = 0;
            }
        }

        private void SpawnGroup(WaveSubInfo groupInfo)
        {
            int count = Random.NextInt(groupInfo.MinEnemyCount, groupInfo.MaxEnemyCount);
            count = (int)Math.Round(count);
            Spawn(groupInfo.EnemyType, count);
        }

        private void Spawn(EnemyStorage.EnemyType enemyType, int count, bool test = false)
        {
            float x = Random.NextFloat(-6.0f, 6.0f);

            for (int i = 0; i < count; i++)
            {
                float y = Random.NextFloat(-2.0f, 2.0f);
                Vector3 position = new Vector3(x + Random.NextFloat(-1.0f, 1.0f), (test ? 0.0f : 8.0f) + y, 0.0f);

                Enemy unit = EnemyStorage.Units[(int)enemyType].Spawner.Invoke();

                unit.Entity.Name = "Enemy";
                unit.Transformation.Position = position.SetZ(1.0f);

                if (WaveMultiplier > 1.1f)
                {
                    unit.MaxHealth *= WaveMultiplier;
                    unit.Health = unit.MaxHealth;
                    unit.AttackDamage *= WaveMultiplier;
                }

                unit.Entity.AddComponent<AudioSource>();
                
                RigidBody body = unit.Entity.AddComponent<RigidBody>();
                body.RigidBodyType = RigidBodyType.Dynamic;
                body.Mass = unit.Mass;
                body.GravityScale = 0.0f;
                body.FixedRotation = true;
                CircleCollider collider = unit.Entity.AddComponent<CircleCollider>();
                collider.Radius = unit.Radius;

                Units.Add(unit);
            }
        }
    }
}