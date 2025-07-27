using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNavigation : MonoBehaviour
{
    [Header("Scene Names")]
    public string[] levelSceneNames = { "LV1", "LV2", "LV3", "LV4", "LV5", "LV6", "LV7", "LV8", "Lv9", "Lv10" };
    
    [Header("UI References")]
    public Button[] levelButtons; // Gán 10 nút level trong Inspector
    public Button backButton; // Gán nút Back trong Inspector
    
    [Header("Panel References")]
    public GameObject levelSelectPanel; // Panel chọn màn
    public GameObject menuPanel; // Panel menu chính
    
    void Start()
    {
        SetupLevelButtons();
        SetupBackButton();
    }
    
    void SetupLevelButtons()
    {
        // Tự động tìm level buttons nếu chưa gán
        if (levelButtons == null || levelButtons.Length == 0)
        {
            levelButtons = new Button[10];
            for (int i = 0; i < 10; i++)
            {
                GameObject buttonObj = GameObject.Find($"LevelButton_{i + 1}");
                if (buttonObj != null)
                {
                    levelButtons[i] = buttonObj.GetComponent<Button>();
                }
            }
        }
        
        // Gán sự kiện cho từng nút
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] != null)
            {
                int levelIndex = i; // Capture level index
                levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
            }
        }
    }
    
    void SetupBackButton()
    {
        // Tự động tìm back button nếu chưa gán
        if (backButton == null)
        {
            GameObject backObj = GameObject.Find("BackButton");
            if (backObj != null)
            {
                backButton = backObj.GetComponent<Button>();
            }
        }
        
        // Gán sự kiện cho nút Back
        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToMainMenu);
        }
    }
    
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelSceneNames.Length)
        {
            string sceneName = levelSceneNames[levelIndex];
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
            Debug.LogError($"Invalid level index: {levelIndex}");
        }
    }
    
    public void BackToMainMenu()
    {
        Debug.Log("Back to main menu");
        
        // Ẩn panel chọn màn
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
        
        // Hiện panel menu chính
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }
    }
    
    // Hàm để test từ Inspector
    [ContextMenu("Test Load Level 1")]
    public void TestLoadLevel1()
    {
        LoadLevel(0);
    }
    
    [ContextMenu("Test Back to Menu")]
    public void TestBackToMenu()
    {
        BackToMainMenu();
    }
} 