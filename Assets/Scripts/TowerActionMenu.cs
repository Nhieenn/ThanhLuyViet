using UnityEngine;
using UnityEngine.UI;

public class TowerActionMenu : MonoBehaviour
{
    private Tower targetTower;

    public Button upgradeButton;
    public Button destroyButton;

    void Start()
    {
        Debug.Log("TowerActionMenu created!");
        
        // Kiểm tra các button có được gán không
        if (upgradeButton == null)
            Debug.LogError("Upgrade button is null!");
        if (destroyButton == null)
            Debug.LogError("Destroy button is null!");
    }

    public void SetTargetTower(Tower tower)
    {
        Debug.Log("Setting target tower: " + tower.name);
        targetTower = tower;
        
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => { 
                Debug.Log("Upgrade button clicked!");
                targetTower.Upgrade(); 
                targetTower.CloseMenu(); 
            });
        }
        
        if (destroyButton != null)
        {
            destroyButton.onClick.RemoveAllListeners();
            destroyButton.onClick.AddListener(() => {
                Debug.Log("Destroy button clicked!");
                Destroy(gameObject); // Xóa menu
                targetTower.DestroyTowerWithoutMenu(); // Xóa tháp mà không gọi CloseMenu nữa
            });
        }
        
        Debug.Log("TowerActionMenu setup completed!");
    }
} 