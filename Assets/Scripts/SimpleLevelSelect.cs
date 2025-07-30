using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleLevelSelect : MonoBehaviour
{
    [Header("Setup")]
    public bool createOnStart = true;
    
    [Header("Panel Settings")]
    public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.95f); // Dark gray
    public Color buttonColor = new Color(0.4f, 0.4f, 0.4f, 1f); // Light gray
    public Color buttonTextColor = Color.white;
    public Color backButtonColor = new Color(0.6f, 0.6f, 0.6f, 1f);
    
    [Header("Grid Settings")]
    public int columns = 5;
    public int rows = 2;
    public Vector2 buttonSize = new Vector2(120, 120);
    public float buttonSpacing = 15f;
    
    [Header("Level Names")]
    public string[] levelNames = { "LV1", "LV2", "LV3", "LV4", "LV5", "LV6", "LV7", "LV8", "Lv9", "Lv10" };
    
    [Header("Star System")]
    public bool showStars = false; // Tắt stars
    public Color starColor = Color.white;
    public Color completedStarColor = Color.orange;
    
    private GameObject levelPanel;
    private MenuController menuController;

    void Start()
    {
        if (createOnStart)
        {
            CreateLevelSelectPanel();
        }
    }

    [ContextMenu("Create Level Select Panel")]
    public void CreateLevelSelectPanel()
    {
        // Tìm Canvas và MenuController
        Canvas canvas = FindAnyObjectByType<Canvas>();
        menuController = FindAnyObjectByType<MenuController>();
        
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        // Tạo Panel chính
        levelPanel = new GameObject("LevelSelectPanel");
        levelPanel.transform.SetParent(canvas.transform, false);
        
        // Thêm Image component cho background với pattern
        Image panelImage = levelPanel.AddComponent<Image>();
        panelImage.color = backgroundColor;
        
        // Setup RectTransform
        RectTransform panelRect = levelPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Ẩn panel ban đầu
        levelPanel.SetActive(false);
        
        // Tạo Back Button ở góc trên bên trái
        CreateBackButton();
        
        // Tạo Grid cho các nút
        CreateLevelGrid();
        
        // Cập nhật MenuController
        UpdateMenuController();
        
        Debug.Log("Level Select Panel created successfully!");
    }

    void CreateBackButton()
    {
        GameObject backObj = new GameObject("BackButton");
        backObj.transform.SetParent(levelPanel.transform, false);
        
        // Thêm Image component
        Image image = backObj.AddComponent<Image>();
        image.color = backButtonColor;
        
        // Thêm Button component
        Button button = backObj.AddComponent<Button>();
        button.onClick.AddListener(BackToMainMenu);
        
        // Tạo Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(backObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 18;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        text.text = "BACK";
        text.fontStyle = FontStyle.Bold;
        
        // Setup RectTransform cho text
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        // Setup RectTransform cho button (góc trên bên trái)
        RectTransform backRect = backObj.GetComponent<RectTransform>();
        backRect.anchorMin = new Vector2(0.05f, 0.85f);
        backRect.anchorMax = new Vector2(0.15f, 0.95f);
        backRect.offsetMin = Vector2.zero;
        backRect.offsetMax = Vector2.zero;
    }

    void CreateLevelGrid()
    {
        // Tạo container cho grid
        GameObject gridContainer = new GameObject("GridContainer");
        gridContainer.transform.SetParent(levelPanel.transform, false);
        
        // Thêm GridLayoutGroup
        GridLayoutGroup gridLayout = gridContainer.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = buttonSize;
        gridLayout.spacing = new Vector2(buttonSpacing, buttonSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        
        // Setup RectTransform (để lại chỗ cho Back button)
        RectTransform gridRect = gridContainer.GetComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0.1f, 0.1f);
        gridRect.anchorMax = new Vector2(0.9f, 0.8f);
        gridRect.offsetMin = Vector2.zero;
        gridRect.offsetMax = Vector2.zero;
        
        // Tạo 10 nút màn chơi
        for (int i = 0; i < levelNames.Length; i++)
        {
            CreateLevelButton(i, gridContainer);
        }
    }

    void CreateLevelButton(int levelIndex, GameObject parent)
    {
        GameObject buttonObj = new GameObject($"LevelButton_{levelIndex + 1}");
        buttonObj.transform.SetParent(parent.transform, false);
        
        // Thêm Image component với màu metallic
        Image image = buttonObj.AddComponent<Image>();
        image.color = buttonColor;
        
        // Thêm Button component
        Button button = buttonObj.AddComponent<Button>();
        
        // Tạo Text cho số level
        GameObject textObj = new GameObject("LevelText");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = buttonTextColor;
        text.text = $"{levelIndex + 1}";
        text.fontStyle = FontStyle.Bold;
        
        // Setup RectTransform cho text (chiếm toàn bộ button)
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        // Setup button functionality
        int levelToLoad = levelIndex;
        button.onClick.AddListener(() => LoadLevel(levelToLoad));
    }

    void CreateStars(GameObject parent, int levelIndex)
    {
        // Tạo container cho stars
        GameObject starsContainer = new GameObject("StarsContainer");
        starsContainer.transform.SetParent(parent.transform, false);
        
        // Setup RectTransform cho stars (dưới số level)
        RectTransform starsRect = starsContainer.GetComponent<RectTransform>();
        starsRect.anchorMin = new Vector2(0.2f, 0.1f);
        starsRect.anchorMax = new Vector2(0.8f, 0.25f);
        starsRect.offsetMin = Vector2.zero;
        starsRect.offsetMax = Vector2.zero;
        
        // Tạo 3 stars
        for (int i = 0; i < 3; i++)
        {
            GameObject starObj = new GameObject($"Star_{i + 1}");
            starObj.transform.SetParent(starsContainer.transform, false);
            
            // Thêm Image component cho star
            Image starImage = starObj.AddComponent<Image>();
            
            // Màu star dựa trên level completion
            if (levelIndex < 3) // Levels 1-3: 3 stars
            {
                starImage.color = starColor;
            }
            else if (levelIndex == 3) // Level 4: 1 star
            {
                starImage.color = (i == 0) ? completedStarColor : Color.clear;
            }
            else // Levels 5+: no stars
            {
                starImage.color = Color.clear;
            }
            
            // Setup RectTransform cho star
            RectTransform starRect = starObj.GetComponent<RectTransform>();
            starRect.anchorMin = new Vector2(i * 0.33f, 0);
            starRect.anchorMax = new Vector2((i + 1) * 0.33f, 1);
            starRect.offsetMin = Vector2.zero;
            starRect.offsetMax = Vector2.zero;
        }
    }

    void UpdateMenuController()
    {
        if (menuController != null)
        {
            // Cập nhật reference trong MenuController
            var menuControllerType = typeof(MenuController);
            var levelSelectPanelField = menuControllerType.GetField("levelSelectPanel");
            if (levelSelectPanelField != null)
            {
                levelSelectPanelField.SetValue(menuController, levelPanel);
                Debug.Log("MenuController updated with Level Select Panel reference");
            }
        }
        else
        {
            Debug.LogWarning("MenuController not found!");
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelNames.Length)
        {
            string sceneName = levelNames[levelIndex];
            Debug.Log($"Attempting to load level: {sceneName}");
            
            // Sử dụng SceneBuildManager nếu có
            SceneBuildManager buildManager = FindAnyObjectByType<SceneBuildManager>();
            if (buildManager != null)
            {
                buildManager.LoadScene(sceneName);
            }
            else
            {
                // Fallback: sử dụng SceneManager trực tiếp
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
            menuController.OnExitSetting();
        }
        else
        {
            Debug.Log("MenuController not found!");
        }
    }
} 