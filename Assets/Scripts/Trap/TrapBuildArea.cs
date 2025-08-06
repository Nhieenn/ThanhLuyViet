using UnityEngine;

public class TrapBuildArea : MonoBehaviour
{
    public GameObject trapBuildMenuPrefab;
    private GameObject currentMenuInstance;

    void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        if (currentMenuInstance == null)
        {
            Canvas targetCanvas = FindObjectOfType<Canvas>();
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            currentMenuInstance = Instantiate(trapBuildMenuPrefab, targetCanvas.transform);
            currentMenuInstance.transform.position = screenPos;
            currentMenuInstance.GetComponent<TrapBuildMenu>().SetTrapBuildArea(this);
        }
    }

    public void CloseMenu()
    {
        if (currentMenuInstance != null)
        {
            Destroy(currentMenuInstance);
            currentMenuInstance = null;
        }
    }
}