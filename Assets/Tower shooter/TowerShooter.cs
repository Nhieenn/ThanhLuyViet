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
    [Range(0f, 1f)] public float shootVolume = 1f;

    private float lastShootTime;
    private Enemy currentTarget;
    private Tower tower;
    private TowerData towerData;
    private Animator animator;
    private AudioSource audioSource; // để phát tiếng bắn

    public enum TargetingStrategy
    {
        Nearest,
        HighestHP,
        Aircraft,
        FirstInRange
    }

    void Start()
    {
        tower = GetComponent<Tower>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource gắn trên tháp

        if (audioSource != null)
        {
            audioSource.playOnAwake = false; // tắt auto phát khi spawn
        }

        if (tower != null)
        {
            towerData = tower.towerData;
            UpdateStatsFromTowerData();
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        FindTarget();

        if (currentTarget != null)
        {
            RotateTowardsTarget();

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

            if (towerData.projectilePrefab != null)
                projectilePrefab = towerData.projectilePrefab;

            projectileSpeed = towerData.projectileSpeed;
            muzzleFlashPrefab = towerData.muzzleFlashPrefab;
            shootSound = towerData.shootSound;

            switch (towerData.towerType)
            {
                case TowerType.AK: targetingStrategy = TargetingStrategy.Nearest; break;
                case TowerType.B41: targetingStrategy = TargetingStrategy.HighestHP; break;
                case TowerType.SAM2: targetingStrategy = TargetingStrategy.Aircraft; break;
            }
        }
    }

    void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy.currentHealth > 0 && IsValidTarget(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }

        currentTarget = enemiesInRange.Count > 0 ? SelectBestTarget(enemiesInRange) : null;
    }

    bool IsValidTarget(Enemy enemy)
    {
        if (targetingStrategy == TargetingStrategy.Aircraft)
            return enemy.enemyData.enemyType == EnemyType.Aircraft;

        return true;
    }

    Enemy SelectBestTarget(List<Enemy> enemies)
    {
        switch (targetingStrategy)
        {
            case TargetingStrategy.Nearest: return GetNearestEnemy(enemies);
            case TargetingStrategy.HighestHP: return GetEnemyWithHighestHP(enemies);
            case TargetingStrategy.FirstInRange: return enemies[0];
            case TargetingStrategy.Aircraft: return GetNearestEnemy(enemies);
        }
        return enemies[0];
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
        if (currentTarget == null || projectilePrefab == null) return;

        if (animator != null)
            animator.SetTrigger("Shoot");

        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
            projectile.Initialize(currentTarget, damage, projectileSpeed);

        if (muzzleFlashPrefab != null)
            Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);

        if (shootSound != null && audioSource != null)
            audioSource.PlayOneShot(shootSound, shootVolume);

        lastShootTime = Time.time;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void OnTowerUpgraded()
    {
        UpdateStatsFromTowerData();
    }

}
