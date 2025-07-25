using UnityEngine;
using UnityEngine.InputSystem;

public class TowerInputHandler : MonoBehaviour
{
    public GameObject actionMenuPrefab;
    private GameObject currentMenuInstance;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("Tower clicked (Input System)");
                if (currentMenuInstance == null)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                    currentMenuInstance = Instantiate(actionMenuPrefab, FindFirstObjectByType<Canvas>().transform);
                    currentMenuInstance.transform.position = screenPos;
                    currentMenuInstance.GetComponent<TowerActionMenu>().SetTargetTower(this.GetComponent<Tower>());
                }
            }
        }
    }
}