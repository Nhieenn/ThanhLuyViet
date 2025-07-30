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
    public int tower1Cost = 100;
    public int tower2Cost = 200;
    public int tower3Cost = 300;

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
        
        tower1Button.onClick.AddListener(() => BuildTower(tower1Prefab, tower1Cost));
        tower2Button.onClick.AddListener(() => BuildTower(tower2Prefab, tower2Cost));
        tower3Button.onClick.AddListener(() => BuildTower(tower3Prefab, tower3Cost));
        cancelButton.onClick.AddListener(CancelBuild); // Thêm listener cho nút thoát
    }

    void BuildTower(GameObject towerPrefab, int cost)
    {
        if (buildableArea == null) return; // Tránh lỗi nếu đã bị destroy

        Debug.Log("BuildTower called! Prefab: " + towerPrefab.name + " at position: " + buildableArea.transform.position);
        Instantiate(towerPrefab, buildableArea.transform.position, Quaternion.identity);

        // Đóng menu trước khi destroy buildableArea
        buildableArea.CloseMenu();

        // Vô hiệu hóa các button để không thể bấm tiếp
        tower1Button.interactable = false;
        tower2Button.interactable = false;
        tower3Button.interactable = false;
        cancelButton.interactable = false;

        // Xóa vùng build sau khi xây tháp
        Destroy(buildableArea.gameObject);

        // Bật lại camera movement
        Camera_move.EnableCameraMovement();

        // Reset singleton reference
        if (currentOpenMenu == this)
        {
            currentOpenMenu = null;
        }

        // Xóa menu sau khi đặt tháp (nếu muốn)
        Destroy(gameObject);
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