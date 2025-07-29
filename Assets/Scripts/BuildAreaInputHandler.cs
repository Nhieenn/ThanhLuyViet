using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAreaInputHandler : MonoBehaviour
{
    public GameObject towerBuildMenuPrefab;
    private GameObject currentMenuInstance;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("Buildable area clicked (Input System)");
                if (currentMenuInstance == null)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                    currentMenuInstance = Instantiate(towerBuildMenuPrefab, FindFirstObjectByType<Canvas>().transform); 
                    currentMenuInstance.transform.position = screenPos;
                    // Sửa dòng này: nếu object không có BuildableArea, truyền null hoặc truyền chính script này nếu bạn gộp logic
                    var menu = currentMenuInstance.GetComponent<TowerBuildMenu>();
                    if (menu == null)
                    {
                        Debug.LogError("TowerBuildMenu script NOT found on the root of TowerBuildMenu prefab! " +
                                       "Check if the script is attached and there are no compile errors.");
                    }
                    else
                    {
                        Debug.Log("TowerBuildMenu script found!");
                    }
                    var buildable = this.GetComponent<BuildableArea>();
                    if (buildable == null)
                    {
                        Debug.LogError("BuildableArea script NOT found on Build Area!");
                    }
                    else
                    {
                        Debug.Log("BuildableArea script found!");
                    }
                    if (menu != null && buildable != null)
                    {
                        menu.SetBuildableArea(buildable);
                    }
                }
            }
        }
    }
}