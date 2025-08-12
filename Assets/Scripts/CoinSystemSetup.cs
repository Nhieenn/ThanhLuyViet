using UnityEngine;
using UnityEngine.UI;
using TMPro; // Thêm namespace cho TextMeshPro

public class CoinSystemSetup : MonoBehaviour
{
    [Header("Setup Settings")]
    [SerializeField] private bool autoSetupOnStart = true;
    [SerializeField] private GameObject coinManagerPrefab;
    
    [Header("UI Setup")]
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private Vector2 coinUIPosition = new Vector2(100, 50);
    [SerializeField] private int startingCoins = 500;
    
    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupCoinSystem();
        }
    }
    
    [ContextMenu("Setup Coin System")]
    public void SetupCoinSystem()
    {
        Debug.Log("Setting up Coin System for Tower Defense...");
        
        // 1. Setup CoinManager
        SetupCoinManager();
        
        // 2. Setup Coin UI
        SetupCoinUI();
        
        Debug.Log("Coin System setup complete for Tower Defense!");
    }
    
    private void SetupCoinManager()
    {
        // Kiểm tra xem đã có CoinManager chưa
        if (CoinManager.Instance != null)
        {
            Debug.Log("CoinManager already exists in scene");
            return;
        }
        
        // Tạo CoinManager
        GameObject coinManagerObj;
        if (coinManagerPrefab != null)
        {
            coinManagerObj = Instantiate(coinManagerPrefab);
        }
        else
        {
            coinManagerObj = new GameObject("CoinManager");
            coinManagerObj.AddComponent<CoinManager>();
        }
        
        // Cấu hình CoinManager
        CoinManager coinManager = coinManagerObj.GetComponent<CoinManager>();
        if (coinManager != null)
        {
            // Có thể cấu hình thêm ở đây nếu cần
            Debug.Log("CoinManager created successfully for Tower Defense");
        }
    }
    
    private void SetupCoinUI()
    {
        // Tìm Canvas
        if (targetCanvas == null)
        {
            targetCanvas = FindObjectOfType<Canvas>();
            if (targetCanvas == null)
            {
                Debug.LogWarning("No Canvas found in scene! Cannot setup Coin UI.");
                return;
            }
        }
        
        // Kiểm tra xem đã có Coin UI chưa
        if (FindObjectOfType<CoinUI>() != null)
        {
            Debug.Log("Coin UI already exists in scene");
            return;
        }
        
        // Tạo Coin UI
        GameObject coinUIObj = new GameObject("CoinUI");
        coinUIObj.transform.SetParent(targetCanvas.transform, false);
        
        // Thêm RectTransform
        RectTransform rectTransform = coinUIObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = coinUIPosition;
        rectTransform.sizeDelta = new Vector2(200, 50);
        
        // Thêm Image background (tùy chọn)
        Image background = coinUIObj.AddComponent<Image>();
        background.color = new Color(0, 0, 0, 0.5f);
        
        // Tạo Coin Icon
        GameObject coinIcon = new GameObject("CoinIcon");
        coinIcon.transform.SetParent(coinUIObj.transform, false);
        
        Image iconImage = coinIcon.AddComponent<Image>();
        iconImage.color = Color.yellow;
        
        RectTransform iconRect = coinIcon.GetComponent<RectTransform>();
        iconRect.anchoredPosition = new Vector2(-80, 0);
        iconRect.sizeDelta = new Vector2(30, 30);
        
        // Tạo Coin Text với TextMeshPro
        GameObject coinText = new GameObject("CoinText");
        coinText.transform.SetParent(coinUIObj.transform, false);
        
        TextMeshProUGUI text = coinText.AddComponent<TextMeshProUGUI>(); // Thay đổi từ Text sang TextMeshProUGUI
        text.text = startingCoins.ToString();
        text.fontSize = 24;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center; // Thay đổi alignment cho TextMeshPro
        
        RectTransform textRect = coinText.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(0, 0);
        textRect.sizeDelta = new Vector2(100, 30);
        
        // Thêm CoinUI script
        CoinUI coinUI = coinUIObj.AddComponent<CoinUI>();
        
        Debug.Log("Coin UI created successfully with TextMeshPro for Tower Defense");
    }
    
    [ContextMenu("Test Coin System")]
    public void TestCoinSystem()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoins(100);
            Debug.Log("Test: Added 100 coins");
        }
        else
        {
            Debug.LogWarning("CoinManager not found! Please setup Coin System first.");
        }
    }
}
