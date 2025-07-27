using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject levelButtonPrefab; // Prefab cho nút màn chơi
    public Transform levelButtonContainer; // Container chứa các nút
    public Button backButton; // Nút Back
    
    [Header("Level Settings")]
    public string[] levelNames = { "LV1", "LV2", "LV3", "LV4", "LV5", "LV6", "LV7", "LV8", "Lv9", "Lv10" };
    public string[] levelDisplayNames = { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", "Level 9", "Level 10" };
    
    [Header("UI Settings")]
    public int maxUnlockedLevel = 10; // Số màn đã mở khóa
    public Color unlockedColor = Color.white;
    public Color lockedColor = Color.gray;
    
    private MenuController menuController;

    void Start()
    {
        menuController = FindObjectOfType<MenuController>();
        CreateLevelButtons();
        SetupBackButton();
    }

    void CreateLevelButtons()
    {
        // Xóa các nút cũ nếu có
        foreach (Transform child in levelButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Tạo 10 nút màn chơi
        for (int i = 0; i < levelNames.Length; i++)
        {
            CreateLevelButton(i);
        }
    }

    void CreateLevelButton(int levelIndex)
    {
        GameObject buttonObj;
        
        if (levelButtonPrefab != null)
        {
            buttonObj = Instantiate(levelButtonPrefab, levelButtonContainer);
        }
        else
        {
            // Tạo nút mặc định nếu không có prefab
            buttonObj = new GameObject($"LevelButton_{levelIndex + 1}");
            buttonObj.transform.SetParent(levelButtonContainer);
            
            // Thêm các component cần thiết
            Image image = buttonObj.AddComponent<Image>();
            Button buttons = buttonObj.AddComponent<Button>();
            
            // Tạo Text cho nút
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);
            Text text = textObj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 24;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = levelDisplayNames[levelIndex];
            
            // Setup RectTransform cho text
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }

        // Setup button
        Button button = buttonObj.GetComponent<Button>();
        int levelToLoad = levelIndex; // Capture level index for lambda
        
        // Kiểm tra màn đã unlock chưa
        bool isUnlocked = (levelIndex + 1) <= maxUnlockedLevel;
        
        if (isUnlocked)
        {
            button.onClick.AddListener(() => LoadLevel(levelToLoad));
            buttonObj.GetComponent<Image>().color = unlockedColor;
        }
        else
        {
            button.interactable = false;
            buttonObj.GetComponent<Image>().color = lockedColor;
        }

        // Setup text
        Text buttonText = buttonObj.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = levelDisplayNames[levelIndex];
            if (!isUnlocked)
            {
                buttonText.text += "\n🔒";
            }
        }
    }

    void SetupBackButton()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToMainMenu);
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelNames.Length)
        {
            string sceneName = levelNames[levelIndex];
            Debug.Log($"Loading level: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Invalid level index: {levelIndex}");
        }
    }

    public void BackToMainMenu()
    {
        if (menuController != null)
        {
            menuController.OnBackButton();
        }
        else
        {
            Debug.Log("MenuController not found!");
        }
    }

    // Hàm để cập nhật số màn đã unlock (có thể gọi từ GameManager)
    public void UpdateUnlockedLevels(int newMaxLevel)
    {
        maxUnlockedLevel = newMaxLevel;
        CreateLevelButtons(); // Tạo lại các nút với trạng thái mới
    }
} 