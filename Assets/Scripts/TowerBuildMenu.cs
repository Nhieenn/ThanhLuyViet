using UnityEngine;
using UnityEngine.UI;

public class TowerBuildMenu : MonoBehaviour
{
    // Singleton để quản lý chỉ một panel được mở
    public static TowerBuildMenu currentOpenMenu = null;

    private BuildableArea buildableArea;

    public Button tower1Button;
    public Button tower2Button;
    public Button tower3Button;
    public Button cancelButton; // Thêm nút thoát
    public GameObject tower1Prefab;
    public GameObject tower2Prefab;
    public GameObject tower3Prefab;
    
    // Remove hardcoded costs - will get from TowerData
    // public int tower1Cost = 100;
    // public int tower2Cost = 200;
    // public int tower3Cost = 300;

    public void SetBuildableArea(BuildableArea area)
    {
        // Đóng panel cũ nếu có (nhưng không bật camera)
        if (currentOpenMenu != null && currentOpenMenu != this)
        {
            currentOpenMenu.ClosePanelOnly();
        }

        // Đặt panel hiện tại làm panel đang mở
        currentOpenMenu = this;
        
        buildableArea = area;
        tower1Button.onClick.RemoveAllListeners();
        tower2Button.onClick.RemoveAllListeners();
        tower3Button.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners(); // Xóa listener cũ của nút thoát
        
        // Get costs from TowerData
        int tower1Cost = GetTowerCost(tower1Prefab);
        int tower2Cost = GetTowerCost(tower2Prefab);
        int tower3Cost = GetTowerCost(tower3Prefab);
        
        tower1Button.onClick.AddListener(() => BuildTower(tower1Prefab, tower1Cost));
        tower2Button.onClick.AddListener(() => BuildTower(tower2Prefab, tower2Cost));
        tower3Button.onClick.AddListener(() => BuildTower(tower3Prefab, tower3Cost));
        cancelButton.onClick.AddListener(CancelBuild); // Thêm listener cho nút thoát
        
        Debug.Log($"Tower costs: {tower1Cost}, {tower2Cost}, {tower3Cost}");
    }
    
    // Get cost from TowerData
    private int GetTowerCost(GameObject towerPrefab)
    {
        if (towerPrefab != null)
        {
            Tower towerComponent = towerPrefab.GetComponent<Tower>();
            if (towerComponent != null && towerComponent.towerData != null)
            {
                int cost = towerComponent.towerData.buildCost;
                Debug.Log($"✅ Getting cost for {towerComponent.towerData.towerName}: {cost} coins");
                return cost;
            }
            else
            {
                Debug.LogError($"❌ TowerData not found for {towerPrefab.name}");
                if (towerComponent == null)
                {
                    Debug.LogError($"❌ Tower component not found on {towerPrefab.name}");
                }
                else if (towerComponent.towerData == null)
                {
                    Debug.LogError($"❌ TowerData is null on {towerPrefab.name}");
                }
            }
        }
        else
        {
            Debug.LogError($"❌ Tower prefab is null!");
        }
        
        // Fallback cost if TowerData not found
        Debug.LogWarning($"⚠️ Using fallback cost: 100 coins");
        return 100;
    }

    void BuildTower(GameObject towerPrefab, int cost)
    {
        if (buildableArea == null) return; // Tránh lỗi nếu đã bị destroy

        Debug.Log($"BuildTower called! Prefab: {towerPrefab.name}, Cost: {cost}");
        
        // Gọi BuildTower của BuildableArea để có validation tiền
        buildableArea.BuildTower(towerPrefab, cost);
        
        // Đóng menu sau khi xây tháp (BuildableArea sẽ tự xử lý việc destroy)
        ClosePanelOnly();
    }

    // Hàm mới: Hủy việc xây tháp
    public void CancelBuild()
    {
        Debug.Log("Cancel build tower");
        
        // Đóng menu build
        if (buildableArea != null)
        {
            buildableArea.CloseMenu();
        }
        else
        {
            // Nếu buildableArea đã bị destroy, bật lại camera movement
            Camera_move.EnableCameraMovement();
        }

        // Reset singleton reference
        if (currentOpenMenu == this)
        {
            currentOpenMenu = null;
        }
        
        // Xóa menu build
        Destroy(gameObject);
    }

    // Hàm để đóng panel từ bên ngoài (không destroy, không bật camera)
    public void ClosePanelOnly()
    {
        Debug.Log("Close panel only");
        
        // Reset singleton reference
        if (currentOpenMenu == this)
        {
            currentOpenMenu = null;
        }
        
        // KHÔNG bật lại camera movement ở đây
        // Camera sẽ vẫn bị tạm dừng cho panel mới
        
        // Xóa menu build
        Destroy(gameObject);
    }
} 