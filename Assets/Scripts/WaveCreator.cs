//using UnityEngine;
//using System.Collections.Generic;

//public class WaveCreator : MonoBehaviour
//{
//    [Header("Wave Creation")]
//    public bool createWavesOnStart = true;
//    public int numberOfLevels = 10;
//    public int wavesPerLevel = 5;
    
//    [Header("Enemy References")]
//    public EnemyData infantryEnemy;
//    public EnemyData tankEnemy;
//    public EnemyData aircraftEnemy;
//    public EnemyData bossEnemy;
    
//    [Header("Wave Templates")]
//    public WaveTemplate[] waveTemplates;
    
//    [System.Serializable]
//    public class WaveTemplate
//    {
//        public string templateName;
//        public EnemySpawnData[] enemySpawns;
//        public float difficultyMultiplier = 1f;
//    }
    
//    private LevelWaveManager waveManager;
    
//    void Start()
//    {
//        if (createWavesOnStart)
//        {
//            CreateAllWaves();
//        }
//    }
    
//    [ContextMenu("Create All Waves")]
//    public void CreateAllWaves()
//    {
//        waveManager = FindObjectOfType<LevelWaveManager>();
//        if (waveManager == null)
//        {
//            Debug.LogError("LevelWaveManager not found! Please add it to the scene first.");
//            return;
//        }
        
//        // Tạo wave templates nếu chưa có
//        if (waveTemplates == null || waveTemplates.Length == 0)
//        {
//            CreateWaveTemplates();
//        }
        
//        // Tạo levels và waves
//        for (int level = 1; level <= numberOfLevels; level++)
//        {
//            CreateLevelWaves(level);
//        }
        
//        Debug.Log($"Created {numberOfLevels} levels with {wavesPerLevel} waves each!");
//    }
    
//    void CreateWaveTemplates()
//    {
//        waveTemplates = new WaveTemplate[]
//        {
//            // Wave 1: Infantry Rush
//            new WaveTemplate
//            {
//                templateName = "Infantry Rush",
//                difficultyMultiplier = 1f,
//                enemySpawns = new EnemySpawnData[]
//                {
//                    new EnemySpawnData
//                    {
//                        enemyData = infantryEnemy,
//                        count = 8,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Sequential,
//                        spawnInterval = 1f
//                    }
//                }
//            },
            
//            // Wave 2: Mixed Infantry
//            new WaveTemplate
//            {
//                templateName = "Mixed Infantry",
//                difficultyMultiplier = 1.2f,
//                enemySpawns = new EnemySpawnData[]
//                {
//                    new EnemySpawnData
//                    {
//                        enemyData = infantryEnemy,
//                        count = 6,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Group,
//                        spawnInterval = 0.8f,
//                        spawnGroupSize = 2
//                    },
//                    new EnemySpawnData
//                    {
//                        enemyData = infantryEnemy,
//                        count = 4,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Sequential,
//                        spawnInterval = 1.2f,
//                        delayBeforeThisEnemy = 3f
//                    }
//                }
//            },
            
//            // Wave 3: Tank Introduction
//            new WaveTemplate
//            {
//                templateName = "Tank Introduction",
//                difficultyMultiplier = 1.5f,
//                enemySpawns = new EnemySpawnData[]
//                {
//                    new EnemySpawnData
//                    {
//                        enemyData = infantryEnemy,
//                        count = 4,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Sequential,
//                        spawnInterval = 1f
//                    },
//                    new EnemySpawnData
//                    {
//                        enemyData = tankEnemy,
//                        count = 2,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Sequential,
//                        spawnInterval = 2f,
//                        delayBeforeThisEnemy = 2f
//                    }
//                }
//            },
            
//            // Wave 4: Air Attack
//            new WaveTemplate
//            {
//                templateName = "Air Attack",
//                difficultyMultiplier = 1.8f,
//                enemySpawns = new EnemySpawnData[]
//                {
//                    new EnemySpawnData
//                    {
//                        enemyData = aircraftEnemy,
//                        count = 3,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Group,
//                        spawnInterval = 1.5f,
//                        spawnGroupSize = 2
//                    },
//                    new EnemySpawnData
//                    {
//                        enemyData = infantryEnemy,
//                        count = 6,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Sequential,
//                        spawnInterval = 0.8f,
//                        delayBeforeThisEnemy = 1f
//                    }
//                }
//            },
            
//            // Wave 5: Boss Wave
//            new WaveTemplate
//            {
//                templateName = "Boss Wave",
//                difficultyMultiplier = 2.5f,
//                enemySpawns = new EnemySpawnData[]
//                {
//                    new EnemySpawnData
//                    {
//                        enemyData = infantryEnemy,
//                        count = 8,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Burst,
//                        spawnInterval = 0.5f
//                    },
//                    new EnemySpawnData
//                    {
//                        enemyData = tankEnemy,
//                        count = 3,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Sequential,
//                        spawnInterval = 2f,
//                        delayBeforeThisEnemy = 2f
//                    },
//                    new EnemySpawnData
//                    {
//                        enemyData = bossEnemy,
//                        count = 1,
//                        spawnPattern = LevelWaveManager.SpawnPattern.Sequential,
//                        spawnInterval = 1f,
//                        delayBeforeThisEnemy = 5f
//                    }
//                }
//            }
//        };
//    }
    
//    void CreateLevelWaves(int levelNumber)
//    {
//        LevelWaveManager.LevelData levelData = new LevelWaveManager.LevelData
//        {
//            levelNumber = levelNumber,
//            levelName = $"Level {levelNumber}",
//            levelDescription = $"Level {levelNumber} - Defend against waves of enemies!",
//            totalWaves = wavesPerLevel,
//            levelTimeLimit = 300f + (levelNumber * 30f), // Tăng thời gian theo level
//            playerStartingLives = 20 - (levelNumber / 3), // Giảm máu theo level
//            playerStartingMoney = 500 + (levelNumber * 50), // Tăng tiền theo level
            
//            // Scaling theo level
//            healthMultiplier = 1f + (levelNumber * 0.1f),
//            speedMultiplier = 1f + (levelNumber * 0.05f),
//            damageMultiplier = 1f + (levelNumber * 0.08f),
//            rewardMultiplier = 1f + (levelNumber * 0.15f),
            
//            waves = new List<LevelWaveManager.WaveData>()
//        };
        
//        // Tạo waves cho level
//        for (int waveIndex = 0; waveIndex < wavesPerLevel; waveIndex++)
//        {
//            LevelWaveManager.WaveData waveData = CreateWaveData(levelNumber, waveIndex + 1);
//            levelData.waves.Add(waveData);
//        }
        
//        // Thêm level vào wave manager
//        waveManager.levels.Add(levelData);
//    }
    
//    LevelWaveManager.WaveData CreateWaveData(int levelNumber, int waveNumber)
//    {
//        // Chọn template dựa trên wave number
//        int templateIndex = (waveNumber - 1) % waveTemplates.Length;
//        WaveTemplate template = waveTemplates[templateIndex];
        
//        // Tính difficulty multiplier
//        float levelDifficulty = 1f + (levelNumber - 1) * 0.2f;
//        float waveDifficulty = 1f + (waveNumber - 1) * 0.1f;
//        float totalDifficulty = template.difficultyMultiplier * levelDifficulty * waveDifficulty;
        
//        LevelWaveManager.WaveData waveData = new LevelWaveManager.WaveData
//        {
//            waveName = $"{template.templateName} (L{levelNumber}W{waveNumber})",
//            waveNumber = waveNumber,
//            delayBeforeWave = 2f + (waveNumber * 0.5f),
//            waitForAllEnemiesDead = true,
//            maxWaveTime = 60f + (waveNumber * 10f),
//            enemySpawns = new List<LevelWaveManager.EnemySpawnData>()
//        };
        
//        // Tạo enemy spawns từ template
//        foreach (EnemySpawnData templateSpawn in template.enemySpawns)
//        {
//            LevelWaveManager.EnemySpawnData spawnData = new LevelWaveManager.EnemySpawnData
//            {
//                enemyData = templateSpawn.enemyData,
//                count = Mathf.RoundToInt(templateSpawn.count * totalDifficulty),
//                spawnPattern = templateSpawn.spawnPattern,
//                spawnInterval = templateSpawn.spawnInterval,
//                spawnGroupSize = templateSpawn.spawnGroupSize,
//                delayBeforeThisEnemy = templateSpawn.delayBeforeThisEnemy,
//                delayAfterThisEnemy = templateSpawn.delayAfterThisEnemy,
                
//                // Custom modifiers
//                useCustomStats = true,
//                customHealthMultiplier = totalDifficulty,
//                customSpeedMultiplier = 1f + (levelNumber * 0.05f),
//                customDamageMultiplier = totalDifficulty,
//                customRewardMultiplier = 1f + (levelNumber * 0.1f)
//            };
            
//            waveData.enemySpawns.Add(spawnData);
//        }
        
//        return waveData;
//    }
    
//    [ContextMenu("Clear All Waves")]
//    public void ClearAllWaves()
//    {
//        waveManager = FindObjectOfType<LevelWaveManager>();
//        if (waveManager != null)
//        {
//            waveManager.levels.Clear();
//            Debug.Log("Cleared all waves!");
//        }
//    }
    
//    [ContextMenu("Create Test Wave")]
//    public void CreateTestWave()
//    {
//        waveManager = FindObjectOfType<LevelWaveManager>();
//        if (waveManager == null)
//        {
//            Debug.LogError("LevelWaveManager not found!");
//            return;
//        }
        
//        // Tạo test level
//        LevelWaveManager.LevelData testLevel = new LevelWaveManager.LevelData
//        {
//            levelNumber = 999,
//            levelName = "Test Level",
//            levelDescription = "Test level for debugging",
//            totalWaves = 1,
//            levelTimeLimit = 300f,
//            playerStartingLives = 20,
//            playerStartingMoney = 1000,
//            waves = new List<LevelWaveManager.WaveData>()
//        };
        
//        // Tạo test wave
//        LevelWaveManager.WaveData testWave = new LevelWaveManager.WaveData
//        {
//            waveName = "Test Wave",
//            waveNumber = 1,
//            delayBeforeWave = 1f,
//            waitForAllEnemiesDead = true,
//            maxWaveTime = 60f,
//            enemySpawns = new List<LevelWaveManager.EnemySpawnData>()
//        };
        
//        // Thêm test enemies
//        if (infantryEnemy != null)
//        {
//            testWave.enemySpawns.Add(new LevelWaveManager.EnemySpawnData
//            {
//                enemyData = infantryEnemy,
//                count = 3,
//                spawnPattern = LevelWaveManager.SpawnPattern.Sequential,
//                spawnInterval = 1f
//            });
//        }
        
//        testLevel.waves.Add(testWave);
//        waveManager.levels.Add(testLevel);
        
//        Debug.Log("Created test wave!");
//    }
    
//    void OnGUI()
//    {
//        if (!Application.isPlaying) return;
        
//        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
//        GUILayout.Label("Wave Creator Debug");
        
//        if (GUILayout.Button("Create All Waves"))
//        {
//            CreateAllWaves();
//        }
        
//        if (GUILayout.Button("Create Test Wave"))
//        {
//            CreateTestWave();
//        }
        
//        if (GUILayout.Button("Clear All Waves"))
//        {
//            ClearAllWaves();
//        }
        
//        if (waveManager != null)
//        {
//            GUILayout.Label($"Levels: {waveManager.levels.Count}");
//            if (waveManager.levels.Count > 0)
//            {
//                GUILayout.Label($"Current Level: {waveManager.currentLevel}");
//                GUILayout.Label($"Current Wave: {waveManager.currentWaveIndex + 1}");
//            }
//        }
        
//        GUILayout.EndArea();
//    }
//} 