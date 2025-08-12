using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HistoryPanel : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text battleNameText;
    public TMP_Text forceComparisonText;
    public TMP_Text battleMeaningText;
    public GameObject panelRoot;
    
    [Header("Game Over Buttons")]
    public Button restartButton;
    public Button menuButton;
    
    private void Start()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
        
        // Setup button listeners
        SetupButtons();
    }
    
    private void SetupButtons()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(GoToMainMenu);
        }
    }
    
    public void ShowHistory()
    {
        Debug.Log("=== SHOW HISTORY CALLED ===");
        
        if (HistoryData.Instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            Debug.Log("Current Scene Name: " + sceneName);
            
            var info = HistoryData.Instance.GetHistoryForScene(sceneName);
            Debug.Log("History Info for " + sceneName + ":");
            Debug.Log("- Battle Name: " + info.battleName);
            Debug.Log("- Force Comparison: " + info.forceComparison);
            Debug.Log("- Battle Meaning: " + info.battleMeaning);
            
            if (battleNameText != null)
            {
                battleNameText.text = info.battleName;
                Debug.Log("Battle Name Text updated: " + battleNameText.text);
            }
            else
            {
                Debug.LogWarning("battleNameText is null!");
            }
            
            if (forceComparisonText != null)
            {
                forceComparisonText.text = info.forceComparison;
                Debug.Log("Force Comparison Text updated: " + forceComparisonText.text);
            }
            else
            {
                Debug.LogWarning("forceComparisonText is null!");
            }
            
            if (battleMeaningText != null)
            {
                battleMeaningText.text = info.battleMeaning;
                Debug.Log("Battle Meaning Text updated: " + battleMeaningText.text);
            }
            else
            {
                Debug.LogWarning("battleMeaningText is null!");
            }
        }
        else
        {
            Debug.LogError("HistoryData.Instance is null!");
        }
        
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("History Panel activated and game paused");
        }
        else
        {
            Debug.LogWarning("panelRoot is null!");
        }
    }
    
    public void HideHistory()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
            Time.timeScale = 1;
        }
    }
    
    // Game Over Functions
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart Game");
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0); // Load scene đầu tiên (Main Menu)
        Debug.Log("Go to Main Menu");
    }
} 