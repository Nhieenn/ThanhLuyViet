using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI Components")]
    public Image healthBarFill;
    public Text healthText;
    
    [Header("Settings")]
    public bool showHealthText = true;
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;
    
    private int currentHealth;
    private int maxHealth;
    
    void Start()
    {
        // Auto-find components if not assigned
        if (healthBarFill == null)
            healthBarFill = GetComponentInChildren<Image>();
            
        if (healthText == null)
            healthText = GetComponentInChildren<Text>();
    }
    
    public void UpdateHealth(int current, int max)
    {
        currentHealth = current;
        maxHealth = max;
        
        // Update health bar fill
        if (healthBarFill != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = healthPercentage;
            
            // Change color based on health
            healthBarFill.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
        }
        
        // Update health text
        if (healthText != null && showHealthText)
        {
            healthText.text = currentHealth + "/" + maxHealth;
        }
    }
    
    public void SetHealthBarVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
} 