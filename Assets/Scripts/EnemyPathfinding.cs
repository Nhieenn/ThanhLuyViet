using UnityEngine;
using System.Collections.Generic;

public class EnemyPathfinding : MonoBehaviour
{
    [Header("Path Settings")]
    public List<Transform> waypoints = new List<Transform>();
    public float waypointReachDistance = 0.1f;
    public bool loopPath = false;
    
    [Header("Movement")]
    public float moveSpeed = 2f;
    public bool rotateTowardsTarget = true;
    
    [Header("Debug")]
    public bool showPath = true;
    public Color pathColor = Color.yellow;
    public float pathLineWidth = 0.1f;
    
    private int currentWaypointIndex = 0;
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Nếu không có waypoints, tìm tự động
        if (waypoints.Count == 0)
        {
            FindWaypoints();
        }
        
        // Set target cho enemy script
        if (enemy != null && waypoints.Count > 0)
        {
            enemy.SetTarget(waypoints[0]);
        }
    }
    
    void Update()
    {
        if (waypoints.Count == 0) return;
        
        MoveToNextWaypoint();
    }
    
    void FindWaypoints()
    {
        // Tìm tất cả GameObject có tag "Waypoint"
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        
        if (waypointObjects.Length > 0)
        {
            // Sắp xếp theo tên (Waypoint1, Waypoint2, ...)
            System.Array.Sort(waypointObjects, (a, b) => a.name.CompareTo(b.name));
            
            foreach (GameObject waypoint in waypointObjects)
            {
                waypoints.Add(waypoint.transform);
            }
            
            Debug.Log("Found " + waypoints.Count + " waypoints");
        }
        else
        {
            // Tạo waypoints mặc định
            CreateDefaultWaypoints();
        }
    }
    
    void CreateDefaultWaypoints()
    {
        // Tạo 3 waypoints mặc định
        Vector3[] defaultPositions = {
            new Vector3(-5, 0, 0),
            new Vector3(0, 2, 0),
            new Vector3(5, 0, 0)
        };
        
        for (int i = 0; i < defaultPositions.Length; i++)
        {
            GameObject waypoint = new GameObject("Waypoint" + (i + 1));
            waypoint.transform.position = defaultPositions[i];
            waypoint.tag = "Waypoint";
            
            // Thêm visual indicator
            GameObject visual = new GameObject("Visual");
            visual.transform.SetParent(waypoint.transform);
            visual.transform.localPosition = Vector3.zero;
            
            SpriteRenderer sr = visual.AddComponent<SpriteRenderer>();
            sr.color = Color.yellow;
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
            
            waypoints.Add(waypoint.transform);
        }
        
        Debug.Log("Created " + waypoints.Count + " default waypoints");
    }
    
    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Count) return;
        
        Transform currentWaypoint = waypoints[currentWaypointIndex];
        if (currentWaypoint == null) return;
        
        // Di chuyển đến waypoint hiện tại
        Vector3 direction = (currentWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        // Xoay enemy về hướng di chuyển
        if (rotateTowardsTarget && spriteRenderer != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        // Kiểm tra xem đã đến waypoint chưa
        float distanceToWaypoint = Vector3.Distance(transform.position, currentWaypoint.position);
        if (distanceToWaypoint <= waypointReachDistance)
        {
            // Đã đến waypoint, chuyển sang waypoint tiếp theo
            currentWaypointIndex++;
            
            // Cập nhật target cho enemy script
            if (enemy != null && currentWaypointIndex < waypoints.Count)
            {
                enemy.SetTarget(waypoints[currentWaypointIndex]);
            }
            
            Debug.Log("Reached waypoint " + (currentWaypointIndex - 1) + ", moving to next");
        }
    }
    
    public void SetWaypoints(List<Transform> newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
        
        if (enemy != null && waypoints.Count > 0)
        {
            enemy.SetTarget(waypoints[0]);
        }
    }
    
    public void SetWaypoints(Vector3[] positions)
    {
        waypoints.Clear();
        
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject waypoint = new GameObject("Waypoint" + (i + 1));
            waypoint.transform.position = positions[i];
            waypoint.tag = "Waypoint";
            waypoints.Add(waypoint.transform);
        }
        
        currentWaypointIndex = 0;
        
        if (enemy != null && waypoints.Count > 0)
        {
            enemy.SetTarget(waypoints[0]);
        }
    }
    
    void OnDrawGizmos()
    {
        if (!showPath || waypoints.Count == 0) return;
        
        Gizmos.color = pathColor;
        
        // Vẽ đường path
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
        
        // Vẽ các waypoint
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] != null)
            {
                Gizmos.DrawWireSphere(waypoints[i].position, 0.2f);
            }
        }
    }
    
    void OnGUI()
    {
        if (!showPath) return;
        
        // Hiển thị thông tin path trên màn hình
        GUILayout.BeginArea(new Rect(10, 200, 200, 100));
        GUILayout.Label("Path Info:");
        GUILayout.Label("Current Waypoint: " + currentWaypointIndex);
        GUILayout.Label("Total Waypoints: " + waypoints.Count);
        GUILayout.EndArea();
    }
} 