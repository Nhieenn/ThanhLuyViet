using UnityEngine;
using UnityEngine.SceneManagement;

public class cayvcl : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingPanel;

    private bool isSettingOpen = false;

    void Start()
    {
        settingPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToggleSetting()
    {
        isSettingOpen = !isSettingOpen;
        settingPanel.SetActive(isSettingOpen);
        Time.timeScale = isSettingOpen ? 0 : 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void SKILL1()
    {
        Debug.Log("Skill1 Active");
    }

    public void SKILL2()
    {
        Debug.Log("Skill2 Active");
    }

}
