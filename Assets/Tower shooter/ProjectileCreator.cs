using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    [Header("Projectile Prefabs")]
    public GameObject bulletPrefab;      // AK47
    public GameObject rocketPrefab;      // B41  
    public GameObject missilePrefab;     // SAM2
    
    [Header("Effect Prefabs")]
    public GameObject bulletHitEffect;
    public GameObject rocketExplosionEffect;
    public GameObject missileExplosionEffect;
    public GameObject muzzleFlashEffect;
    
    [Header("Audio Clips")]
    public AudioClip bulletShootSound;
    public AudioClip rocketShootSound;
    public AudioClip missileShootSound;
    public AudioClip bulletHitSound;
    public AudioClip rocketHitSound;
    public AudioClip missileHitSound;
    
    public static ProjectileCreator Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Tạo projectile prefab cho AK47
    public GameObject CreateBulletPrefab()
    {
        GameObject bullet = new GameObject("Bullet_AK47");
        
        // Add components
        SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
        CircleCollider2D collider = bullet.AddComponent<CircleCollider2D>();
        Projectile projectile = bullet.AddComponent<Projectile>();
        TrailRenderer trail = bullet.AddComponent<TrailRenderer>();
        
        // Setup sprite (create a simple circle sprite)
        spriteRenderer.sprite = CreateCircleSprite();
        spriteRenderer.color = Color.yellow;
        spriteRenderer.sortingOrder = 10;
        
        // Setup collider
        collider.isTrigger = true;
        collider.radius = 0.1f;
        
        // Setup trail
        trail.time = 0.1f;
        trail.startWidth = 0.05f;
        trail.endWidth = 0.01f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = Color.yellow;
        trail.endColor = Color.clear;
        
        // Setup projectile
        projectile.hitEffectPrefab = bulletHitEffect;
        projectile.hitSound = bulletHitSound;
        projectile.trailRenderer = trail;
        projectile.projectileType = Projectile.ProjectileType.Bullet;
        projectile.speed = 20f;
        projectile.lifetime = 3f;
        
        return bullet;
    }
    
    // Tạo projectile prefab cho B41
    public GameObject CreateRocketPrefab()
    {
        GameObject rocket = new GameObject("Rocket_B41");
        
        // Add components
        SpriteRenderer spriteRenderer = rocket.AddComponent<SpriteRenderer>();
        CircleCollider2D collider = rocket.AddComponent<CircleCollider2D>();
        Projectile projectile = rocket.AddComponent<Projectile>();
        TrailRenderer trail = rocket.AddComponent<TrailRenderer>();
        
        // Setup sprite
        spriteRenderer.sprite = CreateCircleSprite();
        spriteRenderer.color = Color.red;
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        
        // Setup collider
        collider.isTrigger = true;
        collider.radius = 0.15f;
        
        // Setup trail
        trail.time = 0.2f;
        trail.startWidth = 0.1f;
        trail.endWidth = 0.02f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = Color.red;
        trail.endColor = Color.clear;
        
        // Setup projectile
        projectile.hitEffectPrefab = rocketExplosionEffect;
        projectile.hitSound = rocketHitSound;
        projectile.trailRenderer = trail;
        projectile.projectileType = Projectile.ProjectileType.Rocket;
        projectile.speed = 12f;
        projectile.lifetime = 4f;
        
        return rocket;
    }
    
    // Tạo projectile prefab cho SAM2
    public GameObject CreateMissilePrefab()
    {
        GameObject missile = new GameObject("Missile_SAM2");
        
        // Add components
        SpriteRenderer spriteRenderer = missile.AddComponent<SpriteRenderer>();
        CircleCollider2D collider = missile.AddComponent<CircleCollider2D>();
        Projectile projectile = missile.AddComponent<Projectile>();
        TrailRenderer trail = missile.AddComponent<TrailRenderer>();
        
        // Setup sprite
        spriteRenderer.sprite = CreateCircleSprite();
        spriteRenderer.color = Color.cyan;
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        
        // Setup collider
        collider.isTrigger = true;
        collider.radius = 0.12f;
        
        // Setup trail
        trail.time = 0.3f;
        trail.startWidth = 0.08f;
        trail.endWidth = 0.01f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = Color.cyan;
        trail.endColor = Color.clear;
        
        // Setup projectile
        projectile.hitEffectPrefab = missileExplosionEffect;
        projectile.hitSound = missileHitSound;
        projectile.trailRenderer = trail;
        projectile.projectileType = Projectile.ProjectileType.Missile;
        projectile.speed = 8f;
        projectile.lifetime = 5f;
        
        return missile;
    }
    
    // Tạo sprite hình tròn đơn giản
    private Sprite CreateCircleSprite()
    {
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        Vector2 center = new Vector2(16, 16);
        float radius = 12f;
        
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    pixels[y * 32 + x] = Color.white;
                }
                else
                {
                    pixels[y * 32 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
    }
    
    // Helper methods để lấy projectile prefab theo tower type
    public GameObject GetProjectilePrefab(TowerType towerType)
    {
        switch (towerType)
        {
            case TowerType.AK:
                return bulletPrefab != null ? bulletPrefab : CreateBulletPrefab();
            case TowerType.B41:
                return rocketPrefab != null ? rocketPrefab : CreateRocketPrefab();
            case TowerType.SAM2:
                return missilePrefab != null ? missilePrefab : CreateMissilePrefab();
            default:
                return bulletPrefab;
        }
    }
    
    public AudioClip GetShootSound(TowerType towerType)
    {
        switch (towerType)
        {
            case TowerType.AK:
                return bulletShootSound;
            case TowerType.B41:
                return rocketShootSound;
            case TowerType.SAM2:
                return missileShootSound;
            default:
                return bulletShootSound;
        }
    }
} 