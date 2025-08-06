using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    public float slowAmount = 0.5f; // Tốc độ còn lại (0.5 = giảm 50%)
    public float slowDuration = 2f;  // Thời gian làm chậm

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            StartCoroutine(ApplySlow(enemy));
        }
    }

    private System.Collections.IEnumerator ApplySlow(Enemy enemy)
    {
        float originalSpeedMultiplier = enemy.speedMultiplier;
        enemy.speedMultiplier *= slowAmount;
        yield return new WaitForSeconds(slowDuration);
        enemy.speedMultiplier = originalSpeedMultiplier;
    }
}