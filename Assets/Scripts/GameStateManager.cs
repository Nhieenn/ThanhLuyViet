using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        Loading,
        Playing,
        Paused,
        Won,
        Lost
    }
    
    [Header("Current State")]
    public GameState currentState = GameState.Loading;
    
    [Header("Game References")]
    public GameWinManager gameWinManager;
    public SimpleHealthText playerHealth;
    
    [Header("Input Settings")]
    public KeyCode pauseKey = KeyCode.Escape;
    public KeyCode resumeKey = KeyCode.Space;
    
    [Header("Game Stats")]
    public float levelStartTime;
    public float currentLevelTime;
    public int currentScore;
    public int highestScore;
    
    // Events
    public System.Action<GameState> OnGameStateChanged;
    public System.Action OnLevelStarted;
    public System.Action OnLevelCompleted;
    public System.Action OnLevelFailed;
    
    private bool isInitialized = false;
    
    void Start()
    {
        InitializeGameStateManager();
    }
    
    void Update()
    {
        if (!isInitialized) return;
        
        HandleInput();
        UpdateGameStats();
    }
    
    void InitializeGameStateManager()
    {
        // Find references
        if (gameWinManager == null)
            gameWinManager = FindObjectOfType<GameWinManager>();
            
        if (playerHealth == null)
            playerHealth = FindObjectOfType<SimpleHealthText>();
        
        // Subscribe to events
        if (gameWinManager != null)
        {
            gameWinManager.OnGameWon += OnGameWon;
            gameWinManager.OnGameOver += OnGameOver;
        }
        
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += OnHealthChanged;
            playerHealth.OnPlayerDeath += OnPlayerDeath;
        }
        
        // Start level
        StartLevel();
        
        isInitialized = true;
        Debug.Log("GameStateManager initialized successfully!");
    }
    
    void HandleInput()
    {
        if (currentState == GameState.Playing)
        {
            if (Input.GetKeyDown(pauseKey))
            {
                PauseGame();
            }
        }
        else if (currentState == GameState.Paused)
        {
            if (Input.GetKeyDown(resumeKey) || Input.GetKeyDown(pauseKey))
            {
                ResumeGame();
            }
        }
    }
    
    void UpdateGameStats()
    {
        if (currentState == GameState.Playing)
        {
            currentLevelTime = Time.time - levelStartTime;
        }
    }
    
    public void StartLevel()
    {
        levelStartTime = Time.time;
        currentLevelTime = 0f;
        currentScore = 0;
        
        ChangeState(GameState.Playing);
        OnLevelStarted?.Invoke();
        
        Debug.Log("Level started!");
    }
    
    public void PauseGame()
    {
        if (currentState != GameState.Playing) return;
        
        ChangeState(GameState.Paused);
        Time.timeScale = 0f;
        
        Debug.Log("Game paused!");
    }
    
    public void ResumeGame()
    {
        if (currentState != GameState.Paused) return;
        
        ChangeState(GameState.Playing);
        Time.timeScale = 1f;
        
        Debug.Log("Game resumed!");
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int currentLevel = GetCurrentLevelNumber();
        int nextLevel = currentLevel + 1;
        
        if (nextLevel <= 10)
        {
            SceneManager.LoadScene($"Lv{nextLevel}");
        }
        else
        {
            LoadMainMenu();
        }
    }
    
    void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        
        GameState oldState = currentState;
        currentState = newState;
        
        OnGameStateChanged?.Invoke(newState);
        
        Debug.Log($"Game state changed: {oldState} -> {newState}");
    }
    
    void OnGameWon()
    {
        ChangeState(GameState.Won);
        OnLevelCompleted?.Invoke();
        
        // Save progress
        SaveLevelProgress();
        
        Debug.Log("Level completed! Victory achieved!");
    }
    
    void OnGameOver()
    {
        ChangeState(GameState.Lost);
        OnLevelFailed?.Invoke();
        
        Debug.Log("Level failed! Game over!");
    }
    
    void OnHealthChanged(int newHealth)
    {
        // Update UI or other systems when health changes
        Debug.Log($"Player health changed to: {newHealth}");
    }
    
    void OnPlayerDeath()
    {
        // Player death is handled by GameWinManager
        Debug.Log("Player death detected!");
    }
    
    void SaveLevelProgress()
    {
        int currentLevel = GetCurrentLevelNumber();
        
        // Save highest level unlocked
        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);
        if (currentLevel > highestLevel)
        {
            PlayerPrefs.SetInt("HighestLevel", currentLevel);
        }
        
        // Save current score if it's higher
        if (currentScore > highestScore)
        {
            highestScore = currentScore;
            PlayerPrefs.SetInt("HighestScore", highestScore);
        }
        
        // Mark level as completed
        PlayerPrefs.SetInt($"Level{currentLevel}Completed", 1);
        PlayerPrefs.Save();
        
        Debug.Log($"Level {currentLevel} progress saved!");
    }
    
    int GetCurrentLevelNumber()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Lv"))
        {
            string levelStr = sceneName.Substring(2);
            if (int.TryParse(levelStr, out int level))
            {
                return level;
            }
        }
        return 1;
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        Debug.Log($"Score added: +{points}. Total: {currentScore}");
    }
    
    public float GetLevelTime()
    {
        return currentLevelTime;
    }
    
    public string GetFormattedLevelTime()
    {
        int minutes = Mathf.FloorToInt(currentLevelTime / 60f);
        int seconds = Mathf.FloorToInt(currentLevelTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
    
    public bool IsGameActive()
    {
        return currentState == GameState.Playing;
    }
    
    public bool IsGamePaused()
    {
        return currentState == GameState.Paused;
    }
    
    public bool IsGameEnded()
    {
        return currentState == GameState.Won || currentState == GameState.Lost;
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
    
    [ContextMenu("Test Victory")]
    public void TestVictory()
    {
        if (gameWinManager != null)
        {
            gameWinManager.TriggerVictory("Test victory from GameStateManager!");
        }
    }
    
    [ContextMenu("Test Game Over")]
    public void TestGameOver()
    {
        if (gameWinManager != null)
        {
            gameWinManager.TriggerGameOver("Test game over from GameStateManager!");
        }
    }
    
    void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUILayout.BeginArea(new Rect(10, 220, 300, 200));
            GUILayout.Label("Game State Manager Debug");
            GUILayout.Label($"Current State: {currentState}");
            GUILayout.Label($"Level Time: {GetFormattedLevelTime()}");
            GUILayout.Label($"Score: {currentScore}");
            GUILayout.Label($"Health: {(playerHealth != null ? playerHealth.currentHealth.ToString() : "N/A")}");
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Pause Game"))
                PauseGame();
            if (GUILayout.Button("Resume Game"))
                ResumeGame();
            if (GUILayout.Button("Restart Level"))
                RestartLevel();
            if (GUILayout.Button("Load Main Menu"))
                LoadMainMenu();
            
            GUILayout.EndArea();
        }
    }
}
