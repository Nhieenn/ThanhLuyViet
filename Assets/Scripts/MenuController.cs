using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Gán tên scene chơi game ở đây hoặc qua Inspector
    public string playSceneName = "SampleScene";
    public GameObject settingsPanel;
    public GameObject creditPanel;
    public Toggle musicToggle;
    public Toggle audioEffectToggle;
    public GameObject menulevel;
    public GameObject mainMenuPanel; // Thêm dòng này ở đầu

    private void Start()
    {
        // Load trạng thái đã lưu khi mở Setting
        if (musicToggle != null)
            musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        if (audioEffectToggle != null)
            audioEffectToggle.isOn = PlayerPrefs.GetInt("AudioEffectOn", 1) == 1;
    }

    // Hàm gọi khi nhấn nút Play
    public void OnPlayButton()
    {// Hàm gọi khi nhấn nút Play
    
    
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);     // Ẩn menu chính

        if (menulevel != null)
            menulevel.SetActive(true);          // Hiện menu chọn level
    


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

    // Hàm lưu cài đặt khi nhấn Save
    public void OnSaveSetting()
    {
        if (musicToggle != null)
            PlayerPrefs.SetInt("MusicOn", musicToggle.isOn ? 1 : 0);
        if (audioEffectToggle != null)
            PlayerPrefs.SetInt("AudioEffectOn", audioEffectToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Settings saved!");
    }

    // Hàm thoát Setting (có thể dùng cho nút Exit hoặc Back)
    public void OnExitSetting()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
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

    public void OnBackFromLevelSelect()
    {
        if (menulevel != null)
            menulevel.SetActive(false);       // Ẩn menu chọn level

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);    // Hiện lại menu chính
    }
    public void OnLoginBitton() 
    {
        SceneManager.LoadScene("FirebaseLogin");


    }
}
