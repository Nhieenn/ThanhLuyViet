using UnityEngine;

public class GameSystemSetup : MonoBehaviour
{
    [Header("System Setup")]
    public bool setupOnStart = true;
    public bool setupPathSystem = true;
    public bool setupWaveSystem = true;
    public bool setupEnemyData = true;
    
    [Header("Path System Settings")]
    public PathManager.PathStyle defaultPathStyle = PathManager.PathStyle.Curved;
    public int defaultWaypointCount = 8;
    public Vector3 startPosition = new Vector3(-8, 0, 0);
    public Vector3 endPosition = new Vector3(8, 0, 0);
    
    [Header("Wave System Settings")]
    public int numberOfLevels = 10;
    public int wavesPerLevel = 5;
    
    [Header("Enemy References")]
    public EnemyData infantryEnemy;
    public EnemyData tankEnemy;
    public EnemyData aircraftEnemy;
    public EnemyData bossEnemy;
    
    private PathSystemSetup pathSetup;
    private WaveCreator waveCreator;
    private EnemyDataCreator enemyCreator;
    private LevelWaveManager waveManager;
    
    void Start()
    {
        if (setupOnStart)
        {
            SetupGameSystem();
        }
    }
    
    [ContextMenu("Setup Game System")]
    public void SetupGameSystem()
    {
        Debug.Log("Setting up complete Game System...");
        
        // Setup Path System
        if (setupPathSystem)
        {
            SetupPathSystem();
        }
        
        // Setup Enemy Data
        if (setupEnemyData)
        {
            SetupEnemyData();
        }
        
        // Setup Wave System
        if (setupWaveSystem)
        {
            SetupWaveSystem();
        }
        
        Debug.Log("Game System setup completed!");
    }
    
    void SetupPathSystem()
    {
        // Tạo PathSystemSetup nếu chưa có
        pathSetup = FindObjectOfType<PathSystemSetup>();
        if (pathSetup == null)
        {
            GameObject pathSetupObj = new GameObject("PathSystemSetup");
            pathSetup = pathSetupObj.AddComponent<PathSystemSetup>();
        }
        
        // Cấu hình PathSystemSetup
        pathSetup.startPosition = startPosition;
        pathSetup.endPosition = endPosition;
        pathSetup.defaultPathStyle = defaultPathStyle;
        pathSetup.defaultWaypointCount = defaultWaypointCount;
        
        // Setup path system
        pathSetup.SetupPathSystem();
        
        Debug.Log("Path System setup completed!");
    }
    
    void SetupEnemyData()
    {
        // Tạo EnemyDataCreator nếu chưa có
        enemyCreator = FindObjectOfType<EnemyDataCreator>();
        if (enemyCreator == null)
        {
            GameObject enemyCreatorObj = new GameObject("EnemyDataCreator");
            enemyCreator = enemyCreatorObj.AddComponent<EnemyDataCreator>();
        }
        
        // Tạo enemy data
        enemyCreator.CreateEnemyTemplates();
        enemyCreator.CreateAllEnemies();
        
        // Tìm và gán enemy references
        FindEnemyReferences();
        
        Debug.Log("Enemy Data setup completed!");
    }
    
    void FindEnemyReferences()
    {
        // Tìm enemy data files
        EnemyData[] allEnemies = Resources.FindObjectsOfTypeAll<EnemyData>();
        
        foreach (EnemyData enemy in allEnemies)
        {
            switch (enemy.enemyType)
            {
                case EnemyType.Infantry:
                    if (enemy.enemyName.Contains("Infantry") && !enemy.enemyName.Contains("Elite"))
                    {
                        infantryEnemy = enemy;
                    }
                    break;
                    
                case EnemyType.Tank:
                    if (enemy.enemyName.Contains("Tank") && !enemy.enemyName.Contains("Heavy"))
                    {
                        tankEnemy = enemy;
                    }
                    break;
                    
                case EnemyType.Aircraft:
                    if (enemy.enemyName.Contains("Aircraft") || enemy.enemyName.Contains("Jet"))
                    {
                        aircraftEnemy = enemy;
                    }
                    break;
                    
                case EnemyType.Boss:
                    if (enemy.enemyName.Contains("Boss") && !enemy.enemyName.Contains("Mega"))
                    {
                        bossEnemy = enemy;
                    }
                    break;
            }
        }
        
        Debug.Log($"Found enemy references: Infantry={infantryEnemy != null}, Tank={tankEnemy != null}, Aircraft={aircraftEnemy != null}, Boss={bossEnemy != null}");
    }
    
    void SetupWaveSystem()
    {
        // Tạo LevelWaveManager nếu chưa có
        waveManager = FindObjectOfType<LevelWaveManager>();
        if (waveManager == null)
        {
            GameObject waveManagerObj = new GameObject("LevelWaveManager");
            waveManager = waveManagerObj.AddComponent<LevelWaveManager>();
        }
        
        // Tạo WaveCreator nếu chưa có
        waveCreator = FindObjectOfType<WaveCreator>();
        if (waveCreator == null)
        {
            GameObject waveCreatorObj = new GameObject("WaveCreator");
            waveCreator = waveCreatorObj.AddComponent<WaveCreator>();
        }
        
        // Cấu hình WaveCreator
        waveCreator.numberOfLevels = numberOfLevels;
        waveCreator.wavesPerLevel = wavesPerLevel;
        waveCreator.infantryEnemy = infantryEnemy;
        waveCreator.tankEnemy = tankEnemy;
        waveCreator.aircraftEnemy = aircraftEnemy;
        waveCreator.bossEnemy = bossEnemy;
        
        // Tạo waves
        waveCreator.CreateAllWaves();
        
        Debug.Log("Wave System setup completed!");
    }
    
    [ContextMenu("Test Game System")]
    public void TestGameSystem()
    {
        Debug.Log("Testing Game System...");
        
        // Test Path System
        PathManager pathManager = FindObjectOfType<PathManager>();
        if (pathManager != null)
        {
            Debug.Log("✓ Path Manager found");
            if (pathManager.GetWaypoints().Count > 0)
            {
                Debug.Log($"✓ Path has {pathManager.GetWaypoints().Count} waypoints");
            }
        }
        else
        {
            Debug.LogError("✗ Path Manager not found");
        }
        
        // Test Enemy Data
        EnemyData[] enemies = Resources.FindObjectsOfTypeAll<EnemyData>();
        Debug.Log($"✓ Found {enemies.Length} enemy data files");
        
        // Test Wave System
        if (waveManager != null)
        {
            Debug.Log($"✓ Wave Manager found with {waveManager.levels.Count} levels");
            if (waveManager.levels.Count > 0)
            {
                Debug.Log($"✓ First level has {waveManager.levels[0].waves.Count} waves");
            }
        }
        else
        {
            Debug.LogError("✗ Wave Manager not found");
        }
        
        Debug.Log("Game System test completed!");
    }
    
    [ContextMenu("Clear Game System")]
    public void ClearGameSystem()
    {
        Debug.Log("Clearing Game System...");
        
        // Clear Path System
        PathManager pathManager = FindObjectOfType<PathManager>();
        if (pathManager != null)
        {
            DestroyImmediate(pathManager.gameObject);
        }
        
        // Clear Wave System
        if (waveManager != null)
        {
            waveManager.levels.Clear();
        }
        
        // Clear Start/End Points
        GameObject startPos = GameObject.Find("StartPos");
        if (startPos != null)
        {
            DestroyImmediate(startPos);
        }
        
        GameObject endPos = GameObject.Find("EndPos");
        if (endPos != null)
        {
            DestroyImmediate(endPos);
        }
        
        // Clear Waypoints
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in waypoints)
        {
            DestroyImmediate(waypoint);
        }
        
        Debug.Log("Game System cleared!");
    }
    
    [ContextMenu("Start Test Level")]
    public void StartTestLevel()
    {
        if (waveManager != null && waveManager.levels.Count > 0)
        {
            waveManager.StartLevel(1);
            Debug.Log("Started test level 1!");
        }
        else
        {
            Debug.LogError("No levels available to start!");
        }
    }
    
    void OnGUI()
    {
        if (!Application.isPlaying) return;
        
        GUILayout.BeginArea(new Rect(10, 430, 300, 200));
        GUILayout.Label("Game System Setup");
        
        if (GUILayout.Button("Setup Game System"))
        {
            SetupGameSystem();
        }
        
        if (GUILayout.Button("Test System"))
        {
            TestGameSystem();
        }
        
        if (GUILayout.Button("Clear System"))
        {
            ClearGameSystem();
        }
        
        if (GUILayout.Button("Start Test Level"))
        {
            StartTestLevel();
        }
        
        // Status info
        GUILayout.Label("System Status:");
        
        PathManager pathManager = FindObjectOfType<PathManager>();
        GUILayout.Label($"Path Manager: {(pathManager != null ? "✓" : "✗")}");
        
        LevelWaveManager waveMgr = FindObjectOfType<LevelWaveManager>();
        GUILayout.Label($"Wave Manager: {(waveMgr != null ? "✓" : "✗")}");
        
        if (waveMgr != null)
        {
            GUILayout.Label($"Levels: {waveMgr.levels.Count}");
        }
        
        EnemyData[] enemies = Resources.FindObjectsOfTypeAll<EnemyData>();
        GUILayout.Label($"Enemy Data: {enemies.Length}");
        
        GUILayout.EndArea();
    }
} 