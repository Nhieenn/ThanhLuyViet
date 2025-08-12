using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinManager : MonoBehaviour
{
    public static GameWinManager Instance;

    [Header("UI Panels")]
    public GameObject panel;            // Panel chung bạn muốn hiển thị cùng lúc với victoryPanel
    public GameObject victoryPanel;    // Bảng chiến thắng
    public GameObject gameOverPanel;   // Bảng thua

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip victoryClip;
    public AudioClip gameOverClip;

    [Header("Game State")]
    public bool isGameOver = false;
    public int totalWaves = 1;
    public int wavesCompleted = 0;

    [Header("Enemy Counting (per wave)")]
    public int enemiesInCurrentWave = 0;
    public int enemiesKilledThisWave = 0;

    [Header("Totals")]
    public int totalEnemiesSpawned = 0;
    public int totalEnemiesEliminated = 0;

    // Events
    public System.Action OnGameWon;
    public System.Action OnGameOver;
    public System.Action<int, int> OnWaveProgressChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (victoryPanel) victoryPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (panel) panel.SetActive(false);
    }

    public void SetEnemiesInCurrentWave(int count)
    {
        enemiesInCurrentWave = Mathf.Max(0, count);
        enemiesKilledThisWave = 0;
        totalEnemiesSpawned += enemiesInCurrentWave;

        Debug.Log($"[GameWinManager] Wave setup: {enemiesInCurrentWave} enemies");
        OnWaveProgressChanged?.Invoke(enemiesKilledThisWave, enemiesInCurrentWave);
    }

    public void EnemyEliminated()
    {
        if (isGameOver) return;

        enemiesKilledThisWave++;
        totalEnemiesEliminated++;

        Debug.Log($"[GameWinManager] Enemy killed: {enemiesKilledThisWave}/{enemiesInCurrentWave}");

        OnWaveProgressChanged?.Invoke(enemiesKilledThisWave, enemiesInCurrentWave);

        if (enemiesInCurrentWave > 0 && enemiesKilledThisWave >= enemiesInCurrentWave)
        {
            CompleteWave();
        }
    }

    public void CompleteWave()
    {
        if (isGameOver) return;

        wavesCompleted++;
        Debug.Log($"[GameWinManager] Wave completed: {wavesCompleted}/{totalWaves}");

        enemiesInCurrentWave = 0;
        enemiesKilledThisWave = 0;

        if (wavesCompleted >= totalWaves)
        {
            TriggerVictory("All waves cleared");
        }
    }

    public void TriggerVictory(string reason = "")
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log($"[GameWinManager] VICTORY: {reason}");
        OnGameWon?.Invoke();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Debug.Log("Victory panel activated");
        }
        else
        {
            Debug.LogWarning("Victory panel is not assigned!");
        }

        if (panel != null)
        {
            panel.SetActive(true);
            Debug.Log("Panel activated");
        }
        else
        {
            Debug.LogWarning("Panel is not assigned!");
        }

        PlayClip(victoryClip);
        Time.timeScale = 0f;
    }

    public void TriggerGameOver(string reason = "")
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log($"[GameWinManager] GAME OVER: {reason}");
        OnGameOver?.Invoke();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("GameOver panel activated");
        }
        else
        {
            Debug.LogWarning("GameOver panel is not assigned!");
        }

        PlayClip(gameOverClip);
        Time.timeScale = 0f;
    }

    void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            if (audioSource == null) Debug.LogWarning("AudioSource is null");
            if (clip == null) Debug.LogWarning("AudioClip is null");
        }
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
        int cur = GetCurrentLevelNumber();
        int next = cur + 1;
        if (next <= 20)
        {
            SceneManager.LoadScene($"Lv{next}");
        }
        else
        {
            LoadMainMenu();
        }
    }

    int GetCurrentLevelNumber()
    {
        string nm = SceneManager.GetActiveScene().name;
        if (nm.StartsWith("Lv"))
        {
            string num = nm.Substring(2);
            if (int.TryParse(num, out int lv)) return lv;
        }
        return 1;
    }
}