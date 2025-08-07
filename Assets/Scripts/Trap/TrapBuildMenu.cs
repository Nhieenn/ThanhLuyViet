using UnityEngine;
using UnityEngine.UI;

public class TrapBuildMenu : MonoBehaviour
{
    // Singleton để quản lý chỉ một panel được mở
    public static TrapBuildMenu currentOpenMenu = null;

    public Button spikeButton;
    public Button mineButton;
    public Button cancelButton;
    public GameObject spikeTrapPrefab;
    public GameObject mineTrapPrefab;
    private TrapBuildArea currentTrapArea;

    public void SetTrapBuildArea(TrapBuildArea area)
    {
        // Đóng panel cũ nếu có (nhưng không bật camera)
        if (currentOpenMenu != null && currentOpenMenu != this)
        {
            currentOpenMenu.ClosePanelOnly();
        }

        // Đặt panel hiện tại làm panel đang mở
        currentOpenMenu = this;
        
        currentTrapArea = area;
        spikeButton.onClick.RemoveAllListeners();
        mineButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        spikeButton.onClick.AddListener(() => BuildTrap(spikeTrapPrefab));
        mineButton.onClick.AddListener(() => BuildTrap(mineTrapPrefab));
        cancelButton.onClick.AddListener(CancelBuild);
    }

    void BuildTrap(GameObject trapPrefab)
    {
        if (currentTrapArea == null) return;
        Instantiate(trapPrefab, currentTrapArea.transform.position, Quaternion.identity);
        currentTrapArea.CloseMenu();
        
        // Reset singleton reference
        if (currentOpenMenu == this)
        {
            currentOpenMenu = null;
        }
        
        // Xóa menu sau khi đặt trap
        Destroy(gameObject);
    }

    void CancelBuild()
    {
        if (currentTrapArea != null)
            currentTrapArea.CloseMenu();
        
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
        Debug.Log("Close trap panel only");
        
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