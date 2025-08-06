using UnityEngine;

public class TowerSetupHelper : MonoBehaviour
{
    [Header("Auto Setup")]
    public bool autoSetupOnStart = true;
    
    void Start()
    {
        if (autoSetupOnStart)
        {
            SetupAllTowers();
        }
    }
    
    // Setup shooting system cho tất cả towers trong scene
    public void SetupAllTowers()
    {
        Tower[] towers = FindObjectsOfType<Tower>();
        
        foreach (Tower tower in towers)
        {
            SetupTowerShooting(tower);
        }
        
        Debug.Log($"Setup shooting system for {towers.Length} towers");
    }
    
    // Setup shooting system cho 1 tower
    public void SetupTowerShooting(Tower tower)
    {
        if (tower == null) return;
        
        // Kiểm tra xem đã có TowerShooter chưa
        TowerShooter shooter = tower.GetComponent<TowerShooter>();
        if (shooter == null)
        {
            // Thêm TowerShooter component
            shooter = tower.gameObject.AddComponent<TowerShooter>();
        }
        
        // Setup projectile prefab từ TowerData
        if (tower.towerData != null)
        {
            // Tự động tạo projectile prefab nếu chưa có
            if (tower.towerData.projectilePrefab == null)
            {
                tower.towerData.projectilePrefab = GetProjectilePrefabForTower(tower.towerData.towerType);
            }
            
            // Setup shoot sound
            if (tower.towerData.shootSound == null)
            {
                tower.towerData.shootSound = GetShootSoundForTower(tower.towerData.towerType);
            }
            
            // Setup projectile speed
            tower.towerData.projectileSpeed = GetProjectileSpeedForTower(tower.towerData.towerType);
        }
        
        Debug.Log($"Setup shooting for {tower.towerData.towerName}");
    }
    
    // Lấy projectile prefab theo tower type
    private GameObject GetProjectilePrefabForTower(TowerType towerType)
    {
        if (ProjectileCreator.Instance != null)
        {
            return ProjectileCreator.Instance.GetProjectilePrefab(towerType);
        }
        
        // Fallback: tạo prefab mới
        switch (towerType)
        {
            case TowerType.AK:
                return CreateSimpleBulletPrefab();
            case TowerType.B41:
                return CreateSimpleRocketPrefab();
            case TowerType.SAM2:
                return CreateSimpleMissilePrefab();
            default:
                return CreateSimpleBulletPrefab();
        }
    }
    
    // Lấy shoot sound theo tower type
    private AudioClip GetShootSoundForTower(TowerType towerType)
    {
        if (ProjectileCreator.Instance != null)
        {
            return ProjectileCreator.Instance.GetShootSound(towerType);
        }
        return null; // Không có sound mặc định
    }
    
    // Lấy projectile speed theo tower type
    private float GetProjectileSpeedForTower(TowerType towerType)
    {
        switch (towerType)
        {
            case TowerType.AK:
                return 20f; // Nhanh
            case TowerType.B41:
                return 12f; // Trung bình
            case TowerType.SAM2:
                return 8f;  // Chậm nhưng tracking
            default:
                return 10f;
        }
    }
    
    // Tạo simple bullet prefab
    private GameObject CreateSimpleBulletPrefab()
    {
        GameObject bullet = new GameObject("SimpleBullet");
        
        SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
        CircleCollider2D collider = bullet.AddComponent<CircleCollider2D>();
        Projectile projectile = bullet.AddComponent<Projectile>();
        
        // Setup sprite
        spriteRenderer.sprite = CreateSimpleCircleSprite();
        spriteRenderer.color = Color.yellow;
        spriteRenderer.sortingOrder = 10;
        
        // Setup collider
        collider.isTrigger = true;
        collider.radius = 0.1f;
        
        // Setup projectile
        projectile.projectileType = Projectile.ProjectileType.Bullet;
        projectile.speed = 20f;
        projectile.lifetime = 3f;
        
        return bullet;
    }
    
    // Tạo simple rocket prefab
    private GameObject CreateSimpleRocketPrefab()
    {
        GameObject rocket = new GameObject("SimpleRocket");
        
        SpriteRenderer spriteRenderer = rocket.AddComponent<SpriteRenderer>();
        CircleCollider2D collider = rocket.AddComponent<CircleCollider2D>();
        Projectile projectile = rocket.AddComponent<Projectile>();
        
        // Setup sprite
        spriteRenderer.sprite = CreateSimpleCircleSprite();
        spriteRenderer.color = Color.red;
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        
        // Setup collider
        collider.isTrigger = true;
        collider.radius = 0.15f;
        
        // Setup projectile
        projectile.projectileType = Projectile.ProjectileType.Rocket;
        projectile.speed = 12f;
        projectile.lifetime = 4f;
        
        return rocket;
    }
    
    // Tạo simple missile prefab
    private GameObject CreateSimpleMissilePrefab()
    {
        GameObject missile = new GameObject("SimpleMissile");
        
        SpriteRenderer spriteRenderer = missile.AddComponent<SpriteRenderer>();
        CircleCollider2D collider = missile.AddComponent<CircleCollider2D>();
        Projectile projectile = missile.AddComponent<Projectile>();
        
        // Setup sprite
        spriteRenderer.sprite = CreateSimpleCircleSprite();
        spriteRenderer.color = Color.cyan;
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        
        // Setup collider
        collider.isTrigger = true;
        collider.radius = 0.12f;
        
        // Setup projectile
        projectile.projectileType = Projectile.ProjectileType.Missile;
        projectile.speed = 8f;
        projectile.lifetime = 5f;
        
        return missile;
    }
    
    // Tạo simple circle sprite
    private Sprite CreateSimpleCircleSprite()
    {
        Texture2D texture = new Texture2D(16, 16);
        Color[] pixels = new Color[16 * 16];
        
        Vector2 center = new Vector2(8, 8);
        float radius = 6f;
        
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    pixels[y * 16 + x] = Color.white;
                }
                else
                {
                    pixels[y * 16 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
    }
    
    // Context menu để setup manual
    [ContextMenu("Setup All Towers")]
    public void SetupAllTowersContext()
    {
        SetupAllTowers();
    }
} 