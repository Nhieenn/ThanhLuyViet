using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Level Settings")]
    public int levelIndex = 0; // Số level (0-9 cho LV1-LV10)
    public string sceneName = ""; // Tên scene (để trống sẽ tự động tạo)
    
    [Header("Scene Names")]
    public string[] levelSceneNames = { "LV1", "LV2", "LV3", "LV4", "LV5", "LV6", "LV7", "LV8", "Lv9", "Lv10" };
    
    void Start()
    {
        // Tự động tạo tên scene nếu để trống
        if (string.IsNullOrEmpty(sceneName))
        {
            if (levelIndex >= 0 && levelIndex < levelSceneNames.Length)
            {
                sceneName = levelSceneNames[levelIndex];
            }
        }
        
        // Gán sự kiện click
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadLevel);
        }
    }
    
    public void LoadLevel()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"Loading level: {sceneName}");
            
            try
            {
                SceneManager.LoadScene(sceneName);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load scene '{sceneName}': {e.Message}");
                Debug.LogError("Please add the scene to Build Settings: File -> Build Settings");
            }
        }
        else
        {
            Debug.LogError($"Scene name is empty for level {levelIndex}");
        }
    }
} 