using UnityEngine;

public class audiomanager : MonoBehaviour
{
    public static audiomanager Instance;

    [Header("Audio Sources")]
    public AudioSource[] musicSources;
    public AudioSource[] sfxSources;

    private bool isMusicMuted;
    private bool isSFXMuted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Lấy trạng thái từ PlayerPrefs
            LoadSettings();
            ApplyMusicState();
            ApplySFXState();
        }
        else
        {
            Destroy(gameObject); // Ngăn tạo thêm bản mới
        }
    }

    private void LoadSettings()
    {
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }

    // ==== Play ====
    public void PlaySFX(AudioClip clip, int sourceIndex = 0)
    {
        if (!isSFXMuted && clip != null && sourceIndex >= 0 && sourceIndex < sfxSources.Length)
        {
            sfxSources[sourceIndex].PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip clip, int sourceIndex = 0)
    {
        if (clip != null && sourceIndex >= 0 && sourceIndex < musicSources.Length)
        {
            if (musicSources[sourceIndex].clip != clip)
            {
                musicSources[sourceIndex].clip = clip;
                if (!isMusicMuted)
                    musicSources[sourceIndex].Play();
            }
        }
    }

    // ==== Toggle ====
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMusicState();
    }

    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;
        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);
        PlayerPrefs.Save();
        ApplySFXState();
    }

    // ==== Apply ====
    private void ApplyMusicState()
    {
        foreach (var source in musicSources)
            source.mute = isMusicMuted;
    }

    private void ApplySFXState()
    {
        foreach (var source in sfxSources)
            source.mute = isSFXMuted;
    }

    // ==== Get ====
    public bool IsMusicMuted() => isMusicMuted;
    public bool IsSFXMuted() => isSFXMuted;
}
