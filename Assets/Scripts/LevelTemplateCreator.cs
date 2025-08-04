using UnityEngine;
using System.Collections.Generic;

public class LevelTemplateCreator : MonoBehaviour
{
    [Header("Available Enemy Data")]
    public List<EnemyData> availableEnemyData = new List<EnemyData>();
    
    [Header("Level Templates")]
    public List<LevelTemplate> levelTemplates = new List<LevelTemplate>();
    
    [System.Serializable]
    public class LevelTemplate
    {
        public string templateName = "Level Template";
        public string description = "Template description";
        public int levelNumber = 1;
        public List<WaveTemplate> waves = new List<WaveTemplate>();
        
        [Header("Level Scaling")]
        public float healthMultiplier = 1f;
        public float speedMultiplier = 1f;
        public float damageMultiplier = 1f;
        public float rewardMultiplier = 1f;
    }
    
    [System.Serializable]
    public class WaveTemplate
    {
        public string waveName = "Wave";
        public int waveNumber = 1;
        public List<EnemySpawnTemplate> enemySpawns = new List<EnemySpawnTemplate>();
        public float delayBeforeWave = 2f;
        public float maxWaveTime = 60f;
    }
    
    [System.Serializable]
    public class EnemySpawnTemplate
    {
        public EnemyType enemyType;
        public int count = 5;
        public LevelWaveManager.SpawnPattern spawnPattern = LevelWaveManager.SpawnPattern.Sequential;
        public float spawnInterval = 0.5f;
        public int spawnGroupSize = 3;
        
        [Header("Custom Modifiers")]
        public bool useCustomModifiers = false;
        public float customHealthMultiplier = 1f;
        public float customSpeedMultiplier = 1f;
        public float customDamageMultiplier = 1f;
        public float customRewardMultiplier = 1f;
    }
    
    [ContextMenu("Create Sample Levels")]
    public void CreateSampleLevels()
    {
        LevelWaveManager levelManager = FindObjectOfType<LevelWaveManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelWaveManager not found!");
            return;
        }
        
        levelManager.levels.Clear();
        
        // Level 1: Tutorial - Infantry only
        CreateTutorialLevel(levelManager);
        
        // Level 2: Mixed - Infantry + Tank
        CreateMixedLevel(levelManager);
        
        // Level 3: Air Attack - Aircraft + Boss
        CreateAirAttackLevel(levelManager);
        
        // Level 4: Boss Rush - Multiple Bosses
        CreateBossRushLevel(levelManager);
        
        // Level 5: Ultimate Challenge - All types with scaling
        CreateUltimateLevel(levelManager);
        
        Debug.Log("Created " + levelManager.levels.Count + " sample levels!");
    }
    
    void CreateTutorialLevel(LevelWaveManager levelManager)
    {
        LevelWaveManager.LevelData level = new LevelWaveManager.LevelData();
        level.levelNumber = 1;
        level.levelName = "Tutorial Level";
        level.levelDescription = "Learn the basics with infantry enemies";
        level.totalWaves = 3;
        level.healthMultiplier = 0.8f; // Weaker enemies
        level.speedMultiplier = 0.7f;
        level.rewardMultiplier = 1.2f; // More rewards for tutorial
        
        // Wave 1: Basic Infantry
        CreateWave(level, 1, "Basic Infantry", EnemyType.Infantry, 5, LevelWaveManager.SpawnPattern.Sequential, 1f);
        
        // Wave 2: More Infantry
        CreateWave(level, 2, "More Infantry", EnemyType.Infantry, 8, LevelWaveManager.SpawnPattern.Group, 0.8f);
        
        // Wave 3: Infantry Rush
        CreateWave(level, 3, "Infantry Rush", EnemyType.Infantry, 12, LevelWaveManager.SpawnPattern.Burst, 0.5f);
        
        levelManager.levels.Add(level);
    }
    
    void CreateMixedLevel(LevelWaveManager levelManager)
    {
        LevelWaveManager.LevelData level = new LevelWaveManager.LevelData();
        level.levelNumber = 2;
        level.levelName = "Mixed Forces";
        level.levelDescription = "Infantry and Tank enemies";
        level.totalWaves = 4;
        level.healthMultiplier = 1f;
        level.speedMultiplier = 1f;
        level.damageMultiplier = 1.1f;
        
        // Wave 1: Infantry
        CreateWave(level, 1, "Infantry Wave", EnemyType.Infantry, 6, LevelWaveManager.SpawnPattern.Sequential, 0.8f);
        
        // Wave 2: Mixed
        CreateMixedWave(level, 2, "Mixed Wave", EnemyType.Infantry, 4, EnemyType.Tank, 2);
        
        // Wave 3: Tank Focus
        CreateWave(level, 3, "Tank Wave", EnemyType.Tank, 4, LevelWaveManager.SpawnPattern.Sequential, 1.5f);
        
        // Wave 4: Final Mixed
        CreateMixedWave(level, 4, "Final Mixed", EnemyType.Infantry, 8, EnemyType.Tank, 3);
        
        levelManager.levels.Add(level);
    }
    
    void CreateAirAttackLevel(LevelWaveManager levelManager)
    {
        LevelWaveManager.LevelData level = new LevelWaveManager.LevelData();
        level.levelNumber = 3;
        level.levelName = "Air Attack";
        level.levelDescription = "Aircraft and Boss enemies";
        level.totalWaves = 5;
        level.healthMultiplier = 1.2f;
        level.speedMultiplier = 1.3f;
        level.damageMultiplier = 1.2f;
        
        // Wave 1: Aircraft
        CreateWave(level, 1, "Aircraft Wave", EnemyType.Aircraft, 3, LevelWaveManager.SpawnPattern.Random, 1.2f);
        
        // Wave 2: More Aircraft
        CreateWave(level, 2, "More Aircraft", EnemyType.Aircraft, 5, LevelWaveManager.SpawnPattern.Group, 1f);
        
        // Wave 3: Boss Introduction
        CreateWave(level, 3, "Boss Intro", EnemyType.Boss, 1, LevelWaveManager.SpawnPattern.Burst, 0f);
        
        // Wave 4: Aircraft + Boss
        CreateMixedWave(level, 4, "Air + Boss", EnemyType.Aircraft, 4, EnemyType.Boss, 1);
        
        // Wave 5: Boss Rush
        CreateWave(level, 5, "Boss Rush", EnemyType.Boss, 2, LevelWaveManager.SpawnPattern.Sequential, 3f);
        
        levelManager.levels.Add(level);
    }
    
    void CreateBossRushLevel(LevelWaveManager levelManager)
    {
        LevelWaveManager.LevelData level = new LevelWaveManager.LevelData();
        level.levelNumber = 4;
        level.levelName = "Boss Rush";
        level.levelDescription = "Multiple powerful bosses";
        level.totalWaves = 3;
        level.healthMultiplier = 1.5f;
        level.speedMultiplier = 1.2f;
        level.damageMultiplier = 1.4f;
        level.rewardMultiplier = 2f; // High rewards for boss level
        
        // Wave 1: Single Boss
        CreateWave(level, 1, "Boss Wave", EnemyType.Boss, 1, LevelWaveManager.SpawnPattern.Burst, 0f);
        
        // Wave 2: Double Boss
        CreateWave(level, 2, "Double Boss", EnemyType.Boss, 2, LevelWaveManager.SpawnPattern.Sequential, 5f);
        
        // Wave 3: Triple Boss
        CreateWave(level, 3, "Triple Boss", EnemyType.Boss, 3, LevelWaveManager.SpawnPattern.Sequential, 4f);
        
        levelManager.levels.Add(level);
    }
    
    void CreateUltimateLevel(LevelWaveManager levelManager)
    {
        LevelWaveManager.LevelData level = new LevelWaveManager.LevelData();
        level.levelNumber = 5;
        level.levelName = "Ultimate Challenge";
        level.levelDescription = "All enemy types with maximum difficulty";
        level.totalWaves = 6;
        level.healthMultiplier = 1.8f;
        level.speedMultiplier = 1.5f;
        level.damageMultiplier = 1.6f;
        level.rewardMultiplier = 3f; // Maximum rewards
        
        // Wave 1: Infantry Rush
        CreateWave(level, 1, "Infantry Rush", EnemyType.Infantry, 10, LevelWaveManager.SpawnPattern.Burst, 0.3f);
        
        // Wave 2: Tank Assault
        CreateWave(level, 2, "Tank Assault", EnemyType.Tank, 6, LevelWaveManager.SpawnPattern.Group, 1.5f);
        
        // Wave 3: Air Strike
        CreateWave(level, 3, "Air Strike", EnemyType.Aircraft, 8, LevelWaveManager.SpawnPattern.Random, 1f);
        
        // Wave 4: Boss Trio
        CreateWave(level, 4, "Boss Trio", EnemyType.Boss, 3, LevelWaveManager.SpawnPattern.Sequential, 3f);
        
        // Wave 5: Mixed Chaos
        CreateAllTypesWave(level, 5, "Mixed Chaos");
        
        // Wave 6: Final Boss
        CreateWave(level, 6, "Final Boss", EnemyType.Boss, 1, LevelWaveManager.SpawnPattern.Burst, 0f);
        
        levelManager.levels.Add(level);
    }
    
    void CreateWave(LevelWaveManager.LevelData level, int waveNumber, string waveName, EnemyType enemyType, int count, LevelWaveManager.SpawnPattern pattern, float interval)
    {
        LevelWaveManager.WaveData wave = new LevelWaveManager.WaveData();
        wave.waveName = waveName;
        wave.waveNumber = waveNumber;
        wave.delayBeforeWave = 2f;
        wave.maxWaveTime = 60f;
        
        LevelWaveManager.EnemySpawnData spawnData = new LevelWaveManager.EnemySpawnData();
        spawnData.enemyData = FindEnemyDataByType(enemyType);
        spawnData.count = count;
        spawnData.spawnPattern = pattern;
        spawnData.spawnInterval = interval;
        spawnData.spawnGroupSize = 3;
        
        wave.enemySpawns.Add(spawnData);
        level.waves.Add(wave);
    }
    
    void CreateMixedWave(LevelWaveManager.LevelData level, int waveNumber, string waveName, EnemyType type1, int count1, EnemyType type2, int count2)
    {
        LevelWaveManager.WaveData wave = new LevelWaveManager.WaveData();
        wave.waveName = waveName;
        wave.waveNumber = waveNumber;
        wave.delayBeforeWave = 2f;
        wave.maxWaveTime = 60f;
        
        // First enemy type
        LevelWaveManager.EnemySpawnData spawn1 = new LevelWaveManager.EnemySpawnData();
        spawn1.enemyData = FindEnemyDataByType(type1);
        spawn1.count = count1;
        spawn1.spawnPattern = LevelWaveManager.SpawnPattern.Group;
        spawn1.spawnInterval = 0.8f;
        spawn1.delayAfterThisEnemy = 2f;
        wave.enemySpawns.Add(spawn1);
        
        // Second enemy type
        LevelWaveManager.EnemySpawnData spawn2 = new LevelWaveManager.EnemySpawnData();
        spawn2.enemyData = FindEnemyDataByType(type2);
        spawn2.count = count2;
        spawn2.spawnPattern = LevelWaveManager.SpawnPattern.Sequential;
        spawn2.spawnInterval = 1.5f;
        wave.enemySpawns.Add(spawn2);
        
        level.waves.Add(wave);
    }
    
    void CreateAllTypesWave(LevelWaveManager.LevelData level, int waveNumber, string waveName)
    {
        LevelWaveManager.WaveData wave = new LevelWaveManager.WaveData();
        wave.waveName = waveName;
        wave.waveNumber = waveNumber;
        wave.delayBeforeWave = 3f;
        wave.maxWaveTime = 90f;
        
        // Infantry
        LevelWaveManager.EnemySpawnData infantry = new LevelWaveManager.EnemySpawnData();
        infantry.enemyData = FindEnemyDataByType(EnemyType.Infantry);
        infantry.count = 6;
        infantry.spawnPattern = LevelWaveManager.SpawnPattern.Group;
        infantry.spawnInterval = 0.5f;
        infantry.delayAfterThisEnemy = 1f;
        wave.enemySpawns.Add(infantry);
        
        // Tank
        LevelWaveManager.EnemySpawnData tank = new LevelWaveManager.EnemySpawnData();
        tank.enemyData = FindEnemyDataByType(EnemyType.Tank);
        tank.count = 3;
        tank.spawnPattern = LevelWaveManager.SpawnPattern.Sequential;
        tank.spawnInterval = 1.5f;
        tank.delayAfterThisEnemy = 1f;
        wave.enemySpawns.Add(tank);
        
        // Aircraft
        LevelWaveManager.EnemySpawnData aircraft = new LevelWaveManager.EnemySpawnData();
        aircraft.enemyData = FindEnemyDataByType(EnemyType.Aircraft);
        aircraft.count = 4;
        aircraft.spawnPattern = LevelWaveManager.SpawnPattern.Random;
        aircraft.spawnInterval = 1f;
        aircraft.delayAfterThisEnemy = 1f;
        wave.enemySpawns.Add(aircraft);
        
        // Boss
        LevelWaveManager.EnemySpawnData boss = new LevelWaveManager.EnemySpawnData();
        boss.enemyData = FindEnemyDataByType(EnemyType.Boss);
        boss.count = 1;
        boss.spawnPattern = LevelWaveManager.SpawnPattern.Burst;
        wave.enemySpawns.Add(boss);
        
        level.waves.Add(wave);
    }
    
    EnemyData FindEnemyDataByType(EnemyType enemyType)
    {
        foreach (EnemyData enemyData in availableEnemyData)
        {
            if (enemyData != null && enemyData.enemyType == enemyType)
            {
                return enemyData;
            }
        }
        
        Debug.LogWarning("No EnemyData found for type: " + enemyType);
        return null;
    }
    
    [ContextMenu("Create Level From Template")]
    public void CreateLevelFromTemplate(int templateIndex)
    {
        if (templateIndex >= 0 && templateIndex < levelTemplates.Count)
        {
            LevelTemplate template = levelTemplates[templateIndex];
            CreateLevelFromTemplate(template);
        }
    }
    
    void CreateLevelFromTemplate(LevelTemplate template)
    {
        LevelWaveManager levelManager = FindObjectOfType<LevelWaveManager>();
        if (levelManager == null) return;
        
        LevelWaveManager.LevelData level = new LevelWaveManager.LevelData();
        level.levelNumber = template.levelNumber;
        level.levelName = template.templateName;
        level.levelDescription = template.description;
        level.healthMultiplier = template.healthMultiplier;
        level.speedMultiplier = template.speedMultiplier;
        level.damageMultiplier = template.damageMultiplier;
        level.rewardMultiplier = template.rewardMultiplier;
        
        foreach (WaveTemplate waveTemplate in template.waves)
        {
            LevelWaveManager.WaveData wave = new LevelWaveManager.WaveData();
            wave.waveName = waveTemplate.waveName;
            wave.waveNumber = waveTemplate.waveNumber;
            wave.delayBeforeWave = waveTemplate.delayBeforeWave;
            wave.maxWaveTime = waveTemplate.maxWaveTime;
            
            foreach (EnemySpawnTemplate spawnTemplate in waveTemplate.enemySpawns)
            {
                LevelWaveManager.EnemySpawnData spawnData = new LevelWaveManager.EnemySpawnData();
                spawnData.enemyData = FindEnemyDataByType(spawnTemplate.enemyType);
                spawnData.count = spawnTemplate.count;
                spawnData.spawnPattern = spawnTemplate.spawnPattern;
                spawnData.spawnInterval = spawnTemplate.spawnInterval;
                spawnData.spawnGroupSize = spawnTemplate.spawnGroupSize;
                
                if (spawnTemplate.useCustomModifiers)
                {
                    spawnData.useCustomStats = true;
                    spawnData.customHealthMultiplier = spawnTemplate.customHealthMultiplier;
                    spawnData.customSpeedMultiplier = spawnTemplate.customSpeedMultiplier;
                    spawnData.customDamageMultiplier = spawnTemplate.customDamageMultiplier;
                    spawnData.customRewardMultiplier = spawnTemplate.customRewardMultiplier;
                }
                
                wave.enemySpawns.Add(spawnData);
            }
            
            level.waves.Add(wave);
        }
        
        levelManager.levels.Add(level);
        Debug.Log("Created level from template: " + template.templateName);
    }
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 350, 10, 340, 300));
        GUILayout.Label("Level Template Creator");
        
        if (GUILayout.Button("Create Sample Levels"))
        {
            CreateSampleLevels();
        }
        
        GUILayout.Label("Available Enemy Types:");
        foreach (EnemyData enemyData in availableEnemyData)
        {
            if (enemyData != null)
            {
                GUILayout.Label("- " + enemyData.enemyName + " (" + enemyData.enemyType + ")");
            }
        }
        
        GUILayout.Label("Level Templates:");
        for (int i = 0; i < levelTemplates.Count; i++)
        {
            if (GUILayout.Button("Create " + levelTemplates[i].templateName))
            {
                CreateLevelFromTemplate(i);
            }
        }
        
        GUILayout.EndArea();
    }
} 