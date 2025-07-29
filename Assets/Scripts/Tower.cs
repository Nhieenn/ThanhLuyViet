using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject actionMenuPrefab;
    public GameObject buildAreaPrefab; // Thêm trường này
    private GameObject currentMenuInstance;

    void OnMouseDown()
    {
        if (currentMenuInstance == null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            currentMenuInstance = Instantiate(actionMenuPrefab, FindFirstObjectByType<Canvas>().transform);
            currentMenuInstance.transform.position = screenPos;
            currentMenuInstance.GetComponent<TowerActionMenu>().SetTargetTower(this);
        }
    }

    public void Upgrade()
    {
        Debug.Log("Upgrade tower!");
        // TODO: Logic nâng cấp tháp (tăng level, damage, v.v.)
    }

    public void DestroyTower()
    {
        Debug.Log("Destroy tower!");
        CloseMenu(); // Đóng menu trước khi xóa tháp
        Destroy(gameObject);
    }

    public void CloseMenu()
    {
        if (currentMenuInstance != null)
            Destroy(currentMenuInstance);
    }

    public void DestroyTowerWithoutMenu()
    {
        Debug.Log("Destroy tower!");
        
        // Spawn lại vùng build tại vị trí tháp
        GameObject buildArea = Instantiate(buildAreaPrefab, transform.position, Quaternion.identity);
        
        // Xóa tháp
        Destroy(gameObject);
    }
} 