using UnityEngine;

public class BuildableArea : MonoBehaviour
{
    public GameObject towerBuildMenuPrefab;
    private GameObject currentMenuInstance;

    void OnMouseDown()
    {
       Debug.Log("Buildable area clicked");
        if (currentMenuInstance == null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            currentMenuInstance = Instantiate(towerBuildMenuPrefab, FindFirstObjectByType<Canvas>().transform);
            currentMenuInstance.transform.position = screenPos;
            currentMenuInstance.GetComponent<TowerBuildMenu>().SetBuildableArea(this);
        }
    }

    public void BuildTower(GameObject towerPrefab, int cost)
    {
        // TODO: Kiểm tra tiền, trừ tiền, xây tower
        // Ví dụ:
        // if (PlayerCurrency.Instance.TrySpend(cost)) {
        //     Instantiate(towerPrefab, transform.position, Quaternion.identity);
        //     Destroy(gameObject); // Xóa vùng buildable
        // }
    }

    public void CloseMenu()
    {
        if (currentMenuInstance != null)
            Destroy(currentMenuInstance);
    }
} 