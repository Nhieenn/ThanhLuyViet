using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;

    [Header("Current Stats")]
    public int currentHealth;

    [Header("Modifiers")]
    public float healthMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float rewardMultiplier = 1f;

    [Header("Components")]
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [Header("Movement")]
    public Vector3[] waypoints;
    public int currentWaypointIndex = 0;
    public float waypointReachedThreshold = 0.1f;

    [Header("UI")]
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;

    void Start()
    {
        InitializeEnemy();
    }

    void Update()
    {
        MoveAlongWaypoints();
        UpdateHealthBar();

        if (enemyData.canHeal && currentHealth < GetMaxHealth())
        {
            HealOverTime();
        }
    }

    void InitializeEnemy()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (enemyData == null)
        {
            Debug.LogError("EnemyData not assigned to " + gameObject.name);
            return;
        }

        currentHealth = GetMaxHealth();

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = enemyData.sprite;
            spriteRenderer.color = enemyData.enemyColor;
        }

        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        Debug.Log("Enemy " + enemyData.enemyName + " initialized with " + currentHealth + " HP");
    }

    void MoveAlongWaypoints()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Vector3 targetPos = waypoints[currentWaypointIndex];
        float speed = GetMoveSpeed();
        Vector3 direction = (targetPos - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance < waypointReachedThreshold)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachDestination();
            }
        }

        // Optional: rotate to face movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void ReachDestination()
    {
        Debug.Log("Enemy reached the end!");
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (enemyData.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(enemyData.hitSound, transform.position);
        }

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

        int moneyReward = enemyData.moneyReward;
        Debug.Log("Player gained $" + moneyReward + " from killing " + enemyData.enemyName);

        if (enemyData.deathEffectPrefab != null)
        {
            Instantiate(enemyData.deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (enemyData.deathSound != null)
        {
            AudioSource.PlayClipAtPoint(enemyData.deathSound, transform.position);
        }

        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }

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
            healthBarInstance.transform.position = transform.position + Vector3.up * 0.5f;
            var healthBar = healthBarInstance.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.UpdateHealth(currentHealth, GetMaxHealth());
            }
        }
    }

    // Helpers
    public int GetMaxHealth() => Mathf.RoundToInt(enemyData.health * healthMultiplier);
    public int GetAttackDamage() => Mathf.RoundToInt(enemyData.attackDamage * damageMultiplier);
    public float GetMoveSpeed() => enemyData.moveSpeed * speedMultiplier;
    public int GetMoneyReward() => Mathf.RoundToInt(enemyData.moneyReward * rewardMultiplier);

    public void SetWaypoints(Vector3[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
    }
}
