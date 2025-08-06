using UnityEngine;
using System.Collections.Generic;

public class TowerShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float range = 3f;
    public float fireRate = 1f;
    public int damage = 10;
    
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    
    [Header("Targeting Strategy")]
    public TargetingStrategy targetingStrategy = TargetingStrategy.Nearest;
    
    [Header("Visual Effects")]
    public GameObject muzzleFlashPrefab;
    public AudioClip shootSound;
    
    private float lastShootTime;
    private Enemy currentTarget;
    private Tower tower;
    private TowerData towerData;
    
    // Targeting strategies
    public enum TargetingStrategy
    {
        Nearest,        // Enemy gần nhất (AK47)
        HighestHP,      // Enemy có HP cao nhất (B41)
        Aircraft,       // Chỉ bắn máy bay (SAM2)
        FirstInRange    // Enemy đầu tiên trong range
    }
    
    void Start()
    {
        tower = GetComponent<Tower>();
        if (tower != null)
        {
            towerData = tower.towerData;
            UpdateStatsFromTowerData();
        }
    }
    
    void Update()
    {
        if (Time.timeScale == 0) return; // Không bắn khi game pause
        
        FindTarget();
        
        if (currentTarget != null)
        {
            // Rotate tower to face target
            RotateTowardsTarget();
            
            // Shoot if cooldown is ready
            if (Time.time - lastShootTime >= 1f / fireRate)
            {
                Shoot();
            }
        }
    }
    
    void UpdateStatsFromTowerData()
    {
        if (towerData != null && towerData.levels.Length > tower.CurrentLevel)
        {
            var levelData = towerData.levels[tower.CurrentLevel];
            damage = levelData.damage;
            range = levelData.range;
            fireRate = levelData.fireRate;
            
            // Set projectile prefab and speed from TowerData
            if (towerData.projectilePrefab != null)
            {
                projectilePrefab = towerData.projectilePrefab;
            }
            projectileSpeed = towerData.projectileSpeed;
            
            // Set visual effects from TowerData
            muzzleFlashPrefab = towerData.muzzleFlashPrefab;
            shootSound = towerData.shootSound;
            
            // Set targeting strategy based on tower type
            switch (towerData.towerType)
            {
                case TowerType.AK:
                    targetingStrategy = TargetingStrategy.Nearest;
                    break;
                case TowerType.B41:
                    targetingStrategy = TargetingStrategy.HighestHP;
                    break;
                case TowerType.SAM2:
                    targetingStrategy = TargetingStrategy.Aircraft;
                    break;
            }
        }
    }
    
    void FindTarget()
    {
        // Find all enemies in range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        List<Enemy> enemiesInRange = new List<Enemy>();
        
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy.currentHealth > 0)
            {
                Debug.Log("Enemy found: " + currentTarget);
                // Check if enemy is valid target based on strategy
                if (IsValidTarget(enemy))
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }
        
        if (enemiesInRange.Count > 0)
        {
            currentTarget = SelectBestTarget(enemiesInRange);
        }
        else
        {
            currentTarget = null;
        }
    }
    
    bool IsValidTarget(Enemy enemy)
    {
        switch (targetingStrategy)
        {
            case TargetingStrategy.Aircraft:
                return enemy.enemyData.enemyType == EnemyType.Aircraft;
            default:
                return true; // All other strategies can target any enemy
        }
    }
    
    Enemy SelectBestTarget(List<Enemy> enemies)
    {
        switch (targetingStrategy)
        {
            case TargetingStrategy.Nearest:
                return GetNearestEnemy(enemies);
            case TargetingStrategy.HighestHP:
                return GetEnemyWithHighestHP(enemies);
            case TargetingStrategy.Aircraft:
                return GetNearestEnemy(enemies); // Among aircraft
            case TargetingStrategy.FirstInRange:
                return enemies[0];
            default:
                return enemies[0];
        }
    }
    
    Enemy GetNearestEnemy(List<Enemy> enemies)
    {
        Enemy nearest = null;
        float minDistance = float.MaxValue;
        
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }
        
        return nearest;
    }
    
    Enemy GetEnemyWithHighestHP(List<Enemy> enemies)
    {
        Enemy highestHP = null;
        int maxHP = 0;
        
        foreach (Enemy enemy in enemies)
        {
            if (enemy.currentHealth > maxHP)
            {
                maxHP = enemy.currentHealth;
                highestHP = enemy;
            }
        }
        
        return highestHP;
    }
    
    void RotateTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    void Shoot()
    {
        Debug.Log("Shoot called!");
        if (currentTarget == null || projectilePrefab == null) return;
        
        // Create projectile
        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        
        if (projectile != null)
        {
            projectile.Initialize(currentTarget, damage, projectileSpeed);
        }
        
        // Visual and audio effects
        if (muzzleFlashPrefab != null)
        {
            Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
        }
        
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
        }
        
        lastShootTime = Time.time;
        
        Debug.Log($"{towerData.towerName} shot at {currentTarget.enemyData.enemyName} for {damage} damage!");
    }
    
    // Called when tower is upgraded
    public void OnTowerUpgraded()
    {
        UpdateStatsFromTowerData();
    }
    
    // Visual range indicator (for debugging)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
} 