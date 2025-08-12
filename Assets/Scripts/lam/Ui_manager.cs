using UnityEngine;
using UnityEngine.SceneManagement;

public class Ui_manager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingPanel;

    private bool isSettingOpen = false;

    public AudioSource audioSource;

    [SerializeField] private GameObject winpanel;
    [SerializeField] private GameObject lostpanel;
    [SerializeField] private GameObject panel;


    void Start()
    {
        winpanel.SetActive(false);
        lostpanel.SetActive(false);
        settingPanel.SetActive(false);
        panel.SetActive(false);
        Time.timeScale = 1;
    }

    // Nút Setting: Bật/tắt panel Setting
    public void ToggleAudio()
    {
        if (audioSource == null) return;

        audioSource.mute = !audioSource.mute; // Đảo trạng thái mute
    }



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

    public void SelectLv2()
    {
        SceneManager.LoadScene("Lv2");
    }
    public void SelectLv3()
    {
        SceneManager.LoadScene("Lv3");
    }
    public void SelectLv4()
    {
        SceneManager.LoadScene("Lv4");
    }
    public void SelectLv5()
    {
        SceneManager.LoadScene("Lv5");
    }
    public void SelectLv6()
    {
        SceneManager.LoadScene("Lv6");
    }
    public void SelectLv7()
    {
        SceneManager.LoadScene("Lv7");
    }
    public void SelectLv8()
    {
        SceneManager.LoadScene("Lv8");
    }
    public void SelectLv9()
    {
        SceneManager.LoadScene("Lv9");

    }
    public void SelectLv10()
    {
        SceneManager.LoadScene("Lv10");
    }
   public void panelButton()
    {
        panel.SetActive(true);
        winpanel.SetActive(false);
        
    }
    public void Backpanel()
    {
        panel.SetActive(false);
        winpanel.SetActive(true);
    }
}
