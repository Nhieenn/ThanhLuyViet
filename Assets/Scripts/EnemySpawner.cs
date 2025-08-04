using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public Transform endPoint;
    public float spawnInterval = 2f;
    public bool autoSpawn = true;
    
    [Header("Wave System")]
    public List<WaveData> waves = new List<WaveData>();
    public int currentWave = 0;
    public bool isWaveActive = false;
    
    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab;
    public GameObject healthBarPrefab;
    
    [Header("Pathfinding")]
    public bool usePathfinding = true;
    public PathCreator pathCreator;
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    [System.Serializable]
    public class WaveData
    {
        public string waveName = "Wave";
        public List<EnemySpawnData> enemies = new List<EnemySpawnData>();
        public float delayBeforeWave = 1f;
        public float delayBetweenEnemies = 1f;
    }
    
    [System.Serializable]
    public class EnemySpawnData
    {
        public EnemyData enemyData;
        public int count = 5;
        public float delay = 0.5f;
    }
    
    private void Start()
    {
        if (spawnPoint == null)
        {
            // Tìm StartPos prefab
            GameObject startPos = GameObject.Find("StartPos");
            if (startPos != null)
            {
                spawnPoint = startPos.transform;
            }
            else
            {
                spawnPoint = transform;
            }
        }
        
        if (endPoint == null)
        {
            // Tìm EndPos prefab
            GameObject endPos = GameObject.Find("EndPos");
            if (endPos != null)
            {
                endPoint = endPos.transform;
            }
        }
        
        // Tìm PathCreator
        if (pathCreator == null)
        {
            pathCreator = FindObjectOfType<PathCreator>();
        }
        
        if (autoSpawn)
        {
            StartCoroutine(StartWaveSystem());
        }
    }
    
    public void StartWave(int waveIndex)
    {
        if (waveIndex >= 0 && waveIndex < waves.Count)
        {
            currentWave = waveIndex;
            StartCoroutine(SpawnWave(waves[waveIndex]));
        }
    }
    
    public void StartNextWave()
    {
        if (currentWave < waves.Count - 1)
        {
            currentWave++;
            StartCoroutine(SpawnWave(waves[currentWave]));
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }
    
    private IEnumerator StartWaveSystem()
    {
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < waves.Count; i++)
        {
            currentWave = i;
            yield return StartCoroutine(SpawnWave(waves[i]));
            
            // Chờ tất cả enemy trong wave chết
            yield return StartCoroutine(WaitForWaveComplete());
        }
        
        Debug.Log("All waves completed!");
    }
    
    private IEnumerator SpawnWave(WaveData wave)
    {
        isWaveActive = true;
        Debug.Log("Starting wave: " + wave.waveName);
        
        yield return new WaitForSeconds(wave.delayBeforeWave);
        
        foreach (EnemySpawnData spawnData in wave.enemies)
        {
            for (int i = 0; i < spawnData.count; i++)
            {
                SpawnEnemy(spawnData.enemyData);
                yield return new WaitForSeconds(spawnData.delay);
            }
        }
    }
    
    private IEnumerator WaitForWaveComplete()
    {
        while (true)
        {
            // Kiểm tra xem còn enemy nào không
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            if (enemies.Length == 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        
        isWaveActive = false;
        Debug.Log("Wave " + currentWave + " completed!");
    }
    
    public void SpawnEnemy(EnemyData enemyData)
    {
        SpawnEnemyWithModifiers(enemyData, 1f, 1f, 1f, 1f);
    }
    
    public GameObject SpawnEnemyWithModifiers(EnemyData enemyData, float healthMultiplier, float speedMultiplier, float damageMultiplier, float rewardMultiplier)
    {
        if (enemyData == null)
        {
            Debug.LogError("EnemyData is null!");
            return null;
        }
        
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is null!");
            return null;
        }
        
        // Tạo enemy GameObject
        GameObject enemyObj = new GameObject(enemyData.enemyName);
        enemyObj.transform.position = spawnPoint.position;
        
        // Thêm các component cần thiết
        SpriteRenderer spriteRenderer = enemyObj.AddComponent<SpriteRenderer>();
        Rigidbody2D rb = enemyObj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        
        // Thêm Enemy script
        Enemy enemy = enemyObj.AddComponent<Enemy>();
        enemy.enemyData = enemyData;
        enemy.healthBarPrefab = healthBarPrefab;
        
        // Áp dụng modifiers
        ApplyEnemyModifiers(enemy, healthMultiplier, speedMultiplier, damageMultiplier, rewardMultiplier);
        
        // Thêm EnemyPathfinding nếu sử dụng pathfinding
        if (usePathfinding)
        {
            EnemyPathfinding pathfinding = enemyObj.AddComponent<EnemyPathfinding>();
            
            // Nếu có PathCreator, sử dụng waypoints từ đó
            if (pathCreator != null)
            {
                pathfinding.SetWaypoints(pathCreator.GetWaypoints());
            }
        }
        else
        {
            // Set target đơn giản (end point)
            if (endPoint != null)
            {
                enemy.SetTarget(endPoint);
            }
            
            // Tạo waypoints đơn giản
            if (endPoint != null)
            {
                Vector3[] waypoints = new Vector3[2];
                waypoints[0] = spawnPoint.position;
                waypoints[1] = endPoint.position;
                enemy.SetWaypoints(waypoints);
            }
        }
        
        Debug.Log("Spawned enemy: " + enemyData.enemyName + " with modifiers - Health: " + healthMultiplier + "x, Speed: " + speedMultiplier + "x");
        return enemyObj;
    }
    
    private void ApplyEnemyModifiers(Enemy enemy, float healthMultiplier, float speedMultiplier, float damageMultiplier, float rewardMultiplier)
    {
        // Lưu modifiers vào enemy để sử dụng sau này
        enemy.healthMultiplier = healthMultiplier;
        enemy.speedMultiplier = speedMultiplier;
        enemy.damageMultiplier = damageMultiplier;
        enemy.rewardMultiplier = rewardMultiplier;
    }
    
    // Test functions
    [ContextMenu("Spawn Test Enemy")]
    public void SpawnTestEnemy()
    {
        if (waves.Count > 0 && waves[0].enemies.Count > 0)
        {
            SpawnEnemy(waves[0].enemies[0].enemyData);
        }
    }
    
    [ContextMenu("Start Test Wave")]
    public void StartTestWave()
    {
        if (waves.Count > 0)
        {
            StartWave(0);
        }
    }
    
    private void OnGUI()
    {
        if (!showDebugInfo) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("Enemy Spawner Debug");
        GUILayout.Label("Current Wave: " + currentWave);
        GUILayout.Label("Wave Active: " + isWaveActive);
        GUILayout.Label("Use Pathfinding: " + usePathfinding);
        
        if (GUILayout.Button("Spawn Test Enemy"))
        {
            SpawnTestEnemy();
        }
        
        if (GUILayout.Button("Start Test Wave"))
        {
            StartTestWave();
        }
        
        if (GUILayout.Button("Start Next Wave"))
        {
            StartNextWave();
        }
        
        GUILayout.EndArea();
    }
} 