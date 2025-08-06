using UnityEngine;
using UnityEngine.UI;

public class TrapBuildMenu : MonoBehaviour
{
    public Button spikeButton;
    public Button mineButton;
    public Button cancelButton;
    public GameObject spikeTrapPrefab;
    public GameObject mineTrapPrefab;
    private TrapBuildArea currentTrapArea;

    public void SetTrapBuildArea(TrapBuildArea area)
    {
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
    }

    void CancelBuild()
    {
        if (currentTrapArea != null)
            currentTrapArea.CloseMenu();
    }
}