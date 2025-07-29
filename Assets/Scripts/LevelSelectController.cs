using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [Header("Level Selection")]
    public string[] levelSceneNames = { "LV1", "LV2", "LV3", "LV4", "LV5", "LV6", "LV7", "LV8", "Lv9", "Lv10" };
    
    [Header("UI References")]
    public GameObject levelSelectPanel;
    public MenuController menuController;

    void Start()
    {
        // Đảm bảo panel chọn màn được ẩn khi khởi động
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);
    }

    // Hàm chọn màn chơi theo index
    public void SelectLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelSceneNames.Length)
        {
            string sceneName = levelSceneNames[levelIndex];
            Debug.Log($"Loading level: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Invalid level index: {levelIndex}");
        }
    }

    // Hàm chọn màn chơi theo tên scene
    public void SelectLevelByName(string sceneName)
    {
        Debug.Log($"Loading level: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // Hàm quay về menu chính
    public void BackToMainMenu()
    {
        if (menuController != null)
        {
            menuController.OnBackButton();
        }
        else
        {
            // Fallback nếu không có MenuController
            if (levelSelectPanel != null)
                levelSelectPanel.SetActive(false);
        }
    }

    // Hàm kiểm tra màn chơi đã mở khóa chưa (có thể mở rộng sau)
    public bool IsLevelUnlocked(int levelIndex)
    {
        // Logic kiểm tra màn chơi đã unlock chưa
        // Có thể dựa vào PlayerPrefs hoặc DataManager
        return levelIndex <= PlayerPrefs.GetInt("MaxUnlockedLevel", 1);
    }
} 