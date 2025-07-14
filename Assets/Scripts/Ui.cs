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
    [SerializeField] private Image isPressed_Pause;

    void Start()
    {
        //================= SETTING =====================
        Setting.onValueChanged.AddListener(OnSettingToggleChanged);
        Setting_Panel.SetActive(Setting.isOn);

        //================= PAUSE GAME =====================
        PauseGame.onValueChanged.AddListener(OnPauseToggleChanged);

        // Khởi tạo Time.timeScale phù hợp
        Time.timeScale = (Setting.isOn || PauseGame.isOn) ? 0 : 1;
    }

    private void OnSettingToggleChanged(bool isOn)
    {
        Setting_Panel.SetActive(isOn);

        // Nếu đang bật Setting hoặc PauseGame thì dừng game
        Time.timeScale = (isOn || PauseGame.isOn) ? 0 : 1;
    }

    private void OnPauseToggleChanged(bool isOn)
    {
        // Cập nhật alpha màu
        Color c = isPressed_Pause.color;
        c.a = isOn ? 0.5f : 1f;
        isPressed_Pause.color = c;

        // Nếu đang bật Pause hoặc Setting thì dừng game
        Time.timeScale = (isOn || Setting.isOn) ? 0 : 1;
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
        Setting.isOn = false;
        PauseGame.isOn = false;

        Time.timeScale = 1;

        // Reset màu nút pause
        Color c = isPressed_Pause.color;
        c.a = 1f;
        isPressed_Pause.color = c;

        Debug.Log("Continue");
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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
