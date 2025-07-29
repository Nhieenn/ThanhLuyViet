using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPanelCreator : MonoBehaviour
{
    [Header("Panel Settings")]
    public Color panelBackgroundColor = new Color(0, 0, 0, 0.8f);
    public Color titleColor = Color.white;
    public Color buttonColor = Color.white;
    public Color buttonTextColor = Color.black;
    public Color lockedButtonColor = Color.gray;
    
    [Header("Grid Settings")]
    public int columns = 5;
    public int rows = 2;
    public float buttonSpacing = 10f;
    public Vector2 buttonSize = new Vector2(100, 100);
    
    [Header("Level Names")]
    public string[] levelSceneNames = { "LV1", "LV2", "LV3", "LV4", "LV5", "LV6", "LV7", "LV8", "Lv9", "Lv10" };
    
    private Canvas canvas;
    private MenuController menuController;
    private GameObject levelSelectPanel;
    private GameObject gridContainer;
    private Button backButton;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        menuController = FindObjectOfType<MenuController>();
        
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in scene!");
            return;
        }
        
        CreateLevelSelectPanel();
        UpdateMenuController();
    }

    void CreateLevelSelectPanel()
    {
        // Tạo Panel chính
        levelSelectPanel = CreatePanel("LevelSelectPanel");
        levelSelectPanel.SetActive(false); // Ban đầu ẩn panel
        
        // Tạo Title
        CreateTitle();
        
        // Tạo Grid Container
        CreateGridContainer();
        
        // Tạo các nút màn chơi
        CreateLevelButtons();
        
        // Tạo Back Button
        CreateBackButton();
    }

    GameObject CreatePanel(string name)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(canvas.transform, false);
        
        // Thêm Image component cho background
        Image image = panel.AddComponent<Image>();
        image.color = panelBackgroundColor;
        
        // Setup RectTransform
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        return panel;
    }

    void CreateTitle()
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(levelSelectPanel.transform, false);
        
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "CHỌN MÀN CHƠI";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 32;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = titleColor;
        
        // Setup RectTransform
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.8f);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;
    }

    void CreateGridContainer()
    {
        gridContainer = new GameObject("GridContainer");
        gridContainer.transform.SetParent(levelSelectPanel.transform, false);
        
        // Thêm GridLayoutGroup
        GridLayoutGroup gridLayout = gridContainer.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = buttonSize;
        gridLayout.spacing = new Vector2(buttonSpacing, buttonSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        
        // Setup RectTransform
        RectTransform gridRect = gridContainer.GetComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0.1f, 0.1f);
        gridRect.anchorMax = new Vector2(0.9f, 0.7f);
        gridRect.offsetMin = Vector2.zero;
        gridRect.offsetMax = Vector2.zero;
    }

    void CreateLevelButtons()
    {
        for (int i = 0; i < levelSceneNames.Length; i++)
        {
            CreateLevelButton(i);
        }
    }

    void CreateLevelButton(int levelIndex)
    {
        GameObject buttonObj = new GameObject($"LevelButton_{levelIndex + 1}");
        buttonObj.transform.SetParent(gridContainer.transform, false);
        
        // Thêm Image component
        Image image = buttonObj.AddComponent<Image>();
        image.color = buttonColor;
        
        // Thêm Button component
        Button button = buttonObj.AddComponent<Button>();
        
        // Tạo Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 16;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = buttonTextColor;
        text.text = $"Level {levelIndex + 1}";
        
        // Setup RectTransform cho text
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        // Setup button functionality
        int levelToLoad = levelIndex;
        button.onClick.AddListener(() => LoadLevel(levelToLoad));
    }

    void CreateBackButton()
    {
        GameObject backObj = new GameObject("BackButton");
        backObj.transform.SetParent(levelSelectPanel.transform, false);
        
        // Thêm Image component
        Image image = backObj.AddComponent<Image>();
        image.color = buttonColor;
        
        // Thêm Button component
        backButton = backObj.AddComponent<Button>();
        backButton.onClick.AddListener(BackToMainMenu);
        
        // Tạo Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(backObj.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = buttonTextColor;
        text.text = "BACK";
        
        // Setup RectTransform cho text
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        // Setup RectTransform cho button
        RectTransform backRect = backObj.GetComponent<RectTransform>();
        backRect.anchorMin = new Vector2(0.4f, 0.05f);
        backRect.anchorMax = new Vector2(0.6f, 0.15f);
        backRect.offsetMin = Vector2.zero;
        backRect.offsetMax = Vector2.zero;
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
                levelSelectPanelField.SetValue(menuController, levelSelectPanel);
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelSceneNames.Length)
        {
            string sceneName = levelSceneNames[levelIndex];
            Debug.Log($"Loading level: {sceneName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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
} 