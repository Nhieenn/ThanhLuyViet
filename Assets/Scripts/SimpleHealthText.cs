using UnityEngine;
using TMPro;

public class SimpleHealthText : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI healthText;
    
    [Header("History Panel")]
    public HistoryPanel historyPanel; // Reference to History Panel
    
    [Header("Game Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    
    [Header("Game Over Integration")]
    public bool useGameWinManager = true;
    public bool showHistoryPanelOnDeath = true;
    
    // Events
    public System.Action<int> OnHealthChanged;
    public System.Action OnPlayerDeath;
    
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        
        // Notify health change
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    public void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + "/" + maxHealth;
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Already dead
        
        currentHealth -= damage;
        
        if (currentHealth < 0)
            currentHealth = 0;
            
        UpdateHealthText();
        
        // Notify health change
        OnHealthChanged?.Invoke(currentHealth);
        
        // Check for death
        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }
    
    public void Heal(int healAmount)
    {
        if (currentHealth <= 0) return; // Can't heal if dead
        
        currentHealth += healAmount;
        
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
            
        UpdateHealthText();
        
        // Notify health change
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    public void SetHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        UpdateHealthText();
        
        // Notify health change
        OnHealthChanged?.Invoke(currentHealth);
        
        // Check for death
        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }
    
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        
        // Notify health change
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    private void PlayerDeath()
    {
        Debug.Log("💀 PLAYER DEATH! Hết máu!");
        
        // Notify death event
        OnPlayerDeath?.Invoke();
        
        // Try to use GameWinManager first
        if (useGameWinManager)
        {
            GameWinManager gameWinManager = FindObjectOfType<GameWinManager>();
            if (gameWinManager != null)
            {
                gameWinManager.TriggerGameOver("Player health depleted!");
                return;
            }
        }
        
        // Fallback to History Panel
        if (showHistoryPanelOnDeath && historyPanel != null)
        {
            historyPanel.ShowHistory();
        }
        else
        {
            Debug.LogWarning("History Panel chưa được gán vào SimpleHealthText!");
        }
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
    
    public int GetMissingHealth()
    {
        return maxHealth - currentHealth;
    }
    
    [ContextMenu("Test Damage 20")]
    public void TestDamage20()
    {
        TakeDamage(20);
    }
    
    [ContextMenu("Test Damage 50")]
    public void TestDamage50()
    {
        TakeDamage(50);
    }
    
    [ContextMenu("Test Damage 100")]
    public void TestDamage100()
    {
        TakeDamage(100);
    }
    
    [ContextMenu("Test Heal 20")]
    public void TestHeal20()
    {
        Heal(20);
    }
    
    [ContextMenu("Test Heal 50")]
    public void TestHeal50()
    {
        Heal(50);
    }
    
    [ContextMenu("Restore Full Health")]
    public void TestRestoreFullHealth()
    {
        RestoreFullHealth();
    }
    
    [ContextMenu("Set Health to 1")]
    public void TestSetHealth1()
    {
        SetHealth(1);
    }
    
    [ContextMenu("Set Health to 0")]
    public void TestSetHealth0()
    {
        SetHealth(0);
    }
}
