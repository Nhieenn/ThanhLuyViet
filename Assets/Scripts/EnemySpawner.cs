using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public Transform spawnPoint;

    [Header("Waypoints")]
    public Transform pathParent;

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null || pathParent == null)
        {
            Debug.LogError("Missing references in EnemySpawner!");
            return;
        }

        // Instantiate enemy
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Gán đường path
        Enemy enemy = newEnemy.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector3[] pathPoints = new Vector3[pathParent.childCount];
            for (int i = 0; i < pathParent.childCount; i++)
            {
                pathPoints[i] = pathParent.GetChild(i).position;
            }

            enemy.SetWaypoints(pathPoints);
        }
    }
}
