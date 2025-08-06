using UnityEngine;
using System.Collections.Generic;

public class PathFromGameObjects : MonoBehaviour
{
    [Header("Path Detection")]
    public bool autoDetectOnStart = true;
    public string[] pathObjectNames = { "Path", "Waypoint", "Checkpoint", "Node" };
    public string[] pathObjectTags = { "Path", "Waypoint", "Checkpoint" };
    
    [Header("Path Creation")]
    public bool createPathFromObjects = true;
    public bool showPathLine = true;
    public Color pathLineColor = Color.yellow;
    public float pathLineWidth = 0.2f;
    
    [Header("Path Manager")]
    public PathManager pathManager;
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    public bool logPathObjects = true;
    
    private List<Transform> detectedPathObjects = new List<Transform>();
    private LineRenderer pathLine;
    
    void Start()
    {
        if (autoDetectOnStart)
        {
            DetectPathObjects();
            if (createPathFromObjects)
            {
                CreatePathFromObjects();
            }
        }
    }
    
    [ContextMenu("Detect Path Objects")]
    public void DetectPathObjects()
    {
        detectedPathObjects.Clear();
        
        // Tìm objects theo tên
        foreach (string objectName in pathObjectNames)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(objectName);
            foreach (GameObject obj in objects)
            {
                if (!detectedPathObjects.Contains(obj.transform))
                {
                    detectedPathObjects.Add(obj.transform);
                    if (logPathObjects)
                    {
                        Debug.Log($"Found path object by tag: {obj.name}");
                    }
                }
            }
            
            // Tìm objects có tên chứa keyword
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.ToLower().Contains(objectName.ToLower()) && !detectedPathObjects.Contains(obj.transform))
                {
                    detectedPathObjects.Add(obj.transform);
                    if (logPathObjects)
                    {
                        Debug.Log($"Found path object by name: {obj.name}");
                    }
                }
            }
        }
        
        // Tìm objects theo tag
        foreach (string tag in pathObjectTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                if (!detectedPathObjects.Contains(obj.transform))
                {
                    detectedPathObjects.Add(obj.transform);
                    if (logPathObjects)
                    {
                        Debug.Log($"Found path object by tag: {obj.name} (Tag: {tag})");
                    }
                }
            }
        }
        
        // Sắp xếp theo tên (để có thứ tự đúng)
        detectedPathObjects.Sort((a, b) => a.name.CompareTo(b.name));
        
        Debug.Log($"Detected {detectedPathObjects.Count} path objects");
        
        // Log tất cả objects tìm được
        if (logPathObjects)
        {
            for (int i = 0; i < detectedPathObjects.Count; i++)
            {
                Debug.Log($"Path Object {i + 1}: {detectedPathObjects[i].name} at {detectedPathObjects[i].position}");
            }
        }
    }
    
    [ContextMenu("Create Path From Objects")]
    public void CreatePathFromObjects()
    {
        if (detectedPathObjects.Count == 0)
        {
            Debug.LogWarning("No path objects detected! Run DetectPathObjects first.");
            return;
        }
        
        // Tạo PathManager nếu chưa có
        if (pathManager == null)
        {
            GameObject pathManagerObj = new GameObject("PathManager");
            pathManager = pathManagerObj.AddComponent<PathManager>();
        }
        
        // Cấu hình PathManager để sử dụng custom waypoints
        pathManager.autoCreatePath = false; // Tắt auto create
        
        // Tạo waypoints từ detected objects
        CreateWaypointsFromObjects();
        
        // Tạo visual path line
        if (showPathLine)
        {
            CreatePathLine();
        }
        
        Debug.Log($"Created path from {detectedPathObjects.Count} objects");
    }
    
    void CreateWaypointsFromObjects()
    {
        // Xóa waypoints cũ nếu có
        GameObject[] oldWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject oldWaypoint in oldWaypoints)
        {
            DestroyImmediate(oldWaypoint);
        }
        
        // Tạo waypoints mới từ detected objects
        List<Transform> waypoints = new List<Transform>();
        
        for (int i = 0; i < detectedPathObjects.Count; i++)
        {
            Transform pathObject = detectedPathObjects[i];
            
            // Tạo waypoint GameObject
            GameObject waypoint = new GameObject($"Waypoint_{i + 1}");
            waypoint.transform.position = pathObject.position;
            waypoint.tag = "Waypoint";
            
            // Thêm visual cho waypoint
            CreateWaypointVisual(waypoint, i);
            
            waypoints.Add(waypoint.transform);
        }
        
        // Set waypoints cho PathManager
        if (pathManager != null)
        {
            // Tạo custom path từ waypoints
            CreateCustomPathFromWaypoints(waypoints);
        }
    }
    
    void CreateCustomPathFromWaypoints(List<Transform> waypoints)
    {
        // Tạo PathManager với custom waypoints
        pathManager.pathStyle = PathManager.PathStyle.Straight; // Sử dụng straight để kết nối các điểm
        
        // Set start và end points
        if (waypoints.Count > 0)
        {
            pathManager.startPoint = waypoints[0];
            pathManager.endPoint = waypoints[waypoints.Count - 1];
        }
        
        // Tạo custom path line
        if (pathLine == null)
        {
            GameObject pathLineObj = new GameObject("CustomPathLine");
            pathLine = pathLineObj.AddComponent<LineRenderer>();
        }
        
        pathLine.material = new Material(Shader.Find("Sprites/Default"));
        pathLine.color = pathLineColor;
        pathLine.startWidth = pathLineWidth;
        pathLine.endWidth = pathLineWidth;
        pathLine.positionCount = waypoints.Count;
        pathLine.sortingOrder = 5;
        
        // Set positions cho line renderer
        for (int i = 0; i < waypoints.Count; i++)
        {
            pathLine.SetPosition(i, waypoints[i].position);
        }
    }
    
    void CreateWaypointVisual(GameObject waypoint, int index)
    {
        GameObject visual = new GameObject("Visual");
        visual.transform.SetParent(waypoint.transform);
        visual.transform.localPosition = Vector3.zero;
        
        SpriteRenderer sr = visual.AddComponent<SpriteRenderer>();
        sr.color = Color.red;
        sr.sortingOrder = 10;
        
        // Tạo sprite hình tròn đơn giản
        Texture2D texture = CreateCircleTexture(16, Color.red);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        sr.sprite = sprite;
        visual.transform.localScale = Vector3.one * 0.3f;
    }
    
    void CreatePathLine()
    {
        if (detectedPathObjects.Count < 2) return;
        
        if (pathLine == null)
        {
            GameObject pathLineObj = new GameObject("PathLine");
            pathLine = pathLineObj.AddComponent<LineRenderer>();
        }
        
        pathLine.material = new Material(Shader.Find("Sprites/Default"));
        pathLine.color = pathLineColor;
        pathLine.startWidth = pathLineWidth;
        pathLine.endWidth = pathLineWidth;
        pathLine.positionCount = detectedPathObjects.Count;
        pathLine.sortingOrder = 5;
        
        // Set positions cho line renderer
        for (int i = 0; i < detectedPathObjects.Count; i++)
        {
            pathLine.SetPosition(i, detectedPathObjects[i].position);
        }
    }
    
    Texture2D CreateCircleTexture(int size, Color color)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f;
        
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                pixels[y * size + x] = distance <= radius ? color : Color.clear;
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
    
    [ContextMenu("Clear Path")]
    public void ClearPath()
    {
        // Xóa waypoints
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in waypoints)
        {
            DestroyImmediate(waypoint);
        }
        
        // Xóa path line
        if (pathLine != null)
        {
            DestroyImmediate(pathLine.gameObject);
            pathLine = null;
        }
        
        detectedPathObjects.Clear();
        
        Debug.Log("Path cleared!");
    }
    
    [ContextMenu("Refresh Path")]
    public void RefreshPath()
    {
        ClearPath();
        DetectPathObjects();
        CreatePathFromObjects();
    }
    
    public List<Transform> GetDetectedPathObjects()
    {
        return new List<Transform>(detectedPathObjects);
    }
    
    public Vector3[] GetPathPositions()
    {
        Vector3[] positions = new Vector3[detectedPathObjects.Count];
        for (int i = 0; i < detectedPathObjects.Count; i++)
        {
            positions[i] = detectedPathObjects[i].position;
        }
        return positions;
    }
    
    void OnDrawGizmos()
    {
        if (!showDebugInfo || detectedPathObjects.Count == 0) return;
        
        // Vẽ đường path trong Scene view
        Gizmos.color = pathLineColor;
        for (int i = 0; i < detectedPathObjects.Count - 1; i++)
        {
            if (detectedPathObjects[i] != null && detectedPathObjects[i + 1] != null)
            {
                Gizmos.DrawLine(detectedPathObjects[i].position, detectedPathObjects[i + 1].position);
            }
        }
        
        // Vẽ các path objects
        Gizmos.color = Color.green;
        for (int i = 0; i < detectedPathObjects.Count; i++)
        {
            if (detectedPathObjects[i] != null)
            {
                Gizmos.DrawWireSphere(detectedPathObjects[i].position, 0.3f);
            }
        }
    }
    
    void OnGUI()
    {
        if (!Application.isPlaying) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 250, 200));
        GUILayout.Label("Path From GameObjects");
        
        if (GUILayout.Button("Detect Path Objects"))
        {
            DetectPathObjects();
        }
        
        if (GUILayout.Button("Create Path"))
        {
            CreatePathFromObjects();
        }
        
        if (GUILayout.Button("Refresh Path"))
        {
            RefreshPath();
        }
        
        if (GUILayout.Button("Clear Path"))
        {
            ClearPath();
        }
        
        // Status info
        GUILayout.Label("Status:");
        GUILayout.Label($"Detected Objects: {detectedPathObjects.Count}");
        
        PathManager pm = FindObjectOfType<PathManager>();
        GUILayout.Label($"Path Manager: {(pm != null ? "✓" : "✗")}");
        
        LineRenderer lr = FindObjectOfType<LineRenderer>();
        GUILayout.Label($"Path Line: {(lr != null ? "✓" : "✗")}");
        
        GUILayout.EndArea();
    }
} 