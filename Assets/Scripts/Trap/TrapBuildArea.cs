using UnityEngine;

public class TrapBuildArea : MonoBehaviour
{
    public GameObject trapBuildMenuPrefab;
    private GameObject currentMenuInstance;

    void OnMouseDown()
    {
        // Kiểm tra nếu game đang pause hoặc setting thì không cho mở panel
        if (Time.timeScale == 0) 
        {
            Debug.Log("Game is paused/setting is open, cannot open trap build menu");
            return;
        }
        
        if (currentMenuInstance == null)
        {
            // Tạm dừng camera movement khi mở menu
            Camera_move.DisableCameraMovement();
            
            Canvas targetCanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();
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
            
            // Bật lại camera movement khi đóng menu
            Camera_move.EnableCameraMovement();
        }
    }
}