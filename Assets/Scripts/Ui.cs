using UnityEngine;
using UnityEngine.SceneManagement; // Thêm để load scene
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Toggle Setting;
    [SerializeField] private GameObject Setting_Panel;

    [Header("Audio")]
    public Toggle musicToggle;
    public Toggle audioEffectToggle;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    void Start()
    {
        //================= SETTING =====================
        Setting.onValueChanged.AddListener(OnSettingToggleChanged);
        Setting_Panel.SetActive(Setting.isOn);
        Time.timeScale = Setting.isOn ? 0 : 1;

        //================= AUDIO =====================
        bool musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        bool sfxOn = PlayerPrefs.GetInt("AudioEffectOn", 1) == 1;

        if (musicToggle != null)
        {
            musicToggle.isOn = musicOn;
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        if (audioEffectToggle != null)
        {
            audioEffectToggle.isOn = sfxOn;
            audioEffectToggle.onValueChanged.AddListener(OnSfxToggleChanged);
        }

        if (musicSource != null) musicSource.mute = !musicOn;
        if (sfxSource != null) sfxSource.mute = !sfxOn;
    }

    private void OnSettingToggleChanged(bool isOn)
    {
        Setting_Panel.SetActive(isOn);
        Time.timeScale = isOn ? 0 : 1;
    }

    //================= BUTTON FUNCTION =====================

    public void Restart()
    {
        Time.timeScale = 1; // đảm bảo không bị tạm dừng
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart");
    }

    public void ContinueGame()
    {
        Setting.isOn = false; // tự động gọi lại OnSettingToggleChanged
        Debug.Log("Continue");
    }

    public void Menu()
    {
        Time.timeScale = 1; // tránh bị kẹt Time.timeScale = 0 khi về menu
        SceneManager.LoadScene(0); // đảm bảo scene menu là buildIndex = 1
        Debug.Log("Menu");
    }

    //================= AUDIO TOGGLE =====================

    private void OnMusicToggleChanged(bool isOn)
    {
        if (musicSource != null) musicSource.mute = !isOn;
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnSfxToggleChanged(bool isOn)
    {
        if (sfxSource != null) sfxSource.mute = !isOn;
        PlayerPrefs.SetInt("AudioEffectOn", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    //================= SKILL =====================

    public void SKILL1()
    {
        Debug.Log("Skill1 Active");
        if (sfxSource != null && !sfxSource.mute) sfxSource.Play();

        //Code
    }

    public void SKILL2()
    {
        Debug.Log("Skill2 Active");
        if (sfxSource != null && !sfxSource.mute) sfxSource.Play();

        //Code
    }
}
