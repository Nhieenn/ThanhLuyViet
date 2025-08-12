using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject confirmationDialog;
    
    [Header("Pause Menu")]
    public Button resumeButton;
    public Button settingsButton;
    public Button mainMenuButton;
    public Button restartButton;
    
    [Header("Settings Panel")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullscreenToggle;
    public Toggle audioToggle;
    public Button backButton;
    
    [Header("Confirmation Dialog")]
    public TextMeshProUGUI confirmationText;
    public Button confirmButton;
    public Button cancelButton;
    
    [Header("Audio")]
    public AudioSource targetAudioSource;
    public AudioClip buttonClickSound;
    public AudioClip openMenuSound;
    
    [Header("Auto Setup")]
    public bool autoSetupUI = true;
    
    private GameStateManager gameStateManager;
    private bool isPaused = false;
    
    void Start()
    {
        InitializePauseMenu();
        if (autoSetupUI)
        {
            SetupPauseMenuUI();
        }
    }
    
    void Update()
    {
        // Handle escape key for pause/resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    void InitializePauseMenu()
    {
        // Find GameStateManager
        gameStateManager = FindObjectOfType<GameStateManager>();
        
        // Find target AudioSource if not assigned
        if (targetAudioSource == null)
        {
            targetAudioSource = FindObjectOfType<AudioSource>();
        }
        
        // Load saved settings
        LoadSettings();
        
        // Hide panels initially
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (confirmationDialog != null) confirmationDialog.SetActive(false);
        
        Debug.Log("PauseMenuUI initialized!");
    }
    
    void SetupPauseMenuUI()
    {
        if (pausePanel == null)
        {
            CreatePausePanel();
        }
        
        if (resumeButton == null)
        {
            CreateResumeButton();
        }
        
        if (settingsButton == null)
        {
            CreateSettingsButton();
        }
        
        if (mainMenuButton == null)
        {
            CreateMainMenuButton();
        }
        
        if (restartButton == null)
        {
            CreateRestartButton();
        }
        
        SetupButtonListeners();
    }
    
    void CreatePausePanel()
    {
        pausePanel = new GameObject("Pause Panel");
        pausePanel.transform.SetParent(FindObjectOfType<Canvas>()?.transform);
        
        RectTransform rect = pausePanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        Image bg = pausePanel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.8f);
        
        // Add pause text
        GameObject pauseTextObj = new GameObject("Pause Text");
        TextMeshProUGUI pauseText = pauseTextObj.AddComponent<TextMeshProUGUI>();
        pauseText.text = "⏸️ GAME PAUSED ⏸️";
        pauseText.color = Color.white;
        pauseText.fontSize = 36;
        pauseText.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = pauseTextObj.GetComponent<RectTransform>();
        textRect.SetParent(pausePanel.transform);
        textRect.anchorMin = new Vector2(0.3f, 0.8f);
        textRect.anchorMax = new Vector2(0.7f, 0.9f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        pausePanel.SetActive(false);
    }
    
    void CreateResumeButton()
    {
        resumeButton = CreateButton("Resume Button", "Resume Game", new Vector2(0.35f, 0.6f), new Vector2(0.65f, 0.7f));
        resumeButton.transform.SetParent(pausePanel.transform);
    }
    
    void CreateSettingsButton()
    {
        settingsButton = CreateButton("Settings Button", "Settings", new Vector2(0.35f, 0.5f), new Vector2(0.65f, 0.6f));
        settingsButton.transform.SetParent(pausePanel.transform);
    }
    
    void CreateMainMenuButton()
    {
        mainMenuButton = CreateButton("Main Menu Button", "Main Menu", new Vector2(0.35f, 0.4f), new Vector2(0.65f, 0.5f));
        mainMenuButton.transform.SetParent(pausePanel.transform);
    }
    
    void CreateRestartButton()
    {
        restartButton = CreateButton("Restart Button", "Restart Level", new Vector2(0.35f, 0.3f), new Vector2(0.65f, 0.4f));
        restartButton.transform.SetParent(pausePanel.transform);
    }
    
    Button CreateButton(string name, string text, Vector2 anchorMin, Vector2 anchorMax)
    {
        GameObject buttonObj = new GameObject(name);
        Button button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.6f, 1f, 1f);
        
        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        GameObject textObj = new GameObject("Text");
        TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = text;
        buttonText.color = Color.white;
        buttonText.fontSize = 18;
        buttonText.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.SetParent(buttonObj.transform);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        return button;
    }
    
    void SetupButtonListeners()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(ResumeGame);
        }
        
        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(OpenSettings);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(ShowMainMenuConfirmation);
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(ShowRestartConfirmation);
        }
    }
    
    public void PauseGame()
    {
        if (isPaused) return;
        
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        
        // Play open menu sound
        if (openMenuSound != null)
        {
            AudioSource.PlayClipAtPoint(openMenuSound, Camera.main.transform.position);
        }
        
        // Notify GameStateManager
        if (gameStateManager != null)
        {
            // This will be handled by GameStateManager's input handling
        }
        
        Debug.Log("Game paused!");
    }
    
    public void ResumeGame()
    {
        if (!isPaused) return;
        
        isPaused = false;
        Time.timeScale = 1f;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false);
        }
        
        // Play button click sound
        if (buttonClickSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
        
        Debug.Log("Game resumed!");
    }
    
    public void OpenSettings()
    {
        if (settingsPanel == null)
        {
            CreateSettingsPanel();
        }
        
        settingsPanel.SetActive(true);
        
        // Play button click sound
        if (buttonClickSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
    }
    
    void CreateSettingsPanel()
    {
        settingsPanel = new GameObject("Settings Panel");
        settingsPanel.transform.SetParent(FindObjectOfType<Canvas>()?.transform);
        
        RectTransform rect = settingsPanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        Image bg = settingsPanel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.9f);
        
        // Create settings content
        CreateSettingsContent();
        
        settingsPanel.SetActive(false);
    }
    
    void CreateSettingsContent()
    {
        // Settings title
        GameObject titleObj = new GameObject("Settings Title");
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "⚙️ SETTINGS ⚙️";
        titleText.color = Color.white;
        titleText.fontSize = 28;
        titleText.alignment = TextAlignmentOptions.Center;
        
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.SetParent(settingsPanel.transform);
        titleRect.anchorMin = new Vector2(0.3f, 0.8f);
        titleRect.anchorMax = new Vector2(0.7f, 0.9f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;
        
        // Audio toggle
        if (audioToggle == null)
        {
            CreateAudioToggle();
        }
        
        // Back button
        if (backButton == null)
        {
            CreateBackButton();
        }
        
        // Setup listeners
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(CloseSettings);
        }
    }
    
    void CreateAudioToggle()
    {
        GameObject toggleObj = new GameObject("Audio Toggle");
        audioToggle = toggleObj.AddComponent<Toggle>();
        
        // Create background
        GameObject bgObj = new GameObject("Background");
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.SetParent(toggleObj.transform);
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Create checkmark
        GameObject checkmarkObj = new GameObject("Checkmark");
        Image checkmarkImage = checkmarkObj.AddComponent<Image>();
        checkmarkImage.color = new Color(0.2f, 0.6f, 1f, 1f);
        
        RectTransform checkmarkRect = checkmarkObj.GetComponent<RectTransform>();
        checkmarkRect.SetParent(toggleObj.transform);
        checkmarkRect.anchorMin = new Vector2(0.1f, 0.1f);
        checkmarkRect.anchorMax = new Vector2(0.9f, 0.9f);
        checkmarkRect.offsetMin = Vector2.zero;
        checkmarkRect.offsetMax = Vector2.zero;
        
        // Create label
        GameObject labelObj = new GameObject("Label");
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.text = "Audio On/Off";
        labelText.color = Color.white;
        labelText.fontSize = 16;
        labelText.alignment = TextAlignmentOptions.Left;
        
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.SetParent(toggleObj.transform);
        labelRect.anchorMin = new Vector2(1.1f, 0.1f);
        labelRect.anchorMax = new Vector2(2.5f, 0.9f);
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;
        
        // Setup toggle
        audioToggle.targetGraphic = bgImage;
        audioToggle.graphic = checkmarkImage;
        audioToggle.isOn = PlayerPrefs.GetInt("AudioEnabled", 1) == 1;
        
        // Setup listener
        audioToggle.onValueChanged.RemoveAllListeners();
        audioToggle.onValueChanged.AddListener(OnAudioToggleChanged);
        
        // Position toggle
        RectTransform toggleRect = toggleObj.GetComponent<RectTransform>();
        toggleRect.SetParent(settingsPanel.transform);
        toggleRect.anchorMin = new Vector2(0.3f, 0.6f);
        toggleRect.anchorMax = new Vector2(0.7f, 0.7f);
        toggleRect.offsetMin = Vector2.zero;
        toggleRect.offsetMax = Vector2.zero;
    }
    
    void CreateBackButton()
    {
        backButton = CreateButton("Back Button", "Back", new Vector2(0.4f, 0.2f), new Vector2(0.6f, 0.3f));
        backButton.transform.SetParent(settingsPanel.transform);
    }
    
    void OnAudioToggleChanged(bool isOn)
    {
        // Toggle target AudioSource
        if (targetAudioSource != null)
        {
            targetAudioSource.enabled = isOn;
        }
        
        // Save state
        PlayerPrefs.SetInt("AudioEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
        
        Debug.Log($"Audio toggled: {(isOn ? "ON" : "OFF")}");
    }
    
    void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        // Play button click sound
        if (buttonClickSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
    }
    
    void ShowMainMenuConfirmation()
    {
        ShowConfirmationDialog("Are you sure you want to return to the main menu?\nAll progress will be lost.", () => {
            LoadMainMenu();
        });
    }
    
    void ShowRestartConfirmation()
    {
        ShowConfirmationDialog("Are you sure you want to restart this level?\nAll progress will be lost.", () => {
            RestartLevel();
        });
    }
    
    void ShowConfirmationDialog(string message, System.Action onConfirm)
    {
        if (confirmationDialog == null)
        {
            CreateConfirmationDialog();
        }
        
        if (confirmationText != null)
        {
            confirmationText.text = message;
        }
        
        // Setup confirm button
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() => {
                onConfirm?.Invoke();
                confirmationDialog.SetActive(false);
            });
        }
        
        // Setup cancel button
        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(() => {
                confirmationDialog.SetActive(false);
            });
        }
        
        confirmationDialog.SetActive(true);
    }
    
    void CreateConfirmationDialog()
    {
        confirmationDialog = new GameObject("Confirmation Dialog");
        confirmationDialog.transform.SetParent(FindObjectOfType<Canvas>()?.transform);
        
        RectTransform rect = confirmationDialog.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.2f, 0.3f);
        rect.anchorMax = new Vector2(0.8f, 0.7f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        Image bg = confirmationDialog.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        
        // Create confirmation text
        if (confirmationText == null)
        {
            GameObject textObj = new GameObject("Confirmation Text");
            confirmationText = textObj.AddComponent<TextMeshProUGUI>();
            confirmationText.text = "Are you sure?";
            confirmationText.color = Color.white;
            confirmationText.fontSize = 18;
            confirmationText.alignment = TextAlignmentOptions.Center;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.SetParent(confirmationDialog.transform);
            textRect.anchorMin = new Vector2(0.1f, 0.6f);
            textRect.anchorMax = new Vector2(0.9f, 0.9f);
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }
        
        // Create confirm button
        if (confirmButton == null)
        {
            confirmButton = CreateButton("Confirm Button", "Confirm", new Vector2(0.1f, 0.1f), new Vector2(0.4f, 0.3f));
            confirmButton.transform.SetParent(confirmationDialog.transform);
        }
        
        // Create cancel button
        if (cancelButton == null)
        {
            cancelButton = CreateButton("Cancel Button", "Cancel", new Vector2(0.6f, 0.1f), new Vector2(0.9f, 0.3f));
            cancelButton.transform.SetParent(confirmationDialog.transform);
        }
        
        confirmationDialog.SetActive(false);
    }
    
    void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    
    void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void LoadSettings()
    {
        // Load audio state
        if (targetAudioSource != null)
        {
            bool audioEnabled = PlayerPrefs.GetInt("AudioEnabled", 1) == 1;
            targetAudioSource.enabled = audioEnabled;
            
            if (audioToggle != null)
            {
                audioToggle.isOn = audioEnabled;
            }
        }
    }
    
    public bool IsPaused()
    {
        return isPaused;
    }
    
    [ContextMenu("Test Pause")]
    public void TestPause()
    {
        PauseGame();
    }
    
    [ContextMenu("Test Resume")]
    public void TestResume()
    {
        ResumeGame();
    }
    
    void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUILayout.BeginArea(new Rect(10, 430, 300, 150));
            GUILayout.Label("Pause Menu Debug");
            GUILayout.Label($"Is Paused: {isPaused}");
            GUILayout.Label($"Time Scale: {Time.timeScale}");
            GUILayout.Label($"Audio Source: {(targetAudioSource != null ? targetAudioSource.name : "None")}");
            GUILayout.Label($"Audio Enabled: {(targetAudioSource != null ? targetAudioSource.enabled.ToString() : "N/A")}");
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Pause Game"))
                PauseGame();
            if (GUILayout.Button("Resume Game"))
                ResumeGame();
            if (GUILayout.Button("Open Settings"))
                OpenSettings();
            
            GUILayout.EndArea();
        }
    }
}

