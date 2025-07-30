using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject levelSelectPanel; // Panel chọn màn
    public GameObject menuPanel; // Panel menu chính
    
    void Start()
    {
        // Gán sự kiện click
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(BackToMainMenu);
        }
    }
    
    public void BackToMainMenu()
    {
        Debug.Log("Back to main menu");
        
        // Ẩn panel chọn màn
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("levelSelectPanel is not assigned!");
        }
        
        // Hiện panel menu chính
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("menuPanel is not assigned!");
        }
    }
} 