using UnityEngine;

public class PathSystemSetup : MonoBehaviour
{
    [Header("Auto Setup")]
    public bool setupOnStart = true;
    public bool createPathManager = true;
    public bool createStartEndPoints = true;
    
    [Header("Path Manager Settings")]
    public PathManager.PathStyle defaultPathStyle = PathManager.PathStyle.Curved;
    public int defaultWaypointCount = 8;
    public float defaultCurveIntensity = 2f;
    
    [Header("Start/End Points")]
    public Vector3 startPosition = new Vector3(-8, 0, 0);
    public Vector3 endPosition = new Vector3(8, 0, 0);
    
    void Start()
    {
        if (setupOnStart)
        {
            SetupPathSystem();
        }
    }
    
    [ContextMenu("Setup Path System")]
    public void SetupPathSystem()
    {
        Debug.Log("Setting up Path System...");
        
        // Tạo StartPos và EndPos
        if (createStartEndPoints)
        {
            CreateStartEndPoints();
        }
        
        // Tạo PathManager
        if (createPathManager)
        {
            CreatePathManager();
        }
        
        Debug.Log("Path System setup completed!");
    }
    
    void CreateStartEndPoints()
    {
        // Tạo StartPos
        GameObject startPos = GameObject.Find("StartPos");
        if (startPos == null)
        {
            startPos = new GameObject("StartPos");
            startPos.transform.position = startPosition;
            Debug.Log("Created StartPos at " + startPosition);
        }
        
        // Tạo EndPos
        GameObject endPos = GameObject.Find("EndPos");
        if (endPos == null)
        {
            endPos = new GameObject("EndPos");
            endPos.transform.position = endPosition;
            Debug.Log("Created EndPos at " + endPosition);
        }
    }
    
    void CreatePathManager()
    {
        // Kiểm tra xem đã có PathManager chưa
        PathManager existingManager = FindObjectOfType<PathManager>();
        if (existingManager != null)
        {
            Debug.Log("PathManager already exists in scene");
            return;
        }
        
        // Tạo GameObject cho PathManager
        GameObject pathManagerObj = new GameObject("PathManager");
        PathManager pathManager = pathManagerObj.AddComponent<PathManager>();
        
        // Cấu hình PathManager
        pathManager.pathStyle = defaultPathStyle;
        pathManager.waypointCount = defaultWaypointCount;
        pathManager.curveIntensity = defaultCurveIntensity;
        pathManager.autoCreatePath = true;
        
        // Tìm và gán start/end points
        GameObject startPos = GameObject.Find("StartPos");
        GameObject endPos = GameObject.Find("EndPos");
        
        if (startPos != null)
        {
            pathManager.startPoint = startPos.transform;
        }
        
        if (endPos != null)
        {
            pathManager.endPoint = endPos.transform;
        }
        
        Debug.Log("Created PathManager with " + defaultPathStyle + " style");
    }
    
    [ContextMenu("Clear Path System")]
    public void ClearPathSystem()
    {
        // Xóa PathManager
        PathManager pathManager = FindObjectOfType<PathManager>();
        if (pathManager != null)
        {
            DestroyImmediate(pathManager.gameObject);
            Debug.Log("Cleared PathManager");
        }
        
        // Xóa StartPos và EndPos
        GameObject startPos = GameObject.Find("StartPos");
        if (startPos != null)
        {
            DestroyImmediate(startPos);
            Debug.Log("Cleared StartPos");
        }
        
        GameObject endPos = GameObject.Find("EndPos");
        if (endPos != null)
        {
            DestroyImmediate(endPos);
            Debug.Log("Cleared EndPos");
        }
        
        // Xóa tất cả waypoints
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in waypoints)
        {
            DestroyImmediate(waypoint);
        }
        Debug.Log("Cleared " + waypoints.Length + " waypoints");
    }
    
    [ContextMenu("Test Path System")]
    public void TestPathSystem()
    {
        // Kiểm tra PathManager
        PathManager pathManager = FindObjectOfType<PathManager>();
        if (pathManager == null)
        {
            Debug.LogError("PathManager not found! Please setup path system first.");
            return;
        }
        
        // Kiểm tra StartPos và EndPos
        GameObject startPos = GameObject.Find("StartPos");
        GameObject endPos = GameObject.Find("EndPos");
        
        if (startPos == null || endPos == null)
        {
            Debug.LogError("StartPos or EndPos not found! Please setup path system first.");
            return;
        }
        
        // Tạo path
        pathManager.CreatePath();
        
        Debug.Log("Path system test completed successfully!");
    }
    
    void OnDrawGizmos()
    {
        // Vẽ start và end positions trong Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, 0.5f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPosition, 0.5f);
        
        // Vẽ đường thẳng giữa start và end
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition, endPosition);
    }
} 