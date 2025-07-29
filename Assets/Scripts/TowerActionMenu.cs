using UnityEngine;
using UnityEngine.UI;

public class TowerActionMenu : MonoBehaviour
{
    private Tower targetTower;

    public Button upgradeButton;
    public Button destroyButton;

    public void SetTargetTower(Tower tower)
    {
        targetTower = tower;
        upgradeButton.onClick.RemoveAllListeners();
        destroyButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => { targetTower.Upgrade(); targetTower.CloseMenu(); });
        destroyButton.onClick.AddListener(() => {
            Destroy(gameObject); // Xóa menu
            targetTower.DestroyTowerWithoutMenu(); // Xóa tháp mà không gọi CloseMenu nữa
        });
    }
} 