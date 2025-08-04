using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelWaveManager : MonoBehaviour
{
    [Header("Level Management")]
    public int currentLevel = 1;
    public List<LevelData> levels = new List<LevelData>();
    
    [Header("Current Level")]
    public LevelData currentLevelData;
    public int currentWaveIndex = 0;
    public bool isWaveActive = false;
    
    [Header("Settings")]
    public bool autoStartLevel = true;
    public bool pauseBetweenWaves = true;
    public float pauseDuration = 3f;
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    [System.Serializable]
    public class LevelData
    {
        [Header("Level Info")]
        public int levelNumber = 1;
        public string levelName = "Level";
        public string levelDescription = "Level description";
        
        [Header("Level Settings")]
        public int totalWaves = 5;
        public float levelTimeLimit = 300f; // 5 phút
        public int playerStartingLives = 20;
        public int playerStartingMoney = 500;
        
        [Header("Enemy Scaling")]
        public float healthMultiplier = 1f;
        public float speedMultiplier = 1f;
        public float damageMultiplier = 1f;
        public float rewardMultiplier = 1f;
        
        [Header("Waves")]
        public List<WaveData> waves = new List<WaveData>();
    }
    
    [System.Serializable]
    public class WaveData
    {
        [Header("Wave Info")]
        public string waveName = "Wave";
        public int waveNumber = 1;
        public float delayBeforeWave = 2f;
        
        [Header("Enemy Spawns")]
        public List<EnemySpawnData> enemySpawns = new List<EnemySpawnData>();
        
        [Header("Wave Completion")]
        public bool waitForAllEnemiesDead = true;
        public float maxWaveTime = 60f;
    }
    
    [System.Serializable]
    public class EnemySpawnData
    {
        [Header("Enemy")]
        public EnemyData enemyData;
        public int count = 5;
        
        [Header("Spawn Pattern")]
        public SpawnPattern spawnPattern = SpawnPattern.Sequential;
        public float spawnInterval = 0.5f;
        public int spawnGroupSize = 3;
        
        [Header("Enemy Modifiers")]
        public bool useCustomStats = false;
        public float customHealthMultiplier = 1f;
        public float customSpeedMultiplier = 1f;
        public float customDamageMultiplier = 1f;
        public float customRewardMultiplier = 1f;
        
        [Header("Timing")]
        public float delayBeforeThisEnemy = 0f;
        public float delayAfterThisEnemy = 0f;
    }
    
    public enum SpawnPattern
    {
        Sequential,    // Spawn từng enemy một
        Group,         // Spawn theo nhóm
        Burst,         // Spawn tất cả cùng lúc
        Random,        // Spawn ngẫu nhiên
        Alternating    // Spawn xen kẽ với enemy khác
    }
    
    private EnemySpawner spawner;
    private Coroutine currentWaveCoroutine;
    private float levelStartTime;
    
    void Start()
    {
        // Tìm EnemySpawner
        spawner = FindObjectOfType<EnemySpawner>();
        if (spawner == null)
        {
            GameObject spawnerObj = new GameObject("EnemySpawner");
            spawner = spawnerObj.AddComponent<EnemySpawner>();
        }
        
        if (autoStartLevel)
        {
            StartLevel(currentLevel);
        }
    }
    
    public void StartLevel(int levelNumber)
    {
        currentLevel = levelNumber;
        currentLevelData = GetLevelData(levelNumber);
        
        if (currentLevelData == null)
        {
            Debug.LogError("Level " + levelNumber + " not found!");
            return;
        }
        
        levelStartTime = Time.time;
        currentWaveIndex = 0;
        
        Debug.Log("Starting Level " + currentLevelData.levelName);
        StartCoroutine(StartLevelWaves());
    }
    
    public void StartNextLevel()
    {
        StartLevel(currentLevel + 1);
    }
    
    public void RestartCurrentLevel()
    {
        StartLevel(currentLevel);
    }
    
    private IEnumerator StartLevelWaves()
    {
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < currentLevelData.waves.Count; i++)
        {
            currentWaveIndex = i;
            yield return StartCoroutine(ExecuteWave(currentLevelData.waves[i]));
            
            if (pauseBetweenWaves && i < currentLevelData.waves.Count - 1)
            {
                Debug.Log("Pausing between waves for " + pauseDuration + " seconds...");
                yield return new WaitForSeconds(pauseDuration);
            }
        }
        
        Debug.Log("Level " + currentLevelData.levelName + " completed!");
    }
    
    private IEnumerator ExecuteWave(WaveData wave)
    {
        isWaveActive = true;
        Debug.Log("Starting wave: " + wave.waveName + " (Wave " + wave.waveNumber + ")");
        
        yield return new WaitForSeconds(wave.delayBeforeWave);
        
        // Spawn tất cả enemy types trong wave
        foreach (EnemySpawnData spawnData in wave.enemySpawns)
        {
            if (spawnData.enemyData != null)
            {
                yield return StartCoroutine(SpawnEnemyType(spawnData, wave));
            }
        }
        
        // Chờ wave hoàn thành
        if (wave.waitForAllEnemiesDead)
        {
            yield return StartCoroutine(WaitForWaveComplete(wave.maxWaveTime));
        }
        
        isWaveActive = false;
        Debug.Log("Wave " + wave.waveNumber + " completed!");
    }
    
    private IEnumerator SpawnEnemyType(EnemySpawnData spawnData, WaveData wave)
    {
        yield return new WaitForSeconds(spawnData.delayBeforeThisEnemy);
        
        switch (spawnData.spawnPattern)
        {
            case SpawnPattern.Sequential:
                yield return StartCoroutine(SpawnSequential(spawnData, wave));
                break;
                
            case SpawnPattern.Group:
                yield return StartCoroutine(SpawnInGroups(spawnData, wave));
                break;
                
            case SpawnPattern.Burst:
                yield return StartCoroutine(SpawnBurst(spawnData));
                break;
                
            case SpawnPattern.Random:
                yield return StartCoroutine(SpawnRandom(spawnData, wave));
                break;
                
            case SpawnPattern.Alternating:
                yield return StartCoroutine(SpawnAlternating(spawnData, wave));
                break;
        }
        
        yield return new WaitForSeconds(spawnData.delayAfterThisEnemy);
    }
    
    private IEnumerator SpawnSequential(EnemySpawnData spawnData, WaveData wave)
    {
        for (int i = 0; i < spawnData.count; i++)
        {
            SpawnEnemyWithModifiers(spawnData);
            yield return new WaitForSeconds(spawnData.spawnInterval);
        }
    }
    
    private IEnumerator SpawnInGroups(EnemySpawnData spawnData, WaveData wave)
    {
        int groups = Mathf.CeilToInt((float)spawnData.count / spawnData.spawnGroupSize);
        
        for (int group = 0; group < groups; group++)
        {
            int enemiesInThisGroup = Mathf.Min(spawnData.spawnGroupSize, 
                spawnData.count - group * spawnData.spawnGroupSize);
            
            // Spawn nhóm enemy
            for (int i = 0; i < enemiesInThisGroup; i++)
            {
                SpawnEnemyWithModifiers(spawnData);
            }
            
            // Chờ trước khi spawn nhóm tiếp theo
            if (group < groups - 1)
            {
                yield return new WaitForSeconds(2f);
            }
        }
    }
    
    private IEnumerator SpawnBurst(EnemySpawnData spawnData)
    {
        // Spawn tất cả cùng lúc
        for (int i = 0; i < spawnData.count; i++)
        {
            SpawnEnemyWithModifiers(spawnData);
        }
        yield return null;
    }
    
    private IEnumerator SpawnRandom(EnemySpawnData spawnData, WaveData wave)
    {
        for (int i = 0; i < spawnData.count; i++)
        {
            SpawnEnemyWithModifiers(spawnData);
            
            // Delay ngẫu nhiên
            float randomDelay = Random.Range(0.1f, spawnData.spawnInterval * 2);
            yield return new WaitForSeconds(randomDelay);
        }
    }
    
    private IEnumerator SpawnAlternating(EnemySpawnData spawnData, WaveData wave)
    {
        // Tìm enemy khác để xen kẽ
        EnemySpawnData alternateEnemy = null;
        foreach (EnemySpawnData otherSpawn in wave.enemySpawns)
        {
            if (otherSpawn != spawnData && otherSpawn.enemyData != null)
            {
                alternateEnemy = otherSpawn;
                break;
            }
        }
        
        for (int i = 0; i < spawnData.count; i++)
        {
            SpawnEnemyWithModifiers(spawnData);
            yield return new WaitForSeconds(spawnData.spawnInterval);
            
            // Spawn enemy xen kẽ
            if (alternateEnemy != null && i < spawnData.count - 1)
            {
                SpawnEnemyWithModifiers(alternateEnemy);
                yield return new WaitForSeconds(alternateEnemy.spawnInterval);
            }
        }
    }
    
    private void SpawnEnemyWithModifiers(EnemySpawnData spawnData)
    {
        if (spawner != null)
        {
            // Tạo enemy với modifiers
            GameObject enemyObj = spawner.SpawnEnemyWithModifiers(
                spawnData.enemyData,
                GetTotalHealthMultiplier(spawnData),
                GetTotalSpeedMultiplier(spawnData),
                GetTotalDamageMultiplier(spawnData),
                GetTotalRewardMultiplier(spawnData)
            );
        }
    }
    
    private float GetTotalHealthMultiplier(EnemySpawnData spawnData)
    {
        float multiplier = currentLevelData.healthMultiplier;
        if (spawnData.useCustomStats)
        {
            multiplier *= spawnData.customHealthMultiplier;
        }
        return multiplier;
    }
    
    private float GetTotalSpeedMultiplier(EnemySpawnData spawnData)
    {
        float multiplier = currentLevelData.speedMultiplier;
        if (spawnData.useCustomStats)
        {
            multiplier *= spawnData.customSpeedMultiplier;
        }
        return multiplier;
    }
    
    private float GetTotalDamageMultiplier(EnemySpawnData spawnData)
    {
        float multiplier = currentLevelData.damageMultiplier;
        if (spawnData.useCustomStats)
        {
            multiplier *= spawnData.customDamageMultiplier;
        }
        return multiplier;
    }
    
    private float GetTotalRewardMultiplier(EnemySpawnData spawnData)
    {
        float multiplier = currentLevelData.rewardMultiplier;
        if (spawnData.useCustomStats)
        {
            multiplier *= spawnData.customRewardMultiplier;
        }
        return multiplier;
    }
    
    private IEnumerator WaitForWaveComplete(float maxTime)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < maxTime)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            if (enemies.Length == 0)
            {
                break;
            }
            
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        
        if (elapsedTime >= maxTime)
        {
            Debug.Log("Wave time limit reached!");
        }
    }
    
    public LevelData GetLevelData(int levelNumber)
    {
        foreach (LevelData level in levels)
        {
            if (level.levelNumber == levelNumber)
            {
                return level;
            }
        }
        return null;
    }
    
    public void StartWave(int waveIndex)
    {
        if (currentLevelData != null && waveIndex >= 0 && waveIndex < currentLevelData.waves.Count)
        {
            currentWaveIndex = waveIndex;
            if (currentWaveCoroutine != null)
            {
                StopCoroutine(currentWaveCoroutine);
            }
            currentWaveCoroutine = StartCoroutine(ExecuteWave(currentLevelData.waves[waveIndex]));
        }
    }
    
    public void StartNextWave()
    {
        if (currentLevelData != null && currentWaveIndex < currentLevelData.waves.Count - 1)
        {
            StartWave(currentWaveIndex + 1);
        }
    }
    
    // Test functions
    [ContextMenu("Start Current Wave")]
    public void StartCurrentWave()
    {
        StartWave(currentWaveIndex);
    }
    
    [ContextMenu("Start Next Wave")]
    public void StartNextWaveTest()
    {
        StartNextWave();
    }
    
    [ContextMenu("Start Next Level")]
    public void StartNextLevelTest()
    {
        StartNextLevel();
    }
    
    private void OnGUI()
    {
        if (!showDebugInfo) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 400, 400));
        GUILayout.Label("Level Wave Manager Debug");
        GUILayout.Label("Current Level: " + currentLevel);
        GUILayout.Label("Current Wave: " + currentWaveIndex);
        GUILayout.Label("Wave Active: " + isWaveActive);
        
        if (currentLevelData != null)
        {
            GUILayout.Label("Level Name: " + currentLevelData.levelName);
            GUILayout.Label("Total Waves: " + currentLevelData.waves.Count);
            GUILayout.Label("Level Time: " + (Time.time - levelStartTime).ToString("F1") + "s");
        }
        
        if (GUILayout.Button("Start Current Wave"))
        {
            StartCurrentWave();
        }
        
        if (GUILayout.Button("Start Next Wave"))
        {
            StartNextWaveTest();
        }
        
        if (GUILayout.Button("Start Next Level"))
        {
            StartNextLevelTest();
        }
        
        if (GUILayout.Button("Restart Level"))
        {
            RestartCurrentLevel();
        }
        
        GUILayout.Label("Level List:");
        for (int i = 0; i < levels.Count; i++)
        {
            string status = (levels[i].levelNumber == currentLevel) ? " [CURRENT]" : "";
            GUILayout.Label(levels[i].levelNumber + ". " + levels[i].levelName + status);
        }
        
        GUILayout.EndArea();
    }
} 