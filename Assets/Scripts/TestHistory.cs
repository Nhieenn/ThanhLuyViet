using UnityEngine;
using UnityEngine.SceneManagement;

public class TestHistory : MonoBehaviour
{
    void Start()
    {
        TestHistoryData();
    }
    
    [ContextMenu("Test History Data")]
    void TestHistoryData()
    {
        Debug.Log("=== TESTING HISTORY DATA ===");
        
        // Check if HistoryData.Instance exists
        if (HistoryData.Instance != null)
        {
            Debug.Log("✓ HistoryData.Instance exists");
            
            // Get current scene name
            string sceneName = SceneManager.GetActiveScene().name;
            Debug.Log("Current Scene Name: " + sceneName);
            
            // Try to get history for current scene
            var info = HistoryData.Instance.GetHistoryForScene(sceneName);
            Debug.Log("History for " + sceneName + ":");
            Debug.Log("- Battle Name: " + info.battleName);
            Debug.Log("- Force Comparison: " + info.forceComparison);
            Debug.Log("- Battle Meaning: " + info.battleMeaning);
            
            // Test with known scene names
            string[] testScenes = { "Lv1", "Lv2", "Lv3" };
            foreach (string testScene in testScenes)
            {
                var testInfo = HistoryData.Instance.GetHistoryForScene(testScene);
                Debug.Log("Test " + testScene + ": " + testInfo.battleName);
            }
        }
        else
        {
            Debug.LogError("✗ HistoryData.Instance is null!");
            Debug.Log("Make sure you have a GameObject with HistoryData script in your scene!");
        }
    }
}
