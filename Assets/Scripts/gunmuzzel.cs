using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectilePrefab;    // Prefab viên đạn (có gắn script Projectile)
    public Transform muzzlePoint;          // Vị trí nòng súng (empty GameObject con của gun)
    public Enemy currentTarget;             // Target bắn
    public int damage = 10;
    public float projectileSpeed = 20f;

    // Ví dụ: gọi hàm này để bắn đạn
    public void Shoot()
    {
        if (projectilePrefab != null && muzzlePoint != null && currentTarget != null)
        {
            // Spawn viên đạn tại vị trí và hướng của nòng súng
            GameObject projObj = Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);

            // Lấy component Projectile để khởi tạo
            Projectile proj = projObj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.Initialize(currentTarget, damage, projectileSpeed);
            }
        }
    }
}
