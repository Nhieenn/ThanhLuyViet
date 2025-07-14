using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Toggle Setting;
    [SerializeField] private GameObject Setting_Panel;

    [Header("Pause Game")]
    [SerializeField] private Toggle PauseGame;

    void Start()
    {
        //================= SETTING =====================
        Setting.onValueChanged.AddListener(OnSettingToggleChanged);
        Setting_Panel.SetActive(Setting.isOn);
        Time.timeScale = Setting.isOn ? 0 : 1;

        //================= PAUSE GAME =====================
        PauseGame.onValueChanged.AddListener(OnPauseToggleChanged);
    }
    private void OnSettingToggleChanged(bool isOn)
    {
        Setting_Panel.SetActive(isOn);
        Time.timeScale = isOn ? 0 : 1;

        // Đảm bảo không bị xung đột nếu Setting mở -> Pause phải tắt
        if (isOn && PauseGame.isOn)
        {
            PauseGame.isOn = false;
        }
    }

    private void OnPauseToggleChanged(bool isOn)
    {
        if (!Setting.isOn) // chỉ pause nếu Setting đang tắt
        {
            Time.timeScale = isOn ? 0 : 1;
        }
    }

    //================= BUTTON FUNCTION =====================

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart");
    }

    public void ContinueGame()
    {
        Setting.isOn = false; // tự động gọi lại OnSettingToggleChanged
        PauseGame.isOn = false; // bỏ pause nếu đang pause
        Debug.Log("Continue");
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0); // Menu là scene 0
        Debug.Log("Menu");
    }

    //================= SKILL =====================
    public void SKILL1()
    {
        Debug.Log("Skill1 Active");
        //Code
    }

    public void SKILL2()
    {
        Debug.Log("Skill2 Active");
        //Code
    }
}
