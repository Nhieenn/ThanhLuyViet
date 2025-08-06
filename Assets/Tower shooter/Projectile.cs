using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public int damage = 10;
    public float lifetime = 5f;
    
    [Header("Visual Effects")]
    public GameObject hitEffectPrefab;
    public AudioClip hitSound;
    public TrailRenderer trailRenderer;
    
    [Header("Projectile Type")]
    public ProjectileType projectileType = ProjectileType.Bullet;
    
    private Enemy target;
    private Vector3 direction;
    private float timer;
    
    public enum ProjectileType
    {
        Bullet,     // AK47 - nhỏ, nhanh
        Rocket,     // B41 - lớn, có splash
        Missile     // SAM2 - có tracking
    }
    
    public void Initialize(Enemy targetEnemy, int damageAmount, float projectileSpeed)
    {
        target = targetEnemy;
        damage = damageAmount;
        speed = projectileSpeed;
        timer = 0f;
        
        // Calculate direction
        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
        }
        
        // Set projectile type based on speed
        if (speed > 15f)
            projectileType = ProjectileType.Bullet;
        else if (speed > 8f)
            projectileType = ProjectileType.Rocket;
        else
            projectileType = ProjectileType.Missile;
        
        // Setup visual effects
        SetupVisualEffects();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        // Destroy if lifetime exceeded
        if (timer >= lifetime)
        {
            DestroyProjectile();
            return;
        }
        
        // Move projectile
        MoveProjectile();
        
        // Check for target collision
        CheckTargetCollision();
    }
    
    void MoveProjectile()
    {
        switch (projectileType)
        {
            case ProjectileType.Bullet:
                MoveStraight();
                break;
            case ProjectileType.Rocket:
                MoveStraight();
                break;
            case ProjectileType.Missile:
                MoveWithTracking();
                break;
        }
    }
    
    void MoveStraight()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    
    void MoveWithTracking()
    {
        if (target != null && target.currentHealth > 0)
        {
            // Update direction towards target
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            direction = Vector3.Lerp(direction, targetDirection, Time.deltaTime * 2f);
        }
        
        transform.position += direction * speed * Time.deltaTime;
    }
    
    void CheckTargetCollision()
    {
        if (target == null || target.currentHealth <= 0)
        {
            DestroyProjectile();
            return;
        }
        
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < 0.5f) // Collision threshold
        {
            HitTarget();
        }
    }
    
    void HitTarget()
    {
        // Deal damage to target
        if (target != null)
        {
            target.TakeDamage(damage);
            
            // Log hit
            Debug.Log($"Projectile hit {target.enemyData.enemyName} for {damage} damage!");
        }
        
        // Create hit effects
        CreateHitEffects();
        
        // Destroy projectile
        DestroyProjectile();
    }
    
    void CreateHitEffects()
    {
        // Create hit effect
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // Play hit sound
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
        
        // Create different effects based on projectile type
        switch (projectileType)
        {
            case ProjectileType.Bullet:
                CreateBulletHitEffect();
                break;
            case ProjectileType.Rocket:
                CreateRocketHitEffect();
                break;
            case ProjectileType.Missile:
                CreateMissileHitEffect();
                break;
        }
    }
    
    void CreateBulletHitEffect()
    {
        // Small spark effect
        Debug.Log("Bullet hit effect created");
    }
    
    void CreateRocketHitEffect()
    {
        // Explosion effect with splash damage
        Debug.Log("Rocket explosion effect created");
        
        // Optional: Deal splash damage to nearby enemies
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D collider in nearbyEnemies)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy != target)
            {
                enemy.TakeDamage(damage / 2); // Half damage for splash
            }
        }
    }
    
    void CreateMissileHitEffect()
    {
        // Large explosion effect
        Debug.Log("Missile explosion effect created");
    }
    
    void SetupVisualEffects()
    {
        // Setup trail renderer if available
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }
        
        // Set projectile color based on type
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            switch (projectileType)
            {
                case ProjectileType.Bullet:
                    spriteRenderer.color = Color.yellow;
                    break;
                case ProjectileType.Rocket:
                    spriteRenderer.color = Color.red;
                    break;
                case ProjectileType.Missile:
                    spriteRenderer.color = Color.cyan;
                    break;
            }
        }
    }
    
    void DestroyProjectile()
    {
        // Disable trail renderer
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        
        Destroy(gameObject);
    }
    
    // Visual debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
} 