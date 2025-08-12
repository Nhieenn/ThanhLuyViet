using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerActionMenuSetup : MonoBehaviour
{
    [Header("Menu Prefab")]
    [SerializeField] private GameObject towerActionMenuPrefab;
    
    [Header("Button Settings")]
    [SerializeField] private Color upgradeButtonColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color destroyButtonColor = new Color(0.8f, 0.2f, 0.2f, 1f);
    [SerializeField] private Color closeButtonColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    
    [Header("Text Settings")]
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private int fontSize = 16;
    
    [ContextMenu("Create Tower Action Menu Prefab")]
    public void CreateTowerActionMenuPrefab()
    {
        if (towerActionMenuPrefab != null)
        {
            Debug.LogWarning("Tower Action Menu Prefab already exists!");
            return;
        }
        
        // Tạo GameObject chính cho menu
        GameObject menuObj = new GameObject("TowerActionMenu");
        
        // Thêm Image component cho background
        Image background = menuObj.AddComponent<Image>();
        background.color = new Color(0, 0, 0, 0.8f);
        
        // Thêm RectTransform
        RectTransform rectTransform = menuObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 150);
        
        // Thêm TowerActionMenu script
        TowerActionMenu menuScript = menuObj.AddComponent<TowerActionMenu>();
        
        // Tạo Upgrade Button
        GameObject upgradeBtn = CreateButton("UpgradeButton", "UPGRADE", upgradeButtonColor, new Vector2(0, 40));
        upgradeBtn.transform.SetParent(menuObj.transform, false);
        menuScript.upgradeButton = upgradeBtn.GetComponent<Button>();
        
        // Tạo Destroy Button
        GameObject destroyBtn = CreateButton("DestroyButton", "DESTROY", destroyButtonColor, new Vector2(0, 0));
        destroyBtn.transform.SetParent(menuObj.transform, false);
        menuScript.destroyButton = destroyBtn.GetComponent<Button>();
        
        // Tạo Close Button (nút thoát)
        GameObject closeBtn = CreateButton("CloseButton", "CLOSE", closeButtonColor, new Vector2(0, -40));
        closeBtn.transform.SetParent(menuObj.transform, false);
        menuScript.closeButton = closeBtn.GetComponent<Button>();
        
        // Tạo Sell Value Text
        GameObject sellTextObj = new GameObject("SellValueText");
        sellTextObj.transform.SetParent(menuObj.transform, false);
        
        TextMeshProUGUI sellText = sellTextObj.AddComponent<TextMeshProUGUI>();
        sellText.text = "Sell: $0";
        sellText.fontSize = fontSize;
        sellText.color = textColor;
        sellText.alignment = TextAlignmentOptions.Center;
        
        RectTransform sellTextRect = sellTextObj.GetComponent<RectTransform>();
        sellTextRect.anchoredPosition = new Vector2(0, -80);
        sellTextRect.sizeDelta = new Vector2(180, 30);
        
        menuScript.sellValueText = sellText;
        
        // Lưu prefab
        towerActionMenuPrefab = menuObj;
        
        Debug.Log("Tower Action Menu Prefab created successfully with Close button!");
    }
    
    private GameObject CreateButton(string name, string text, Color color, Vector2 position)
    {
        GameObject buttonObj = new GameObject(name);
        
        // Thêm Image component
        Image image = buttonObj.AddComponent<Image>();
        image.color = color;
        
        // Thêm Button component
        Button button = buttonObj.AddComponent<Button>();
        
        // Setup RectTransform
        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(160, 35);
        
        // Tạo Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.color = textColor;
        textComponent.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        return buttonObj;
    }
    
    [ContextMenu("Test Menu Creation")]
    public void TestMenuCreation()
    {
        if (towerActionMenuPrefab == null)
        {
            CreateTowerActionMenuPrefab();
        }
        
        // Tạo menu test trong scene
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            GameObject testMenu = Instantiate(towerActionMenuPrefab, canvas.transform);
            testMenu.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            
            // Tự động destroy sau 3 giây
            Destroy(testMenu, 3f);
            
            Debug.Log("Test menu created! Will be destroyed in 3 seconds.");
        }
        else
        {
            Debug.LogError("No Canvas found in scene!");
        }
    }
}
