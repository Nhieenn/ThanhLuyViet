using UnityEngine;
using UnityEngine.SceneManagement;

public class Ui_manager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingPanel;

    private bool isSettingOpen = false;

    void Start()
    {
        settingPanel.SetActive(false);
        Time.timeScale = 1;
    }

    // Nút Setting: Bật/tắt panel Setting
    public void ToggleSetting()
    {
        isSettingOpen = !isSettingOpen;
        settingPanel.SetActive(isSettingOpen);
        Time.timeScale = isSettingOpen ? 0 : 1;
    }

    // Nút Restart
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Nút Menu (về menu chính)
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    // Nút Skill 1
    public void SKILL1()
    {
        Debug.Log("Skill1 Active");
        // Thêm code xử lý skill 1 ở đây
    }

    // Nút Skill 2
    public void SKILL2()
    {
        Debug.Log("Skill2 Active");
        // Thêm code xử lý skill 2 ở đây
    }

}
