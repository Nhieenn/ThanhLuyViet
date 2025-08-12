using UnityEngine;

public class BuildableArea : MonoBehaviour
{
    public GameObject towerBuildMenuPrefab;
    private GameObject currentMenuInstance;

    void OnMouseDown()
    {
       Debug.Log("Buildable area clicked");
       
       // Kiểm tra nếu game đang pause hoặc setting thì không cho mở panel
       if (Time.timeScale == 0)
       {
           Debug.Log("Game is paused/setting is open, cannot open tower build menu");
           return;
       }
       
       // Debug: Kiểm tra các thành phần cần thiết
       if (towerBuildMenuPrefab == null)
       {
           Debug.LogError("towerBuildMenuPrefab is null! Please assign it in inspector.");
           return;
       }
       
       // Tìm Canvas chính xác hơn
       Canvas targetCanvas = FindCanvasForUI();
       if (targetCanvas == null)
       {
           Debug.LogError("No suitable Canvas found in scene! Please add a Canvas with GraphicRaycaster.");
           return;
       }
       
       Camera mainCamera = Camera.main;
       if (mainCamera == null)
       {
           Debug.LogError("No Camera with tag 'MainCamera' found! Please check camera tag.");
           return;
       }
       
       if (currentMenuInstance == null)
       {
           // Tạm dừng camera movement khi mở menu
           Camera_move.DisableCameraMovement();
           
           Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
           Debug.Log("Screen position: " + screenPos);
           Debug.Log("Screen width: " + Screen.width + ", height: " + Screen.height);
           
           // Đảm bảo vị trí nằm trong màn hình
           screenPos.x = Mathf.Clamp(screenPos.x, 100, Screen.width - 100);
           screenPos.y = Mathf.Clamp(screenPos.y, 100, Screen.height - 100);
           Debug.Log("Clamped screen position: " + screenPos);
           
           currentMenuInstance = Instantiate(towerBuildMenuPrefab, targetCanvas.transform);
           currentMenuInstance.transform.position = screenPos;
           
           // Debug: Kiểm tra vị trí menu sau khi tạo
           Debug.Log("Menu position after creation: " + currentMenuInstance.transform.position);
           Debug.Log("Menu local position: " + currentMenuInstance.transform.localPosition);
           Debug.Log("Using Canvas: " + targetCanvas.name);
           
           // SetBuildableArea sẽ tự động đóng panel cũ nếu có
           currentMenuInstance.GetComponent<TowerBuildMenu>().SetBuildableArea(this);
           
           Debug.Log("Tower build menu created successfully!");
       }
    }

    // Hàm tìm Canvas phù hợp
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

    public void BuildTower(GameObject towerPrefab, int cost)
    {
        // Kiểm tra tiền trước khi xây tower
        if (CoinManager.Instance != null)
        {
            if (CoinManager.Instance.CanAfford(cost))
            {
                if (CoinManager.Instance.TrySpendCoins(cost))
                {
                    Instantiate(towerPrefab, transform.position, Quaternion.identity);
                    Debug.Log($"✅ Tower built successfully! Cost: {cost}");
                    
                    // Đóng menu và xử lý cleanup
                    CloseMenu();
                    
                    // Xóa vùng build sau khi xây tháp thành công
                    Destroy(gameObject);
                }
            }
            else
            {
                int missing = CoinManager.Instance.GetMissingAmount(cost);
                Debug.LogWarning($"❌ Cannot build tower! Missing {missing} coins. Required: {cost}, Available: {CoinManager.Instance.CurrentCoins}");
            }
        }
        else
        {
            Debug.LogError("CoinManager not found!");
        }
    }

    public void CloseMenu()
    {
        if (currentMenuInstance != null)
        {
            Destroy(currentMenuInstance);
            currentMenuInstance = null;
            
            // Bật lại camera movement khi đóng menu
            Camera_move.EnableCameraMovement();
        }
    }
} 