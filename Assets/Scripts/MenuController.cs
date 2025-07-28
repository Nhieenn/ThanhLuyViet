using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject levelSelectPanel;
    // Gán tên scene chơi game ở đây hoặc qua Inspector
    public string playSceneName = "SampleScene";
    public GameObject settingsPanel;
    public GameObject creditPanel;
    public Toggle musicToggle;
    public Toggle audioEffectToggle;
    public GameObject gameMenu;
    public GameObject menu;
    
    
    private void Start()
    {
        // Tự động tìm và gán các reference nếu chưa được gán
        AutoAssignReferences();
        
        // Load trạng thái đã lưu khi mở Setting
        if (musicToggle != null)
            musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        if (audioEffectToggle != null)
            audioEffectToggle.isOn = PlayerPrefs.GetInt("AudioEffectOn", 1) == 1;
    }
    
    private void AutoAssignReferences()
    {
        // Tìm Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in scene!");
            return;
        }
        
        // Tự động tìm menuPanel
        if (menuPanel == null)
        {
            menuPanel = FindPanelByName(canvas.transform, "MenuPanel");
            if (menuPanel == null)
            {
                menuPanel = FindPanelByName(canvas.transform, "Menu");
                if (menuPanel == null)
                {
                    Debug.LogWarning("menuPanel not found! Please assign it manually in Inspector.");
                }
            }
        }
        
        // Tự động tìm levelSelectPanel
        if (levelSelectPanel == null)
        {
            levelSelectPanel = FindPanelByName(canvas.transform, "LevelSelectPanel");
            if (levelSelectPanel == null)
            {
                levelSelectPanel = FindPanelByName(canvas.transform, "Levelselect");
                if (levelSelectPanel == null)
                {
                    Debug.LogWarning("levelSelectPanel not found! Please assign it manually in Inspector.");
                }
            }
        }
        
        // Tự động tìm gameMenu
        if (gameMenu == null)
        {
            gameMenu = FindPanelByName(canvas.transform, "GameMenu");
            if (gameMenu == null)
            {
                // Nếu không tìm thấy gameMenu, sử dụng menuPanel
                gameMenu = menuPanel;
            }
        }
        
        // Tự động tìm settingsPanel
        if (settingsPanel == null)
        {
            settingsPanel = FindPanelByName(canvas.transform, "SettingsPanel");
        }
        
        // Tự động tìm creditPanel
        if (creditPanel == null)
        {
            creditPanel = FindPanelByName(canvas.transform, "CreditPanel");
        }
        
        // Tự động tìm Toggle components
        if (musicToggle == null)
        {
            musicToggle = FindToggleByName("MusicToggle");
        }
        
        if (audioEffectToggle == null)
        {
            audioEffectToggle = FindToggleByName("AudioEffectToggle");
        }
    }
    
    private GameObject FindPanelByName(Transform parent, string panelName)
    {
        // Tìm trực tiếp
        Transform found = parent.Find(panelName);
        if (found != null)
        {
            return found.gameObject;
        }
        
        // Tìm trong tất cả children
        foreach (Transform child in parent)
        {
            if (child.name.Contains(panelName) || child.name.Contains("Panel"))
            {
                return child.gameObject;
            }
            
            // Tìm đệ quy
            GameObject foundInChild = FindPanelByName(child, panelName);
            if (foundInChild != null)
            {
                return foundInChild;
            }
        }
        
        return null;
    }
    
    private Toggle FindToggleByName(string toggleName)
    {
        Toggle[] toggles = FindObjectsOfType<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            if (toggle.name.Contains(toggleName))
            {
                return toggle;
            }
        }
        return null;
    }

    

    // Hàm gọi khi nhấn nút Play
    public void backtomenu()
    {
        levelSelectPanel.active = false;
        menu.active = true;
    }    

    public void OnPlayButton()
    {
        levelSelectPanel.active = true;
        menu.active = false;

        //// Kiểm tra null trước khi sử dụng
        //if (menuPanel != null)
        //    menuPanel.SetActive(false);
        //else
        //    Debug.LogWarning("menuPanel is null!");
            
        //if (gameMenu != null)
        //    gameMenu.SetActive(false);
        //else
        //    Debug.LogWarning("gameMenu is null!");
            
        //if (levelSelectPanel != null)
        //    levelSelectPanel.SetActive(true);
        //else
        //    Debug.LogWarning("levelSelectPanel is null!");
        
        
        //// Không load scene ngay, để người dùng chọn màn
        //Debug.Log("Showing level select panel");
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
        else
            Debug.LogWarning("settingsPanel is null!");
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
        else
            Debug.LogWarning("creditPanel is null!");
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

    // Gọi hàm này khi nhấn nút Back
    public void OnBackButton()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false); // Ẩn panel chọn màn
        if (menuPanel != null)
            menuPanel.SetActive(true);

        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);

         

        else
            Debug.LogWarning("menuPanel is null!");
            
        if (gameMenu != null)
            gameMenu.SetActive(true);
        else
            Debug.LogWarning("gameMenu is null!");
    }
} 