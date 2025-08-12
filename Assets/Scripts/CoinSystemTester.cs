using UnityEngine;

public class CoinSystemTester : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private int testEnemyReward = 25;
    [SerializeField] private int testTowerCost = 100;
    [SerializeField] private int testUpgradeCost = 50;
    [SerializeField] private int testSellValue = 75;
    
    [Header("Debug Info")]
    [SerializeField] private bool showDebugInfo = true;
    
    void Start()
    {
        if (showDebugInfo)
        {
            Debug.Log("=== COIN SYSTEM TESTER STARTED ===");
            Debug.Log($"Current Coins: {CoinManager.Instance?.CurrentCoins ?? 0}");
        }
    }
    
    void Update()
    {
        // Test keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestEnemyKill();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestBuyTower();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TestUpgradeTower();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TestSellTower();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            TestResetCoins();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            TestClearPlayerPrefs();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            TestForceStartingCoins();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            TestCanAfford();
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            TestBuyTowerLogic();
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestTowerDataCosts();
        }
    }
    
    [ContextMenu("Test Enemy Kill")]
    public void TestEnemyKill()
    {
        if (CoinManager.Instance != null)
        {
            int beforeCoins = CoinManager.Instance.CurrentCoins;
            CoinManager.Instance.AddCoinsFromEnemy(testEnemyReward);
            int afterCoins = CoinManager.Instance.CurrentCoins;
            
            Debug.Log($"=== TEST ENEMY KILL ===");
            Debug.Log($"Before: {beforeCoins} coins");
            Debug.Log($"Enemy Reward: {testEnemyReward} coins");
            Debug.Log($"After: {afterCoins} coins");
            Debug.Log($"Result: {(afterCoins == beforeCoins + testEnemyReward ? "✅ PASS" : "❌ FAIL")}");
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test Buy Tower")]
    public void TestBuyTower()
    {
        if (CoinManager.Instance != null)
        {
            int beforeCoins = CoinManager.Instance.CurrentCoins;
            bool success = CoinManager.Instance.TrySpendCoins(testTowerCost);
            int afterCoins = CoinManager.Instance.CurrentCoins;
            
            Debug.Log($"=== TEST BUY TOWER ===");
            Debug.Log($"Before: {beforeCoins} coins");
            Debug.Log($"Tower Cost: {testTowerCost} coins");
            Debug.Log($"Success: {success}");
            Debug.Log($"After: {afterCoins} coins");
            
            if (success)
            {
                Debug.Log($"Result: {(afterCoins == beforeCoins - testTowerCost ? "✅ PASS" : "❌ FAIL")}");
            }
            else
            {
                Debug.Log($"Result: {(afterCoins == beforeCoins ? "✅ PASS (Not enough coins)" : "❌ FAIL")}");
            }
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test Upgrade Tower")]
    public void TestUpgradeTower()
    {
        if (CoinManager.Instance != null)
        {
            int beforeCoins = CoinManager.Instance.CurrentCoins;
            bool success = CoinManager.Instance.TrySpendCoins(testUpgradeCost);
            int afterCoins = CoinManager.Instance.CurrentCoins;
            
            Debug.Log($"=== TEST UPGRADE TOWER ===");
            Debug.Log($"Before: {beforeCoins} coins");
            Debug.Log($"Upgrade Cost: {testUpgradeCost} coins");
            Debug.Log($"Success: {success}");
            Debug.Log($"After: {afterCoins} coins");
            
            if (success)
            {
                Debug.Log($"Result: {(afterCoins == beforeCoins - testUpgradeCost ? "✅ PASS" : "❌ FAIL")}");
            }
            else
            {
                Debug.Log($"Result: {(afterCoins == beforeCoins ? "✅ PASS (Not enough coins)" : "❌ FAIL")}");
            }
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test Sell Tower")]
    public void TestSellTower()
    {
        if (CoinManager.Instance != null)
        {
            int beforeCoins = CoinManager.Instance.CurrentCoins;
            CoinManager.Instance.AddCoins(testSellValue);
            int afterCoins = CoinManager.Instance.CurrentCoins;
            
            Debug.Log($"=== TEST SELL TOWER ===");
            Debug.Log($"Before: {beforeCoins} coins");
            Debug.Log($"Sell Value: {testSellValue} coins");
            Debug.Log($"After: {afterCoins} coins");
            Debug.Log($"Result: {(afterCoins == beforeCoins + testSellValue ? "✅ PASS" : "❌ FAIL")}");
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test Reset Coins")]
    public void TestResetCoins()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.ResetCoins();
            Debug.Log($"=== TEST RESET COINS ===");
            Debug.Log($"Coins after reset: {CoinManager.Instance.CurrentCoins}");
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test Clear PlayerPrefs")]
    public void TestClearPlayerPrefs()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.ClearPlayerPrefsAndReset();
            Debug.Log($"=== TEST CLEAR PLAYERPREFS ===");
            Debug.Log($"Coins after clear: {CoinManager.Instance.CurrentCoins}");
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test Force Starting Coins")]
    public void TestForceStartingCoins()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.ForceStartingCoins();
            Debug.Log($"=== TEST FORCE STARTING COINS ===");
            Debug.Log($"Coins after force: {CoinManager.Instance.CurrentCoins}");
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test Can Afford")]
    public void TestCanAfford()
    {
        if (CoinManager.Instance != null)
        {
            int currentCoins = CoinManager.Instance.CurrentCoins;
            int testCost = 150;
            
            Debug.Log($"=== TEST CAN AFFORD ===");
            Debug.Log($"Current coins: {currentCoins}");
            Debug.Log($"Test cost: {testCost}");
            
            bool canAfford = CoinManager.Instance.CanAfford(testCost);
            Debug.Log($"Can afford: {canAfford}");
            
            if (!canAfford)
            {
                int missing = CoinManager.Instance.GetMissingAmount(testCost);
                Debug.Log($"Missing amount: {missing}");
            }
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test Buy Tower Logic")]
    public void TestBuyTowerLogic()
    {
        if (CoinManager.Instance != null)
        {
            int currentCoins = CoinManager.Instance.CurrentCoins;
            int testCost = 100;
            
            Debug.Log($"=== TEST BUY TOWER LOGIC ===");
            Debug.Log($"Current coins: {currentCoins}");
            Debug.Log($"Test cost: {testCost}");
            
            // Test CanAfford
            bool canAfford = CoinManager.Instance.CanAfford(testCost);
            Debug.Log($"Can afford: {canAfford}");
            
            if (canAfford)
            {
                // Test TrySpendCoins
                bool success = CoinManager.Instance.TrySpendCoins(testCost);
                Debug.Log($"TrySpendCoins success: {success}");
                
                if (success)
                {
                    Debug.Log($"✅ Tower purchase successful! Remaining coins: {CoinManager.Instance.CurrentCoins}");
                }
                else
                {
                    Debug.LogError("❌ TrySpendCoins failed even though CanAfford returned true!");
                }
            }
            else
            {
                int missing = CoinManager.Instance.GetMissingAmount(testCost);
                Debug.Log($"❌ Cannot buy tower! Missing {missing} coins");
            }
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }
    
    [ContextMenu("Test TowerData Costs")]
    public void TestTowerDataCosts()
    {
        Debug.Log("=== TEST TOWERDATA COSTS ===");
        
        // Find TowerBuildMenu in scene
        TowerBuildMenu buildMenu = FindObjectOfType<TowerBuildMenu>();
        if (buildMenu != null)
        {
            Debug.Log("✅ TowerBuildMenu found in scene");
            
            // Test each tower prefab
            if (buildMenu.tower1Prefab != null)
            {
                Tower tower1 = buildMenu.tower1Prefab.GetComponent<Tower>();
                if (tower1 != null && tower1.towerData != null)
                {
                    Debug.Log($"✅ Tower1: {tower1.towerData.towerName} - Cost: {tower1.towerData.buildCost}");
                }
                else
                {
                    Debug.LogError("❌ Tower1: TowerData not found");
                }
            }
            
            if (buildMenu.tower2Prefab != null)
            {
                Tower tower2 = buildMenu.tower2Prefab.GetComponent<Tower>();
                if (tower2 != null && tower2.towerData != null)
                {
                    Debug.Log($"✅ Tower2: {tower2.towerData.towerName} - Cost: {tower2.towerData.buildCost}");
                }
                else
                {
                    Debug.LogError("❌ Tower2: TowerData not found");
                }
            }
            
            if (buildMenu.tower3Prefab != null)
            {
                Tower tower3 = buildMenu.tower3Prefab.GetComponent<Tower>();
                if (tower3 != null && tower3.towerData != null)
                {
                    Debug.Log($"✅ Tower3: {tower3.towerData.towerName} - Cost: {tower3.towerData.buildCost}");
                }
                else
                {
                    Debug.LogError("❌ Tower3: TowerData not found");
                }
            }
        }
        else
        {
            Debug.LogWarning("⚠️ TowerBuildMenu not found in scene (normal if menu not open)");
        }
    }
    
    [ContextMenu("Show Current Status")]
    public void ShowCurrentStatus()
    {
        if (CoinManager.Instance != null)
        {
            Debug.Log($"=== CURRENT COIN STATUS ===");
            Debug.Log($"Current Coins: {CoinManager.Instance.CurrentCoins}");
            Debug.Log($"CoinManager Instance: {(CoinManager.Instance != null ? "✅ Found" : "❌ Not Found")}");
            
            // Kiểm tra UI
            var coinUIs = FindObjectsOfType<CoinUI>();
            Debug.Log($"CoinUI Components: {coinUIs.Length}");
            
            foreach (var coinUI in coinUIs)
            {
                Debug.Log($"- CoinUI: {coinUI.name}");
            }
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
            GUILayout.BeginArea(new Rect(10, 10, 300, 250));
            GUILayout.Label("=== COIN SYSTEM TESTER ===");
            GUILayout.Label($"Current Coins: {CoinManager.Instance?.CurrentCoins ?? 0}");
            GUILayout.Space(10);
            
            GUILayout.Label("Test Keys:");
            GUILayout.Label("1 - Test Enemy Kill");
            GUILayout.Label("2 - Test Buy Tower");
            GUILayout.Label("3 - Test Upgrade Tower");
            GUILayout.Label("4 - Test Sell Tower");
            GUILayout.Label("R - Reset Coins");
            GUILayout.Label("C - Clear PlayerPrefs");
            GUILayout.Label("F - Force Starting Coins");
            GUILayout.Label("A - Test Can Afford");
            GUILayout.Label("B - Test Buy Tower Logic");
            GUILayout.Label("T - Test TowerData Costs");
            
            GUILayout.EndArea();
        }
    }
}
