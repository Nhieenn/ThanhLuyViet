using UnityEngine;
using UnityEngine.UI;
using TMPro; // Thêm namespace cho TextMeshPro

public class TowerActionMenu : MonoBehaviour
{
    private Tower targetTower;

    [Header("Action Buttons")]
    public Button upgradeButton;
    public Button destroyButton;
    public Button closeButton; // Thêm nút thoát
    
    [Header("UI Elements")]
    public TextMeshProUGUI sellValueText; // Thêm text hiển thị giá bán

    void Start()
    {
        Debug.Log("TowerActionMenu created!");
        
        // Kiểm tra các button có được gán không
        if (upgradeButton == null)
            Debug.LogError("Upgrade button is null!");
        if (destroyButton == null)
            Debug.LogError("Destroy button is null!");
        if (closeButton == null)
            Debug.LogError("Close button is null!");
    }

    public void SetTargetTower(Tower tower)
    {
        Debug.Log("Setting target tower: " + tower.name);
        targetTower = tower;
        
        // Setup Upgrade Button
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => { 
                Debug.Log("Upgrade button clicked!");
                targetTower.Upgrade(); 
                targetTower.CloseMenu(); 
            });
        }
        
        // Setup Destroy Button
        if (destroyButton != null)
        {
            destroyButton.onClick.RemoveAllListeners();
            destroyButton.onClick.AddListener(() => {
                Debug.Log("Destroy button clicked!");
                Destroy(gameObject); // Xóa menu
                targetTower.DestroyTowerWithoutMenu(); // Xóa tháp mà không gọi CloseMenu nữa
            });
        }
        
        // Setup Close Button (nút thoát)
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() => {
                Debug.Log("Close button clicked!");
                CloseMenu();
            });
        }
        
        // Hiển thị giá bán tháp
        if (sellValueText != null && tower.towerData != null)
        {
            var currentLevelData = tower.towerData.levels[tower.CurrentLevel];
            sellValueText.text = "Sell: $" + currentLevelData.sellValue;
        }
        
        Debug.Log("TowerActionMenu setup completed!");
    }
    
    // Phương thức đóng menu
    public void CloseMenu()
    {
        Debug.Log("Closing TowerActionMenu");
        
        // Bật lại camera movement
        Camera_move.EnableCameraMovement();
        
        // Thông báo cho tower rằng menu đã đóng
        if (targetTower != null)
        {
            targetTower.CloseMenu();
        }
        
        // Xóa menu
        Destroy(gameObject);
    }
    
    // Phương thức để đóng menu từ bên ngoài
    public void ForceClose()
    {
        CloseMenu();
    }
} 