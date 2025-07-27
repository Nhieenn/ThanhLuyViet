using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelGridUI : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columns = 5; // Số cột trong grid
    public int rows = 2; // Số hàng trong grid
    public float buttonSpacing = 10f; // Khoảng cách giữa các nút
    
    [Header("Button Settings")]
    public GameObject buttonPrefab; // Prefab cho nút (có thể để null để tạo tự động)
    public Transform gridContainer; // Container chứa grid
    
    [Header("Level Names")]
    public string[] levelSceneNames = { "LV1", "LV2", "LV3", "LV4", "LV5", "LV6", "LV7", "LV8", "Lv9", "Lv10" };
    
    [Header("Visual Settings")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = Color.gray;
    public int maxUnlockedLevel = 10;
    
    private MenuController menuController;

    void Start()
    {
        menuController = FindObjectOfType<MenuController>();
        CreateLevelGrid();
    }

    void CreateLevelGrid()
    {
        if (gridContainer == null)
        {
            Debug.LogError("Grid Container is not assigned!");
            return;
        }

        // Xóa các nút cũ
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        // Tính toán kích thước nút
        RectTransform containerRect = gridContainer.GetComponent<RectTransform>();
        float buttonWidth = (containerRect.rect.width - (columns - 1) * buttonSpacing) / columns;
        float buttonHeight = (containerRect.rect.height - (rows - 1) * buttonSpacing) / rows;

        // Tạo grid layout
        GridLayoutGroup gridLayout = gridContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = gridContainer.gameObject.AddComponent<GridLayoutGroup>();
        }
        
        gridLayout.cellSize = new Vector2(buttonWidth, buttonHeight);
        gridLayout.spacing = new Vector2(buttonSpacing, buttonSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        // Tạo 10 nút màn chơi
        for (int i = 0; i < levelSceneNames.Length; i++)
        {
            CreateLevelButton(i);
        }
    }

    void CreateLevelButton(int levelIndex)
    {
        GameObject buttonObj;
        
        if (buttonPrefab != null)
        {
            buttonObj = Instantiate(buttonPrefab, gridContainer);
        }
        else
        {
            // Tạo nút mặc định
            buttonObj = new GameObject($"LevelButton_{levelIndex + 1}");
            buttonObj.transform.SetParent(gridContainer);
            
            // Thêm Image component
            Image image = buttonObj.AddComponent<Image>();
            image.color = Color.white;
            
            // Thêm Button component
            Button buttons = buttonObj.AddComponent<Button>();
            
            // Tạo Text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);
            
            Text text = textObj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 20;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.black;
            text.text = $"Level {levelIndex + 1}";
            
            // Setup RectTransform cho text
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }

        // Setup button functionality
        Button button = buttonObj.GetComponent<Button>();
        int levelToLoad = levelIndex;
        
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
            buttonText.text = $"Level {levelIndex + 1}";
            if (!isUnlocked)
            {
                buttonText.text += "\n🔒";
            }
        }
    }

    public void LoadLevel(int levelIndex)
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

    // Hàm để cập nhật số màn đã unlock
    public void UpdateUnlockedLevels(int newMaxLevel)
    {
        maxUnlockedLevel = newMaxLevel;
        CreateLevelGrid();
    }
} 