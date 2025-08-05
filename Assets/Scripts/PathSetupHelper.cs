using UnityEngine;

public class PathSetupHelper : MonoBehaviour
{
    [Header("Path Setup")]
    public bool createStartEndPoints = true;
    public Vector3 startPosition = new Vector3(-8, 0, 0);
    public Vector3 endPosition = new Vector3(8, 0, 0);
    
    [Header("Visual Settings")]
    public Color startPointColor = Color.green;
    public Color endPointColor = Color.red;
    public float pointSize = 0.5f;
    
    void Start()
    {
        if (createStartEndPoints)
        {
            CreateStartEndPoints();
        }
    }
    
    [ContextMenu("Create Start/End Points")]
    public void CreateStartEndPoints()
    {
        // Tạo StartPos
        GameObject startPos = GameObject.Find("StartPos");
        if (startPos == null)
        {
            startPos = new GameObject("StartPos");
            startPos.transform.position = startPosition;
            CreatePointVisual(startPos, startPointColor, "Start");
            Debug.Log("Created StartPos at " + startPosition);
        }
        
        // Tạo EndPos
        GameObject endPos = GameObject.Find("EndPos");
        if (endPos == null)
        {
            endPos = new GameObject("EndPos");
            endPos.transform.position = endPosition;
            CreatePointVisual(endPos, endPointColor, "End");
            Debug.Log("Created EndPos at " + endPosition);
        }
    }
    
    void CreatePointVisual(GameObject point, Color color, string label)
    {
        // Tạo visual cho point
        GameObject visual = new GameObject("Visual");
        visual.transform.SetParent(point.transform);
        visual.transform.localPosition = Vector3.zero;
        
        SpriteRenderer sr = visual.AddComponent<SpriteRenderer>();
        sr.color = color;
        sr.sortingOrder = 10;
        
        // Tạo sprite hình tròn
        Texture2D texture = CreateCircleTexture(32, color);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
        sr.sprite = sprite;
        visual.transform.localScale = Vector3.one * pointSize;
        
        // Tạo label text
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(point.transform);
        labelObj.transform.localPosition = Vector3.up * (pointSize + 0.5f);
        
        TextMesh textMesh = labelObj.AddComponent<TextMesh>();
        textMesh.text = label;
        textMesh.fontSize = 20;
        textMesh.color = color;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
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
    
    [ContextMenu("Clear Start/End Points")]
    public void ClearStartEndPoints()
    {
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
    }
    
    void OnDrawGizmos()
    {
        // Vẽ start point
        Gizmos.color = startPointColor;
        Gizmos.DrawWireSphere(startPosition, pointSize);
        
        // Vẽ end point
        Gizmos.color = endPointColor;
        Gizmos.DrawWireSphere(endPosition, pointSize);
        
        // Vẽ đường thẳng giữa start và end
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition, endPosition);
    }
} 