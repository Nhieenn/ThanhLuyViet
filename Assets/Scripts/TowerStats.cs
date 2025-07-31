using UnityEngine;
using UnityEngine.UI;

public class TowerStats : MonoBehaviour
{
    [Header("UI References")]
    public Text towerNameText;
    public Text levelText;
    public Text damageText;
    public Text rangeText;
    public Text fireRateText;
    public Text upgradeCostText;
    public Text descriptionText;
    
    [Header("Visual Elements")]
    public Image towerIcon;
    public Slider levelProgressBar;
    public Button upgradeButton;
    
    private Tower currentTower;
    private TowerData towerData;
    
    public void SetTower(Tower tower)
    {
        currentTower = tower;
        towerData = tower.towerData;
        
        if (towerData != null)
        {
            UpdateUI();
        }
    }
    
    public void UpdateUI()
    {
        if (towerData == null) return;
        
        // Cập nhật thông tin cơ bản
        if (towerNameText != null)
            towerNameText.text = towerData.towerName;
            
        if (levelText != null)
            levelText.text = "Level " + (currentTower.CurrentLevel + 1);
            
        if (towerIcon != null && towerData.levels.Length > currentTower.CurrentLevel)
            towerIcon.sprite = towerData.levels[currentTower.CurrentLevel].sprite;
        
        // Cập nhật stats
        var currentLevelData = towerData.levels[currentTower.CurrentLevel];
        
        if (damageText != null)
            damageText.text = "Damage: " + currentLevelData.damage;
            
        if (rangeText != null)
            rangeText.text = "Range: " + currentLevelData.range.ToString("F1");
            
        if (fireRateText != null)
            fireRateText.text = "Fire Rate: " + currentLevelData.fireRate.ToString("F1");
            
        if (descriptionText != null)
            descriptionText.text = currentLevelData.description;
        
        // Cập nhật nút upgrade và text giá
        if (upgradeButton != null)
        {
            bool canUpgrade = currentTower.CurrentLevel < towerData.levels.Length - 1;
            upgradeButton.interactable = canUpgrade;
            
            if (upgradeCostText != null)
            {
                if (currentTower.CurrentLevel == 0)
                {
                    // Level 1 (mặc định): hiển thị giá nâng cấp lên level 2
                    var nextLevelData = towerData.levels[currentTower.CurrentLevel + 1];
                    upgradeCostText.text = "Upgrade: $" + nextLevelData.upgradeCost;
                }
                else if (canUpgrade)
                {
                    // Level 2: hiển thị giá nâng cấp lên level 3
                    var nextLevelData = towerData.levels[currentTower.CurrentLevel + 1];
                    upgradeCostText.text = "Upgrade: $" + nextLevelData.upgradeCost;
                }
                else
                {
                    // Level 3 (max): hiển thị thông báo
                    upgradeCostText.text = "MAX LEVEL";
                }
            }
        }
        
        // Cập nhật progress bar
        if (levelProgressBar != null)
        {
            float progress = (float)(currentTower.CurrentLevel + 1) / towerData.levels.Length;
            levelProgressBar.value = progress;
        }
    }
    
    public void OnUpgradeButtonClicked()
    {
        if (currentTower != null)
        {
            currentTower.Upgrade();
            UpdateUI(); // Cập nhật UI sau khi nâng cấp
        }
    }
} 