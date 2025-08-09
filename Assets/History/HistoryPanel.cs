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

    private void Start()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    public void ShowHistory()
    {
        if (HistoryData.Instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            var info = HistoryData.Instance.GetHistoryForScene(sceneName);
            
            if (battleNameText != null)
                battleNameText.text = info.battleName;
            if (forceComparisonText != null)
                forceComparisonText.text = info.forceComparison;
            if (battleMeaningText != null)
                battleMeaningText.text = info.battleMeaning;
        }
        
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
            Time.timeScale = 0;
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
} 