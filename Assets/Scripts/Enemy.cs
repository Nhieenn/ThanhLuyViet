using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;
    
    [Header("Current Stats")]
    public int currentHealth;
    
    [Header("Components")]
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    
    [Header("Movement")]
    public Transform target;
    public Vector3[] waypoints;
    public int currentWaypointIndex = 0;
    
    [Header("UI")]
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    
    void Start()
    {
        InitializeEnemy();
    }
    
    void Update()
    {
        MoveToTarget();
        UpdateHealthBar();
        
        // Special abilities
        if (enemyData.canHeal && currentHealth < GetMaxHealth())
        {
            HealOverTime();
        }
    }
    
    void InitializeEnemy()
    {
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        if (enemyData == null)
        {
            Debug.LogError("EnemyData not assigned to " + gameObject.name);
            return;
        }
        
        // Set initial stats
        currentHealth = GetMaxHealth();
        
        // Set visual
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = enemyData.sprite;
            spriteRenderer.color = enemyData.enemyColor;
        }
        
        // Create health bar
        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        
        Debug.Log("Enemy " + enemyData.enemyName + " initialized with " + currentHealth + " HP");
    }
    
    void MoveToTarget()
    {
        if (target == null || enemyData == null) return;
        
        float moveSpeed = enemyData.moveSpeed;
        Vector3 direction = (target.position - transform.position).normalized;
        
        // Move towards target
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        // Rotate towards target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Play hit sound
        if (enemyData.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(enemyData.hitSound, transform.position);
        }
        
        // Visual feedback
        StartCoroutine(DamageFlash());
        
        Debug.Log(enemyData.enemyName + " took " + damage + " damage. Health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    System.Collections.IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }
    
    void Die()
    {
        Debug.Log(enemyData.enemyName + " died!");
        
        // Give money reward
        int moneyReward = enemyData.moneyReward;
        // PlayerCurrency.Instance.AddMoney(moneyReward);
        Debug.Log("Player gained $" + moneyReward + " from killing " + enemyData.enemyName);
        
        // Play death effect
        if (enemyData.deathEffectPrefab != null)
        {
            Instantiate(enemyData.deathEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // Play death sound
        if (enemyData.deathSound != null)
        {
            AudioSource.PlayClipAtPoint(enemyData.deathSound, transform.position);
        }
        
        // Destroy health bar
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }
        
        // Destroy enemy
        Destroy(gameObject);
    }
    
    void HealOverTime()
    {
        float healAmount = enemyData.healRate * Time.deltaTime;
        currentHealth = Mathf.Min(currentHealth + (int)healAmount, GetMaxHealth());
    }
    
    void UpdateHealthBar()
    {
        if (healthBarInstance != null)
        {
            // Update health bar position
            healthBarInstance.transform.position = transform.position + Vector3.up * 0.5f;
            
            // Update health bar fill
            var healthBar = healthBarInstance.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.UpdateHealth(currentHealth, GetMaxHealth());
            }
        }
    }
    
    // Helper methods
    public int GetMaxHealth()
    {
        return enemyData.health;
    }
    
    public int GetAttackDamage()
    {
        return enemyData.attackDamage;
    }
    
    public float GetMoveSpeed()
    {
        return enemyData.moveSpeed;
    }
    
    public int GetMoneyReward()
    {
        return enemyData.moneyReward;
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    public void SetWaypoints(Vector3[] newWaypoints)
    {
        waypoints = newWaypoints;
        if (waypoints.Length > 0)
        {
            target = new GameObject("Waypoint").transform;
            target.position = waypoints[0];
        }
    }
} 