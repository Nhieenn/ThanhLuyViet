//using UnityEngine;

//public class LevelSceneSetup : MonoBehaviour
//{
//    [Header("Auto Setup")]
//    public bool setupOnStart = true;
    
//    [Header("Level Path Manager")]
//    public LevelPathManager levelPathManager;
    
//    [Header("Wave System")]
//    public bool setupWaveSystem = true;
//    public LevelWaveManager waveManager;
    
//    void Start()
//    {
//        if (setupOnStart)
//        {
//            SetupLevelScene();
//        }
//    }
    
//    [ContextMenu("Setup Level Scene")]
//    public void SetupLevelScene()
//    {
//        Debug.Log("Setting up Level Scene...");
        
//        // Lấy level hiện tại từ PlayerPrefs
//        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
//        Debug.Log($"Setting up Level {currentLevel}");
        
//        // Setup Path System
//        SetupPathSystem(currentLevel);
        
//        // Setup Wave System
//        if (setupWaveSystem)
//        {
//            SetupWaveSystem(currentLevel);
//        }
        
//        Debug.Log($"Level {currentLevel} scene setup completed!");
//    }
    
//    void SetupPathSystem(int level)
//    {
//        // Tạo LevelPathManager nếu chưa có
//        if (levelPathManager == null)
//        {
//            GameObject pathManagerObj = new GameObject("LevelPathManager");
//            levelPathManager = pathManagerObj.AddComponent<LevelPathManager>();
//        }
        
//        // Set level và load path
//        levelPathManager.SetLevel(level);
        
//        Debug.Log($"Path system setup for Level {level}");
//    }
    
//    void SetupWaveSystem(int level)
//    {
//        // Tạo LevelWaveManager nếu chưa có
//        if (waveManager == null)
//        {
//            GameObject waveManagerObj = new GameObject("LevelWaveManager");
//            waveManager = waveManagerObj.AddComponent<LevelWaveManager>();
//        }
        
//        // Tạo WaveCreator nếu chưa có
//        WaveCreator waveCreator = FindObjectOfType<WaveCreator>();
//        if (waveCreator == null)
//        {
//            GameObject waveCreatorObj = new GameObject("WaveCreator");
//            waveCreator = waveCreatorObj.AddComponent<WaveCreator>();
//        }
        
//        // Cấu hình WaveCreator cho level hiện tại
//        waveCreator.numberOfLevels = 1; // Chỉ tạo waves cho level hiện tại
//        waveCreator.wavesPerLevel = 5;
        
//        // Tìm enemy references
//        FindEnemyReferences(waveCreator);
        
//        // Tạo waves cho level hiện tại
//        waveCreator.CreateAllWaves();
        
//        // Start level
//        if (waveManager.levels.Count > 0)
//        {
//            waveManager.StartLevel(1); // Level 1 trong scene này
//        }
        
//        Debug.Log($"Wave system setup for Level {level}");
//    }
    
//    void FindEnemyReferences(WaveCreator waveCreator)
//    {
//        // Tìm enemy data files
//        EnemyData[] allEnemies = Resources.FindObjectsOfTypeAll<EnemyData>();
        
//        foreach (EnemyData enemy in allEnemies)
//        {
//            switch (enemy.enemyType)
//            {
//                case EnemyType.Infantry:
//                    if (enemy.enemyName.Contains("Infantry") && !enemy.enemyName.Contains("Elite"))
//                    {
//                        waveCreator.infantryEnemy = enemy;
//                    }
//                    break;
                    
//                case EnemyType.Tank:
//                    if (enemy.enemyName.Contains("Tank") && !enemy.enemyName.Contains("Heavy"))
//                    {
//                        waveCreator.tankEnemy = enemy;
//                    }
//                    break;
                    
//                case EnemyType.Aircraft:
//                    if (enemy.enemyName.Contains("Aircraft") || enemy.enemyName.Contains("Jet"))
//                    {
//                        waveCreator.aircraftEnemy = enemy;
//                    }
//                    break;
                    
//                case EnemyType.Boss:
//                    if (enemy.enemyName.Contains("Boss") && !enemy.enemyName.Contains("Mega"))
//                    {
//                        waveCreator.bossEnemy = enemy;
//                    }
//                    break;
//            }
//        }
        
//        Debug.Log($"Found enemy references: Infantry={waveCreator.infantryEnemy != null}, Tank={waveCreator.tankEnemy != null}, Aircraft={waveCreator.aircraftEnemy != null}, Boss={waveCreator.bossEnemy != null}");
//    }
    
//    [ContextMenu("Test Level Setup")]
//    public void TestLevelSetup()
//    {
//        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
//        Debug.Log($"Testing Level {currentLevel} setup...");
        
//        // Test Path System
//        if (levelPathManager != null)
//        {
//            Debug.Log($"✓ LevelPathManager found for Level {currentLevel}");
//            Debug.Log($"✓ Path Style: {levelPathManager.GetCurrentPathStyle()}");
//            Debug.Log($"✓ Start Position: {levelPathManager.GetCurrentStartPosition()}");
//            Debug.Log($"✓ End Position: {levelPathManager.GetCurrentEndPosition()}");
//        }
//        else
//        {
//            Debug.LogError("✗ LevelPathManager not found");
//        }
        
//        // Test Wave System
//        if (waveManager != null)
//        {
//            Debug.Log($"✓ LevelWaveManager found with {waveManager.levels.Count} levels");
//            if (waveManager.levels.Count > 0)
//            {
//                Debug.Log($"✓ First level has {waveManager.levels[0].waves.Count} waves");
//            }
//        }
//        else
//        {
//            Debug.LogError("✗ LevelWaveManager not found");
//        }
        
//        // Test Path Manager
//        PathManager pathManager = FindObjectOfType<PathManager>();
//        if (pathManager != null)
//        {
//            Debug.Log($"✓ PathManager found with {pathManager.GetWaypoints().Count} waypoints");
//        }
//        else
//        {
//            Debug.LogError("✗ PathManager not found");
//        }
        
//        Debug.Log("Level setup test completed!");
//    }
    
//    [ContextMenu("Start Current Level")]
//    public void StartCurrentLevel()
//    {
//        if (waveManager != null && waveManager.levels.Count > 0)
//        {
//            waveManager.StartLevel(1);
//            Debug.Log("Started current level!");
//        }
//        else
//        {
//            Debug.LogError("No levels available to start!");
//        }
//    }
    
//    void OnGUI()
//    {
//        if (!Application.isPlaying) return;
        
//        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        
//        GUILayout.BeginArea(new Rect(10, 320, 250, 200));
//        GUILayout.Label($"Level {currentLevel} Scene Setup");
        
//        if (GUILayout.Button("Setup Level Scene"))
//        {
//            SetupLevelScene();
//        }
        
//        if (GUILayout.Button("Test Setup"))
//        {
//            TestLevelSetup();
//        }
        
//        if (GUILayout.Button("Start Level"))
//        {
//            StartCurrentLevel();
//        }
        
//        // Status info
//        GUILayout.Label("Status:");
        
//        LevelPathManager lpm = FindObjectOfType<LevelPathManager>();
//        GUILayout.Label($"Path Manager: {(lpm != null ? "✓" : "✗")}");
        
//        LevelWaveManager lwm = FindObjectOfType<LevelWaveManager>();
//        GUILayout.Label($"Wave Manager: {(lwm != null ? "✓" : "✗")}");
        
//        PathManager pm = FindObjectOfType<PathManager>();
//        GUILayout.Label($"Path System: {(pm != null ? "✓" : "✗")}");
        
//        if (pm != null)
//        {
//            GUILayout.Label($"Waypoints: {pm.GetWaypoints().Count}");
//        }
        
//        GUILayout.EndArea();
//    }
//} 