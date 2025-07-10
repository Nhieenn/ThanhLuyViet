using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Gán tên scene chơi game ở đây hoặc qua Inspector
    public string playSceneName = "SampleScene";
    public GameObject settingsPanel;
    public GameObject creditPanel;

    // Hàm gọi khi nhấn nút Play
    public void OnPlayButton()
    {
        SceneManager.LoadScene(playSceneName);
    }

    // Hàm gọi khi nhấn nút Continue
    public void OnContinueButton()
    {
        // Thêm logic load game hoặc resume game ở đây
        Debug.Log("Continue game");
    }

    // Hàm gọi khi nhấn nút Setting
    public void OnSettingButton()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // Hàm gọi khi nhấn nút Exit
    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("Exit game");
    }

    // Hàm gọi khi nhấn nút Credit
    public void OnCreditButton()
    {
        if (creditPanel != null)
            creditPanel.SetActive(true);
    }

    // Hàm đóng panel Setting
    public void OnCloseSetting()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // Hàm đóng panel Credit
    public void OnCloseCredit()
    {
        if (creditPanel != null)
            creditPanel.SetActive(false);
    }
} 