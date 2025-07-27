using UnityEngine;
using UnityEngine.UI;

public class TowerBuildMenu : MonoBehaviour
{
    private BuildableArea buildableArea;

    public Button tower1Button;
    public Button tower2Button;
    public Button tower3Button;
    public GameObject tower1Prefab;
    public GameObject tower2Prefab;
    public GameObject tower3Prefab;
    public int tower1Cost = 100;
    public int tower2Cost = 200;
    public int tower3Cost = 300;

    public void SetBuildableArea(BuildableArea area)
    {
        buildableArea = area;
        tower1Button.onClick.RemoveAllListeners();
        tower2Button.onClick.RemoveAllListeners();
        tower3Button.onClick.RemoveAllListeners();
        tower1Button.onClick.AddListener(() => BuildTower(tower1Prefab, tower1Cost));
        tower2Button.onClick.AddListener(() => BuildTower(tower2Prefab, tower2Cost));
        tower3Button.onClick.AddListener(() => BuildTower(tower3Prefab, tower3Cost));
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

        // Xóa vùng build sau khi xây tháp
        Destroy(buildableArea.gameObject);

        // Xóa menu sau khi đặt tháp (nếu muốn)
        Destroy(gameObject);
    }
} 