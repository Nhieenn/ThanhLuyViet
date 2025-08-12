using UnityEngine;
using UnityEngine.UI;
using TMPro; // Thêm namespace cho TextMeshPro

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinText; // Thay đổi từ Text sang TextMeshProUGUI
    [SerializeField] private GameObject coinIcon;
    
    [Header("Coin Settings")]
    [SerializeField] private int startingCoins = 500;
    [SerializeField] private bool forceStartingCoins = false; // Thêm option để force dùng startingCoins
    
    [Header("Audio")]
    [SerializeField] private AudioClip coinCollectSound;
    
    private int currentCoins;
    
    public int CurrentCoins => currentCoins;
    
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
        InitializeCoins();
        UpdateCoinUI();
    }
    
    private void InitializeCoins()
    {
        Debug.Log($"=== INITIALIZE COINS ===");
        Debug.Log($"Starting Coins setting: {startingCoins}");
        
        // Luôn reset về startingCoins mỗi lần Play/Restart
        currentCoins = startingCoins;
        PlayerPrefs.SetInt("PlayerCoins", currentCoins);
        PlayerPrefs.Save();
        Debug.Log($"✅ Reset to starting coins: {startingCoins} (every play/restart)");
        
        Debug.Log($"Final current coins: {currentCoins}");
    }
    
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        PlayerPrefs.SetInt("PlayerCoins", currentCoins);
        PlayerPrefs.Save();
        
        UpdateCoinUI();
        
        // Play sound nếu có
        if (coinCollectSound != null)
        {
            AudioSource.PlayClipAtPoint(coinCollectSound, Camera.main.transform.position);
        }
        
        // Trigger UI animation
        TriggerCoinUIAnimation(true, amount);
        
        Debug.Log($"Added {amount} coins. Total: {currentCoins}");
    }
    
    public bool TrySpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            PlayerPrefs.SetInt("PlayerCoins", currentCoins);
            PlayerPrefs.Save();
            
            UpdateCoinUI();
            
            // Trigger UI animation
            TriggerCoinUIAnimation(false, amount);
            
            Debug.Log($"Spent {amount} coins. Remaining: {currentCoins}");
            return true;
        }
        
        Debug.LogWarning($"❌ Not enough coins! Required: {amount}, Available: {currentCoins}");
        return false;
    }
    
    // Kiểm tra xem có đủ tiền để mua/nâng cấp không
    public bool CanAfford(int amount)
    {
        bool canAfford = currentCoins >= amount;
        if (!canAfford)
        {
            Debug.LogWarning($"❌ Cannot afford! Required: {amount}, Available: {currentCoins}");
        }
        return canAfford;
    }
    
    // Lấy số tiền còn thiếu
    public int GetMissingAmount(int requiredAmount)
    {
        return Mathf.Max(0, requiredAmount - currentCoins);
    }
    
    // Phương thức mới: Thêm coin trực tiếp khi enemy chết (cho Tower Defense)
    public void AddCoinsFromEnemy(int coinAmount)
    {
        AddCoins(coinAmount);
        Debug.Log($"Enemy defeated! Earned {coinAmount} coins. Total: {currentCoins}");
    }
    
    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = currentCoins.ToString();
        }
    }
    
    private void TriggerCoinUIAnimation(bool isAdding, int amount)
    {
        // Tìm tất cả CoinUI trong scene để trigger animation
        CoinUI[] coinUIs = FindObjectsOfType<CoinUI>();
        foreach (CoinUI coinUI in coinUIs)
        {
            if (isAdding)
            {
                coinUI.OnCoinsAdded(amount);
            }
            else
            {
                coinUI.OnCoinsSpent(amount);
            }
        }
    }
    
    public void SetCoinUI(TextMeshProUGUI coinTextUI, GameObject coinIconUI) // Thay đổi parameter type
    {
        coinText = coinTextUI;
        coinIcon = coinIconUI;
        UpdateCoinUI();
    }
    
    // Reset coins (cho testing)
    [ContextMenu("Reset Coins")]
    public void ResetCoins()
    {
        currentCoins = startingCoins;
        PlayerPrefs.SetInt("PlayerCoins", currentCoins);
        PlayerPrefs.Save();
        UpdateCoinUI();
        Debug.Log("Coins reset to starting amount");
    }
    
    // Clear PlayerPrefs và force reset (cho development)
    [ContextMenu("Clear PlayerPrefs and Reset")]
    public void ClearPlayerPrefsAndReset()
    {
        PlayerPrefs.DeleteKey("PlayerCoins");
        PlayerPrefs.Save();
        currentCoins = startingCoins;
        UpdateCoinUI();
        Debug.Log($"Cleared PlayerPrefs and reset to starting coins: {startingCoins}");
    }
    
    // Force reset về starting coins (bỏ qua PlayerPrefs)
    [ContextMenu("Force Starting Coins")]
    public void ForceStartingCoins()
    {
        forceStartingCoins = true;
        InitializeCoins();
        UpdateCoinUI();
        Debug.Log($"Forced to starting coins: {startingCoins}");
    }
    
    // Kiểm tra và sửa vấn đề startingCoins ngay lập tức
    [ContextMenu("Check and Fix Starting Coins")]
    public void CheckAndFixStartingCoins()
    {
        Debug.Log("=== CHECKING STARTING COINS ISSUE ===");
        
        // Kiểm tra PlayerPrefs
        int savedCoins = PlayerPrefs.GetInt("PlayerCoins", -999);
        Debug.Log($"PlayerPrefs value: {savedCoins}");
        Debug.Log($"Starting coins setting: {startingCoins}");
        Debug.Log($"Current coins: {currentCoins}");
        
        if (savedCoins != startingCoins && savedCoins != -999)
        {
            Debug.LogWarning($"⚠️ MISMATCH DETECTED!");
            Debug.LogWarning($"PlayerPrefs: {savedCoins} | Starting: {startingCoins}");
            
            // Tự động sửa
            Debug.Log("🔧 Auto-fixing...");
            ClearPlayerPrefsAndReset();
        }
        else
        {
            Debug.Log("✅ Starting coins are correct!");
        }
    }
    
    // Add coins (cho testing)
    [ContextMenu("Add 100 Coins")]
    public void AddTestCoins()
    {
        AddCoins(100);
    }
}
