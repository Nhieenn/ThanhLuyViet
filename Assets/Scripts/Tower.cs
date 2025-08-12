using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject actionMenuPrefab;
    public GameObject buildAreaPrefab;
    public TowerData towerData; // Thêm trường này
    
    private GameObject currentMenuInstance;
    private int currentLevel = 0; // Level bắt đầu từ 0
    private SpriteRenderer spriteRenderer;
    
    // Property để truy cập currentLevel từ bên ngoài
    public int CurrentLevel => currentLevel;

    void OnMouseDown()
    {
        // Kiểm tra nếu game đang pause hoặc setting thì không cho mở panel
        if (Time.timeScale == 0)
        {
            Debug.Log("Game is paused/setting is open, cannot open tower action menu");
            return;
        }
        
        if (currentMenuInstance == null)
        {
            // Tạm dừng camera movement khi mở menu
            Camera_move.DisableCameraMovement();
            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Debug.Log("Tower clicked! Screen position: " + screenPos);
            
            // Đảm bảo vị trí nằm trong màn hình
            screenPos.x = Mathf.Clamp(screenPos.x, 100, Screen.width - 100);
            screenPos.y = Mathf.Clamp(screenPos.y, 100, Screen.height - 100);
            Debug.Log("Clamped tower screen position: " + screenPos);
            
            // Tìm Canvas chính xác
            Canvas targetCanvas = FindCanvasForUI();
            if (targetCanvas == null)
            {
                Debug.LogError("No suitable Canvas found for tower menu!");
                return;
            }
            
            currentMenuInstance = Instantiate(actionMenuPrefab, targetCanvas.transform);
            currentMenuInstance.transform.position = screenPos;
            
            Debug.Log("Tower action menu created at: " + currentMenuInstance.transform.position);
            Debug.Log("Using Canvas: " + targetCanvas.name);
            
            currentMenuInstance.GetComponent<TowerActionMenu>().SetTargetTower(this);
        }
    }

    // Hàm tìm Canvas phù hợp (copy từ BuildableArea)
    private Canvas FindCanvasForUI()
    {
        // Ưu tiên 1: Tìm Canvas có tag "UI"
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.CompareTag("UI"))
            {
                Debug.Log("Found Canvas with tag 'UI': " + canvas.name);
                return canvas;
            }
        }
        
        // Ưu tiên 2: Tìm Canvas có GraphicRaycaster (Canvas chính)
        foreach (Canvas canvas in canvases)
        {
            if (canvas.GetComponent<UnityEngine.UI.GraphicRaycaster>() != null)
            {
                Debug.Log("Found Canvas with GraphicRaycaster: " + canvas.name);
                return canvas;
            }
        }
        
        // Ưu tiên 3: Tìm Canvas đầu tiên có RenderMode = Screen Space - Overlay
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                Debug.Log("Found Canvas with ScreenSpaceOverlay: " + canvas.name);
                return canvas;
            }
        }
        
        // Fallback: Canvas đầu tiên tìm được
        if (canvases.Length > 0)
        {
            Debug.Log("Using first Canvas found: " + canvases[0].name);
            return canvases[0];
        }
        
        return null;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Tower prefab must have SpriteRenderer component!");
        }
        
        // Set sprite ban đầu
        if (towerData != null && towerData.levels.Length > 0)
        {
            spriteRenderer.sprite = towerData.levels[0].sprite;
            spriteRenderer.color = towerData.towerColor;
        }
    }

    public void Upgrade()
    {
        if (towerData == null)
        {
            Debug.LogError("TowerData not assigned!");
            return;
        }
        
        if (currentLevel >= towerData.levels.Length - 1)
        {
            Debug.Log("Tower already at max level!");
            return;
        }
        
        // Kiểm tra tiền để nâng cấp
        int upgradeCost = towerData.levels[currentLevel + 1].upgradeCost;
        
        if (CoinManager.Instance != null)
        {
            if (CoinManager.Instance.CanAfford(upgradeCost))
            {
                if (CoinManager.Instance.TrySpendCoins(upgradeCost))
                {
                    currentLevel++;
                    Debug.Log($"✅ Upgraded {towerData.towerName} to level {currentLevel}! Cost: {upgradeCost}");
                    
                    // Thay đổi sprite
                    if (currentLevel < towerData.levels.Length)
                    {
                        spriteRenderer.sprite = towerData.levels[currentLevel].sprite;
                        
                        // Hiệu ứng nâng cấp
                        if (towerData.upgradeEffectPrefab != null)
                        {
                            Instantiate(towerData.upgradeEffectPrefab, transform.position, Quaternion.identity);
                        }
                        
                        // Âm thanh nâng cấp
                        if (towerData.upgradeSound != null)
                        {
                            AudioSource.PlayClipAtPoint(towerData.upgradeSound, transform.position);
                        }
                    }
                    
                    // Cập nhật shooting stats
                    TowerShooter shooter = GetComponent<TowerShooter>();
                    if (shooter != null)
                    {
                        shooter.OnTowerUpgraded();
                    }
                    
                    Debug.Log("New stats - Damage: " + towerData.levels[currentLevel].damage + 
                              ", Range: " + towerData.levels[currentLevel].range + 
                              ", Fire Rate: " + towerData.levels[currentLevel].fireRate);
                }
            }
            else
            {
                int missing = CoinManager.Instance.GetMissingAmount(upgradeCost);
                Debug.LogWarning($"❌ Cannot upgrade tower! Missing {missing} coins. Required: {upgradeCost}, Available: {CoinManager.Instance.CurrentCoins}");
            }
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }

    public void DestroyTower()
    {
        Debug.Log("Destroy tower!");
        
        // Trả lại coin khi bán tháp
        if (towerData != null && currentLevel < towerData.levels.Length)
        {
            int sellValue = towerData.levels[currentLevel].sellValue;
            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoins(sellValue);
                Debug.Log($"Tower sold for {sellValue} coins!");
            }
        }
        
        CloseMenu(); // Đóng menu trước khi xóa tháp
        Destroy(gameObject);
    }

    public void CloseMenu()
    {
        if (currentMenuInstance != null)
        {
            Destroy(currentMenuInstance);
            currentMenuInstance = null;
            
            // Bật lại camera movement
            Camera_move.EnableCameraMovement();
        }
    }

    public void DestroyTowerWithoutMenu()
    {
        Debug.Log("Destroy tower!");
        
        // Trả lại coin khi bán tháp
        if (towerData != null && currentLevel < towerData.levels.Length)
        {
            int sellValue = towerData.levels[currentLevel].sellValue;
            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoins(sellValue);
                Debug.Log($"Tower sold for {sellValue} coins!");
            }
        }
        
        // Spawn lại vùng build tại vị trí tháp
        GameObject buildArea = Instantiate(buildAreaPrefab, transform.position, Quaternion.identity);
        
        // Bật lại camera movement
        Camera_move.EnableCameraMovement();
        
        // Xóa tháp
        Destroy(gameObject);
    }
} 