//using UnityEngine;

//public class LevelAutoSetup : MonoBehaviour
//{
//    [Header("Auto Setup")]
//    public bool setupOnStart = true;
    
//    [Header("Path System")]
//    public bool setupPathSystem = true;
//    public bool useExistingPathObjects = true; // Sử dụng GameObject có sẵn trong scene
//    public PathManager.PathStyle[] levelPathStyles = new PathManager.PathStyle[10];
//    public Vector3[] levelStartPositions = new Vector3[10];
//    public Vector3[] levelEndPositions = new Vector3[10];
//    public int[] levelWaypointCounts = new int[10];
    
//    [Header("Wave System")]
//    public bool setupWaveSystem = true;
//    public int wavesPerLevel = 5;
    
//    [Header("Enemy References")]
//    public EnemyData infantryEnemy;
//    public EnemyData tankEnemy;
//    public EnemyData aircraftEnemy;
//    public EnemyData bossEnemy;
    
//    private PathManager pathManager;
//    private PathFromGameObjects pathFromObjects;
//    private LevelWaveManager waveManager;
//    private WaveCreator waveCreator;
    
//    void Start()
//    {
//        if (setupOnStart)
//        {
//            SetupLevel();
//        }
//    }
    
//    [ContextMenu("Setup Level")]
//    public void SetupLevel()
//    {
//        // Lấy level number từ tên scene
//        int levelNumber = GetLevelNumberFromScene();
//        Debug.Log($"Setting up Level {levelNumber}");
        
//        // Khởi tạo cấu hình mặc định
//        InitializeDefaultConfigurations();
        
//        // Setup Path System
//        if (setupPathSystem)
//        {
//            if (useExistingPathObjects)
//            {
//                SetupPathFromExistingObjects();
//            }
//            else
//            {
//                SetupPathSystem(levelNumber);
//            }
//        }
        
//        // Setup Wave System
//        if (setupWaveSystem)
//        {
//            SetupWaveSystem(levelNumber);
//        }
        
//        Debug.Log($"Level {levelNumber} setup completed!");
//    }
    
//    void SetupPathFromExistingObjects()
//    {
//        // Tạo PathFromGameObjects nếu chưa có
//        if (pathFromObjects == null)
//        {
//            GameObject pathFromObjectsObj = new GameObject("PathFromGameObjects");
//            pathFromObjects = pathFromObjectsObj.AddComponent<PathFromGameObjects>();
//        }
        
//        // Cấu hình PathFromGameObjects
//        pathFromObjects.autoDetectOnStart = false; // Tắt auto detect để tự control
//        pathFromObjects.createPathFromObjects = true;
//        pathFromObjects.showPathLine = true;
        
//        // Detect và tạo path từ GameObject có sẵn
//        pathFromObjects.DetectPathObjects();
//        pathFromObjects.CreatePathFromObjects();
        
//        // Lấy PathManager từ PathFromGameObjects
//        pathManager = pathFromObjects.pathManager;
        
//        Debug.Log("Path system setup from existing GameObjects");
//    }
    
//    int GetLevelNumberFromScene()
//    {
//        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
//        // Tìm số level từ tên scene (Lv1, Lv2, etc.)
//        if (sceneName.StartsWith("Lv") || sceneName.StartsWith("LV"))
//        {
//            string numberPart = sceneName.Substring(2); // Bỏ "Lv" hoặc "LV"
//            if (int.TryParse(numberPart, out int level))
//            {
//                return level;
//            }
//        }
        
//        // Fallback: tìm trong tên scene
//        for (int i = 1; i <= 10; i++)
//        {
//            if (sceneName.Contains(i.ToString()))
//            {
//                return i;
//            }
//        }
        
//        Debug.LogWarning($"Could not determine level number from scene name: {sceneName}. Using level 1.");
//        return 1;
//    }
    
//    void InitializeDefaultConfigurations()
//    {
//        // Path styles cho 10 levels
//        levelPathStyles[0] = PathManager.PathStyle.Straight;      // Level 1
//        levelPathStyles[1] = PathManager.PathStyle.Curved;        // Level 2
//        levelPathStyles[2] = PathManager.PathStyle.ZigZag;        // Level 3
//        levelPathStyles[3] = PathManager.PathStyle.Curved;        // Level 4
//        levelPathStyles[4] = PathManager.PathStyle.Spiral;        // Level 5
//        levelPathStyles[5] = PathManager.PathStyle.ZigZag;        // Level 6
//        levelPathStyles[6] = PathManager.PathStyle.Curved;        // Level 7
//        levelPathStyles[7] = PathManager.PathStyle.Random;        // Level 8
//        levelPathStyles[8] = PathManager.PathStyle.Spiral;        // Level 9
//        levelPathStyles[9] = PathManager.PathStyle.Random;        // Level 10
        
//        // Start/End positions cho 10 levels
//        // Level 1-5: Từ trái sang phải
//        for (int i = 0; i < 5; i++)
//        {
//            levelStartPositions[i] = new Vector3(-8, 0, 0);
//            levelEndPositions[i] = new Vector3(8, 0, 0);
//        }
        
//        // Level 6-8: Từ trên xuống dưới
//        for (int i = 5; i < 8; i++)
//        {
//            levelStartPositions[i] = new Vector3(0, 6, 0);
//            levelEndPositions[i] = new Vector3(0, -6, 0);
//        }
        
//        // Level 9-10: Đường chéo
//        levelStartPositions[8] = new Vector3(-6, 4, 0);
//        levelEndPositions[8] = new Vector3(6, -4, 0);
//        levelStartPositions[9] = new Vector3(-4, -6, 0);
//        levelEndPositions[9] = new Vector3(4, 6, 0);
        
//        // Waypoint counts cho 10 levels
//        for (int i = 0; i < 10; i++)
//        {
//            levelWaypointCounts[i] = 6 + (i * 2); // 6, 8, 10, 12, 14, 16, 18, 20, 22, 24
//        }
//    }
    
//    void SetupPathSystem(int levelNumber)
//    {
//        // Tạo PathManager nếu chưa có
//        if (pathManager == null)
//        {
//            GameObject pathManagerObj = new GameObject("PathManager");
//            pathManager = pathManagerObj.AddComponent<PathManager>();
//        }
        
//        int levelIndex = levelNumber - 1;
//        if (levelIndex >= 0 && levelIndex < 10)
//        {
//            // Cấu hình PathManager cho level hiện tại
//            pathManager.pathStyle = levelPathStyles[levelIndex];
//            pathManager.startPoint = GetOrCreateStartPoint(levelStartPositions[levelIndex]);
//            pathManager.endPoint = GetOrCreateEndPoint(levelEndPositions[levelIndex]);
//            pathManager.waypointCount = levelWaypointCounts[levelIndex];
//            pathManager.autoCreatePath = true;
            
//            // Tạo path
//            pathManager.CreatePath();
            
//            Debug.Log($"Path system setup for Level {levelNumber}: {levelPathStyles[levelIndex]} style with {levelWaypointCounts[levelIndex]} waypoints");
//        }
//    }
    
//    void SetupWaveSystem(int levelNumber)
//    {
//        // Tạo LevelWaveManager nếu chưa có
//        if (waveManager == null)
//        {
//            GameObject waveManagerObj = new GameObject("LevelWaveManager");
//            waveManager = waveManagerObj.AddComponent<LevelWaveManager>();
//        }
        
//        // Tạo WaveCreator nếu chưa có
//        if (waveCreator == null)
//        {
//            GameObject waveCreatorObj = new GameObject("WaveCreator");
//            waveCreator = waveCreatorObj.AddComponent<WaveCreator>();
//        }
        
//        // Cấu hình WaveCreator cho level hiện tại
//        waveCreator.numberOfLevels = 1; // Chỉ tạo waves cho level hiện tại
//        waveCreator.wavesPerLevel = wavesPerLevel;
//        waveCreator.createWavesOnStart = false; // Tắt auto create để tự control
        
//        // Tìm enemy references
//        FindEnemyReferences();
        
//        // Tạo waves cho level hiện tại
//        waveCreator.CreateAllWaves();
        
//        // Start level sau 2 giây
//        Invoke("StartLevel", 2f);
        
//        Debug.Log($"Wave system setup for Level {levelNumber} with {wavesPerLevel} waves");
//    }
    
//    void FindEnemyReferences()
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
    
//    Transform GetOrCreateStartPoint(Vector3 position)
//    {
//        GameObject startPos = GameObject.Find("StartPos");
//        if (startPos == null)
//        {
//            startPos = new GameObject("StartPos");
//        }
//        startPos.transform.position = position;
//        return startPos.transform;
//    }
    
//    Transform GetOrCreateEndPoint(Vector3 position)
//    {
//        GameObject endPos = GameObject.Find("EndPos");
//        if (endPos == null)
//        {
//            endPos = new GameObject("EndPos");
//        }
//        endPos.transform.position = position;
//        return endPos.transform;
//    }
    
//    void StartLevel()
//    {
//        if (waveManager != null && waveManager.levels.Count > 0)
//        {
//            waveManager.StartLevel(1);
//            Debug.Log("Level started!");
//        }
//        else
//        {
//            Debug.LogError("No levels available to start!");
//        }
//    }
    
//    [ContextMenu("Test Level Setup")]
//    public void TestLevelSetup()
//    {
//        int levelNumber = GetLevelNumberFromScene();
//        Debug.Log($"Testing Level {levelNumber} setup...");
        
//        // Test Path System
//        if (pathManager != null)
//        {
//            Debug.Log($"✓ PathManager found for Level {levelNumber}");
//            Debug.Log($"✓ Path Style: {pathManager.pathStyle}");
//            Debug.Log($"✓ Waypoints: {pathManager.GetWaypoints().Count}");
//        }
//        else
//        {
//            Debug.LogError("✗ PathManager not found");
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
        
//        Debug.Log("Level setup test completed!");
//    }
    
//    [ContextMenu("Start Level Now")]
//    public void StartLevelNow()
//    {
//        StartLevel();
//    }
    
//    void OnGUI()
//    {
//        if (!Application.isPlaying) return;
        
//        int levelNumber = GetLevelNumberFromScene();
        
//        GUILayout.BeginArea(new Rect(10, 10, 250, 200));
//        GUILayout.Label($"Level {levelNumber} Auto Setup");
        
//        if (GUILayout.Button("Setup Level"))
//        {
//            SetupLevel();
//        }
        
//        if (GUILayout.Button("Test Setup"))
//        {
//            TestLevelSetup();
//        }
        
//        if (GUILayout.Button("Start Level Now"))
//        {
//            StartLevelNow();
//        }
        
//        // Status info
//        GUILayout.Label("Status:");
        
//        PathManager pm = FindObjectOfType<PathManager>();
//        GUILayout.Label($"Path Manager: {(pm != null ? "✓" : "✗")}");
        
//        LevelWaveManager lwm = FindObjectOfType<LevelWaveManager>();
//        GUILayout.Label($"Wave Manager: {(lwm != null ? "✓" : "✗")}");
        
//        if (pm != null)
//        {
//            GUILayout.Label($"Waypoints: {pm.GetWaypoints().Count}");
//        }
        
//        if (lwm != null)
//        {
//            GUILayout.Label($"Levels: {lwm.levels.Count}");
//        }
        
//        GUILayout.EndArea();
//    }
//} 