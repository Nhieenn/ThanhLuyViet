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
    public AudioClip shootSound;      // Tiếng đạn bắn
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

        if (target != null)
        {
            // Tính direction từ vị trí viên đạn đến target (bình thường lúc spawn viên đạn đã xoay đúng)
            direction = (target.transform.position - transform.position).normalized;
        }
        else
        {
            // Nếu không có target, đi thẳng theo forward (hoặc right, tùy setup)
            direction = transform.right;  // default hướng ngang bên phải
        }

        // Set projectile type dựa trên speed
        if (speed > 15f)
            projectileType = ProjectileType.Bullet;
        else if (speed > 8f)
            projectileType = ProjectileType.Rocket;
        else
            projectileType = ProjectileType.Missile;

        SetupVisualEffects();
        PlayShootSound();
    }

    void PlayShootSound()
    {
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            DestroyProjectile();
            return;
        }

        MoveProjectile();
        CheckTargetCollision();
    }

    void MoveProjectile()
    {
        switch (projectileType)
        {
            case ProjectileType.Bullet:
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

        // Xoay projectile theo hướng di chuyển (2D top-down, quay quanh trục Z)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void MoveWithTracking()
    {
        if (target != null && target.currentHealth > 0)
        {
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            direction = Vector3.Lerp(direction, targetDirection, Time.deltaTime * 2f);
        }

        transform.position += direction * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void CheckTargetCollision()
    {
        if (target == null || target.currentHealth <= 0)
        {
            DestroyProjectile();
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < 0.5f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (target != null)
        {
            target.TakeDamage(damage);
            Debug.Log($"Projectile hit {target.enemyData.enemyName} for {damage} damage!");
        }

        CreateHitEffects();
        DestroyProjectile();
    }

    void CreateHitEffects()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

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
        Debug.Log("Bullet hit effect created");
    }

    void CreateRocketHitEffect()
    {
        Debug.Log("Rocket explosion effect created");

        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D collider in nearbyEnemies)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy != target)
            {
                enemy.TakeDamage(damage / 2);
            }
        }
    }

    void CreateMissileHitEffect()
    {
        Debug.Log("Missile explosion effect created");
    }

    void SetupVisualEffects()
    {
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }

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
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
