using UnityEngine;

public class SetupHistoryData : MonoBehaviour
{
    [Header("Setup Instructions")]
    [TextArea(5, 10)]
    public string instructions = "1. Tạo GameObject mới tên 'HistoryData'\n2. Thêm script 'HistoryData' vào GameObject này\n3. Đảm bảo GameObject được active\n4. Chạy game để kiểm tra";
    
    void Start()
    {
        CheckHistoryData();
    }
    
    [ContextMenu("Check History Data")]
    void CheckHistoryData()
    {
        Debug.Log("=== CHECKING HISTORY DATA ===");
        
        if (HistoryData.Instance != null)
        {
            Debug.Log("✓ HistoryData.Instance exists!");
            Debug.Log("✓ History data system is working!");
        }
        else
        {
            Debug.LogError("✗ HistoryData.Instance is null!");
            Debug.Log("SOLUTION: Create a GameObject named 'HistoryData' and add the 'HistoryData' script to it!");
        }
    }
    
    [ContextMenu("Create History Data GameObject")]
    void CreateHistoryDataGameObject()
    {
        // Try to find existing one first
        if (HistoryData.Instance != null)
        {
            Debug.Log("HistoryData already exists!");
            return;
        }
        
        // Create new GameObject
        GameObject historyDataGO = new GameObject("HistoryData");
        historyDataGO.AddComponent<HistoryData>();
        
        Debug.Log("✓ Created HistoryData GameObject!");
        Debug.Log("✓ Added HistoryData script!");
        Debug.Log("Now run 'Check History Data' to verify it's working.");
    }
}
