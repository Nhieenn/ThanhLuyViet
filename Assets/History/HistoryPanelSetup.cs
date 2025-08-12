using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HistoryPanelSetup : MonoBehaviour
{
    [Header("Setup Instructions")]
    [TextArea(3, 10)]
    public string setupInstructions = "1. Chọn HistoryPanelRoot prefab\n2. Thêm 2 Button con: RestartButton và MenuButton\n3. Gán vào HistoryPanel script\n4. Xóa nút Close cũ";
    
    [Header("Button References")]
    public Button restartButton;
    public Button menuButton;
    
    [Header("Text References")]
    public TMP_Text restartText;
    public TMP_Text menuText;
    
    [ContextMenu("Setup History Panel")]
    public void SetupHistoryPanel()
    {
        Debug.Log("=== SETUP HISTORY PANEL ===");
        Debug.Log("1. Chọn HistoryPanelRoot prefab");
        Debug.Log("2. Thêm 2 Button con:");
        Debug.Log("   - RestartButton (với text 'Restart')");
        Debug.Log("   - MenuButton (với text 'Menu')");
        Debug.Log("3. Gán vào HistoryPanel script");
        Debug.Log("4. Xóa nút Close cũ");
        Debug.Log("5. Kéo thả SimpleHealthText vào History Panel field");
    }
    
    [ContextMenu("Test Restart")]
    public void TestRestart()
    {
        if (restartButton != null)
        {
            restartButton.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("Restart button chưa được gán!");
        }
    }
    
    [ContextMenu("Test Menu")]
    public void TestMenu()
    {
        if (menuButton != null)
        {
            menuButton.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("Menu button chưa được gán!");
        }
    }
}
