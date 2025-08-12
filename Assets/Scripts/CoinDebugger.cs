using UnityEngine;

public class CoinDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private bool autoCheckOnStart = true;
    
    void Start()
    {
        if (autoCheckOnStart)
        {
            CheckCoinStatus();
        }
    }
    
    void Update()
    {
        // Test key để sửa vấn đề startingCoins
        if (Input.GetKeyDown(KeyCode.X))
        {
            FixStartingCoinsIssue();
        }
    }
    
    [ContextMenu("Check Coin Status")]
    public void CheckCoinStatus()
    {
        Debug.Log("=== COIN DEBUGGER ===");
        
        if (CoinManager.Instance != null)
        {
            var coinManager = CoinManager.Instance;
            
            // Kiểm tra PlayerPrefs
            int playerPrefsValue = PlayerPrefs.GetInt("PlayerCoins", -999);
            Debug.Log($"PlayerPrefs value: {playerPrefsValue}");
            
            // Kiểm tra current coins
            Debug.Log($"Current coins: {coinManager.CurrentCoins}");
            
            // Kiểm tra starting coins (qua reflection vì là private)
            var startingCoinsField = typeof(CoinManager).GetField("startingCoins", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (startingCoinsField != null)
            {
                int startingCoins = (int)startingCoinsField.GetValue(coinManager);
                Debug.Log($"Starting coins setting: {startingCoins}");
                
                // So sánh
                if (coinManager.CurrentCoins != startingCoins)
                {
                    Debug.LogWarning($"⚠️ MISMATCH: Current ({coinManager.CurrentCoins}) != Starting ({startingCoins})");
                    Debug.LogWarning("This means PlayerPrefs has a saved value from previous play session");
                }
                else
                {
                    Debug.Log("✅ Current coins match starting coins");
                }
            }
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Fix Starting Coins Issue")]
    public void FixStartingCoinsIssue()
    {
        Debug.Log("=== FIXING STARTING COINS ISSUE ===");
        
        if (CoinManager.Instance != null)
        {
            // Clear PlayerPrefs
            PlayerPrefs.DeleteKey("PlayerCoins");
            PlayerPrefs.Save();
            Debug.Log("✅ Cleared PlayerPrefs");
            
            // Force reset
            CoinManager.Instance.ClearPlayerPrefsAndReset();
            Debug.Log("✅ Forced reset to starting coins");
            
            // Check again
            CheckCoinStatus();
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    void OnGUI()
    {
        if (showDebugInfo)
        {
            GUILayout.BeginArea(new Rect(Screen.width - 320, 10, 300, 180));
            GUILayout.Label("=== COIN DEBUGGER ===");
            
            if (CoinManager.Instance != null)
            {
                GUILayout.Label($"Current: {CoinManager.Instance.CurrentCoins}");
                GUILayout.Label($"PlayerPrefs: {PlayerPrefs.GetInt("PlayerCoins", -999)}");
                GUILayout.Label("Press X to Fix Issue");
                
                if (GUILayout.Button("Check Status"))
                {
                    CheckCoinStatus();
                }
                
                if (GUILayout.Button("Fix Issue"))
                {
                    FixStartingCoinsIssue();
                }
            }
            else
            {
                GUILayout.Label("CoinManager not found!");
            }
            
            GUILayout.EndArea();
        }
    }
}
