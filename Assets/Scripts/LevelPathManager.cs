using UnityEngine;
using System.Collections.Generic;

public class LevelPathManager : MonoBehaviour
{
    [Header("Level Path Settings")]
    public int currentLevel = 1;
    public PathManager.PathStyle[] levelPathStyles = new PathManager.PathStyle[10];
    public Vector3[] levelStartPositions = new Vector3[10];
    public Vector3[] levelEndPositions = new Vector3[10];
    public int[] levelWaypointCounts = new int[10];
    
    [Header("Path Manager")]
    public PathManager pathManager;
    
    void Start()
    {
        // Khởi tạo path styles mặc định cho 10 levels
        InitializeDefaultPathStyles();
        
        // Khởi tạo vị trí start/end mặc định
        InitializeDefaultPositions();
        
        // Khởi tạo waypoint counts mặc định
        InitializeDefaultWaypointCounts();
        
        // Tạo PathManager nếu chưa có
        if (pathManager == null)
        {
            CreatePathManager();
        }
    }
    
    void InitializeDefaultPathStyles()
    {
        levelPathStyles[0] = PathManager.PathStyle.Straight;      // Level 1: Đường thẳng
        levelPathStyles[1] = PathManager.PathStyle.Curved;        // Level 2: Đường cong
        levelPathStyles[2] = PathManager.PathStyle.ZigZag;        // Level 3: Zic zac
        levelPathStyles[3] = PathManager.PathStyle.Curved;        // Level 4: Đường cong
        levelPathStyles[4] = PathManager.PathStyle.Spiral;        // Level 5: Xoắn ốc
        levelPathStyles[5] = PathManager.PathStyle.ZigZag;        // Level 6: Zic zac
        levelPathStyles[6] = PathManager.PathStyle.Curved;        // Level 7: Đường cong
        levelPathStyles[7] = PathManager.PathStyle.Random;        // Level 8: Ngẫu nhiên
        levelPathStyles[8] = PathManager.PathStyle.Spiral;        // Level 9: Xoắn ốc
        levelPathStyles[9] = PathManager.PathStyle.Random;        // Level 10: Ngẫu nhiên
    }
    
    void InitializeDefaultPositions()
    {
        // Level 1-5: Đường đi từ trái sang phải
        for (int i = 0; i < 5; i++)
        {
            levelStartPositions[i] = new Vector3(-8, 0, 0);
            levelEndPositions[i] = new Vector3(8, 0, 0);
        }
        
        // Level 6-8: Đường đi từ trên xuống dưới
        for (int i = 5; i < 8; i++)
        {
            levelStartPositions[i] = new Vector3(0, 6, 0);
            levelEndPositions[i] = new Vector3(0, -6, 0);
        }
        
        // Level 9-10: Đường đi chéo
        levelStartPositions[8] = new Vector3(-6, 4, 0);
        levelEndPositions[8] = new Vector3(6, -4, 0);
        levelStartPositions[9] = new Vector3(-4, -6, 0);
        levelEndPositions[9] = new Vector3(4, 6, 0);
    }
    
    void InitializeDefaultWaypointCounts()
    {
        // Tăng số lượng waypoints theo level để tạo độ phức tạp
        for (int i = 0; i < 10; i++)
        {
            levelWaypointCounts[i] = 6 + (i * 2); // 6, 8, 10, 12, 14, 16, 18, 20, 22, 24
        }
    }
    
    void CreatePathManager()
    {
        GameObject pathManagerObj = new GameObject("PathManager");
        pathManager = pathManagerObj.AddComponent<PathManager>();
    }
    
    [ContextMenu("Load Path For Current Level")]
    public void LoadPathForCurrentLevel()
    {
        if (pathManager == null)
        {
            CreatePathManager();
        }
        
        int levelIndex = currentLevel - 1; // Chuyển từ level number sang array index
        
        if (levelIndex >= 0 && levelIndex < 10)
        {
            // Cấu hình PathManager cho level hiện tại
            pathManager.pathStyle = levelPathStyles[levelIndex];
            pathManager.startPoint = GetOrCreateStartPoint(levelStartPositions[levelIndex]);
            pathManager.endPoint = GetOrCreateEndPoint(levelEndPositions[levelIndex]);
            pathManager.waypointCount = levelWaypointCounts[levelIndex];
            
            // Tạo path
            pathManager.CreatePath();
            
            Debug.Log($"Loaded path for Level {currentLevel}: {levelPathStyles[levelIndex]} style with {levelWaypointCounts[levelIndex]} waypoints");
        }
        else
        {
            Debug.LogError($"Invalid level: {currentLevel}. Must be between 1-10.");
        }
    }
    
    Transform GetOrCreateStartPoint(Vector3 position)
    {
        GameObject startPos = GameObject.Find("StartPos");
        if (startPos == null)
        {
            startPos = new GameObject("StartPos");
        }
        startPos.transform.position = position;
        return startPos.transform;
    }
    
    Transform GetOrCreateEndPoint(Vector3 position)
    {
        GameObject endPos = GameObject.Find("EndPos");
        if (endPos == null)
        {
            endPos = new GameObject("EndPos");
        }
        endPos.transform.position = position;
        return endPos.transform;
    }
    
    [ContextMenu("Load Path For Level 1")]
    public void LoadLevel1() { currentLevel = 1; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 2")]
    public void LoadLevel2() { currentLevel = 2; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 3")]
    public void LoadLevel3() { currentLevel = 3; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 4")]
    public void LoadLevel4() { currentLevel = 4; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 5")]
    public void LoadLevel5() { currentLevel = 5; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 6")]
    public void LoadLevel6() { currentLevel = 6; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 7")]
    public void LoadLevel7() { currentLevel = 7; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 8")]
    public void LoadLevel8() { currentLevel = 8; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 9")]
    public void LoadLevel9() { currentLevel = 9; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load Path For Level 10")]
    public void LoadLevel10() { currentLevel = 10; LoadPathForCurrentLevel(); }
    
    [ContextMenu("Load All Paths")]
    public void LoadAllPaths()
    {
        for (int level = 1; level <= 10; level++)
        {
            currentLevel = level;
            LoadPathForCurrentLevel();
            Debug.Log($"Created path for Level {level}");
        }
    }
    
    public void SetLevel(int level)
    {
        if (level >= 1 && level <= 10)
        {
            currentLevel = level;
            LoadPathForCurrentLevel();
        }
        else
        {
            Debug.LogError($"Invalid level: {level}. Must be between 1-10.");
        }
    }
    
    public PathManager.PathStyle GetCurrentPathStyle()
    {
        int levelIndex = currentLevel - 1;
        if (levelIndex >= 0 && levelIndex < levelPathStyles.Length)
        {
            return levelPathStyles[levelIndex];
        }
        return PathManager.PathStyle.Straight;
    }
    
    public Vector3 GetCurrentStartPosition()
    {
        int levelIndex = currentLevel - 1;
        if (levelIndex >= 0 && levelIndex < levelStartPositions.Length)
        {
            return levelStartPositions[levelIndex];
        }
        return Vector3.zero;
    }
    
    public Vector3 GetCurrentEndPosition()
    {
        int levelIndex = currentLevel - 1;
        if (levelIndex >= 0 && levelIndex < levelEndPositions.Length)
        {
            return levelEndPositions[levelIndex];
        }
        return Vector3.zero;
    }
    
    void OnGUI()
    {
        if (!Application.isPlaying) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 200, 300));
        GUILayout.Label("Level Path Manager");
        
        GUILayout.Label($"Current Level: {currentLevel}");
        
        if (GUILayout.Button("Load Current Level Path"))
        {
            LoadPathForCurrentLevel();
        }
        
        GUILayout.Space(10);
        
        for (int i = 1; i <= 10; i++)
        {
            if (GUILayout.Button($"Load Level {i}"))
            {
                SetLevel(i);
            }
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Load All Paths"))
        {
            LoadAllPaths();
        }
        
        GUILayout.EndArea();
    }
} 