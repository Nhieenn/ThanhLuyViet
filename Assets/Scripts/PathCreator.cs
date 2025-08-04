using UnityEngine;
using System.Collections.Generic;

public class PathCreator : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform spawnPoint;
    public Transform homeBase;
    public int waypointCount = 5;
    public bool createPathOnStart = true;
    
    [Header("Path Generation")]
    public bool useCurvedPath = true;
    public float curveHeight = 2f;
    public float curveWidth = 3f;
    
    [Header("Visual")]
    public bool showPathLines = true;
    public Color pathColor = Color.yellow;
    public float lineWidth = 0.1f;
    
    private List<Transform> waypoints = new List<Transform>();
    
    void Start()
    {
        if (createPathOnStart)
        {
            CreatePath();
        }
    }
    
    [ContextMenu("Create Path")]
    public void CreatePath()
    {
        // Tìm spawn point và home base nếu chưa được gán
        if (spawnPoint == null)
        {
            GameObject startPos = GameObject.Find("StartPos");
            if (startPos != null)
            {
                spawnPoint = startPos.transform;
            }
        }
        
        if (homeBase == null)
        {
            GameObject endPos = GameObject.Find("EndPos");
            if (endPos != null)
            {
                homeBase = endPos.transform;
            }
        }
        
        if (spawnPoint == null || homeBase == null)
        {
            Debug.LogError("Spawn point or Home base not found!");
            return;
        }
        
        // Xóa waypoints cũ
        ClearWaypoints();
        
        // Tạo waypoints mới
        CreateWaypoints();
        
        Debug.Log("Path created with " + waypoints.Count + " waypoints");
    }
    
    void CreateWaypoints()
    {
        Vector3 startPos = spawnPoint.position;
        Vector3 endPos = homeBase.position;
        
        for (int i = 0; i < waypointCount; i++)
        {
            float t = (float)i / (waypointCount - 1);
            Vector3 waypointPos;
            
            if (useCurvedPath)
            {
                // Tạo đường cong
                Vector3 midPoint = Vector3.Lerp(startPos, endPos, 0.5f);
                midPoint.y += curveHeight;
                midPoint.x += Random.Range(-curveWidth, curveWidth);
                
                // Bezier curve với 3 điểm
                Vector3 p0 = startPos;
                Vector3 p1 = midPoint;
                Vector3 p2 = endPos;
                
                waypointPos = CalculateBezierPoint(t, p0, p1, p2);
            }
            else
            {
                // Đường thẳng
                waypointPos = Vector3.Lerp(startPos, endPos, t);
            }
            
            // Tạo GameObject cho waypoint
            GameObject waypoint = new GameObject("Waypoint" + (i + 1));
            waypoint.transform.position = waypointPos;
            waypoint.tag = "Waypoint";
            
            // Thêm visual indicator
            CreateWaypointVisual(waypoint, i);
            
            waypoints.Add(waypoint.transform);
        }
    }
    
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        
        Vector3 point = uu * p0 + 2 * u * t * p1 + tt * p2;
        return point;
    }
    
    void CreateWaypointVisual(GameObject waypoint, int index)
    {
        GameObject visual = new GameObject("Visual");
        visual.transform.SetParent(waypoint.transform);
        visual.transform.localPosition = Vector3.zero;
        
        SpriteRenderer sr = visual.AddComponent<SpriteRenderer>();
        
        // Màu khác nhau cho waypoint đầu, giữa và cuối
        if (index == 0)
        {
            sr.color = Color.green; // Spawn point
        }
        else if (index == waypointCount - 1)
        {
            sr.color = Color.red; // Home base
        }
        else
        {
            sr.color = Color.yellow; // Waypoint giữa
        }
        
        sr.sortingOrder = 5;
        
        // Tạo sprite đơn giản
        Texture2D texture = new Texture2D(16, 16);
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        visual.transform.localScale = Vector3.one * 0.3f;
    }
    
    void ClearWaypoints()
    {
        // Xóa tất cả waypoints cũ
        GameObject[] oldWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in oldWaypoints)
        {
            DestroyImmediate(waypoint);
        }
        
        waypoints.Clear();
    }
    
    public List<Transform> GetWaypoints()
    {
        return waypoints;
    }
    
    public void SetPathForEnemy(EnemyPathfinding enemyPathfinding)
    {
        if (enemyPathfinding != null)
        {
            enemyPathfinding.SetWaypoints(waypoints);
        }
    }
    
    void OnDrawGizmos()
    {
        if (!showPathLines || waypoints.Count == 0) return;
        
        Gizmos.color = pathColor;
        
        // Vẽ đường path
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 300, 200, 100));
        GUILayout.Label("Path Creator");
        
        if (GUILayout.Button("Create Path"))
        {
            CreatePath();
        }
        
        if (GUILayout.Button("Clear Path"))
        {
            ClearWaypoints();
        }
        
        GUILayout.Label("Waypoints: " + waypoints.Count);
        GUILayout.EndArea();
    }
} 