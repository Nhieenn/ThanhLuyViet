using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<WaveConfigSO> waveConfigs;                 // Danh sách các wave
    public Transform pathParentInScene;                    // Path dùng chung (gán từ scene)

    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    IEnumerator SpawnAllWaves()
    {
        for (currentWave = 0; currentWave < waveConfigs.Count; currentWave++)
        {
            var waveConfig = waveConfigs[currentWave];

            // Gán đường path từ scene vào wave
            waveConfig.pathParent = pathParentInScene;

            yield return StartCoroutine(SpawnWave(waveConfig));
            yield return new WaitForSeconds(2f); // Delay giữa các wave, tuỳ chỉnh
        }
    }

    IEnumerator SpawnWave(WaveConfigSO waveConfig)
    {
        for (int i = 0; i < waveConfig.enemies.Count; i++)
        {
            GameObject enemyPrefab = waveConfig.enemies[i];
            GameObject newEnemy = Instantiate(enemyPrefab, waveConfig.GetWaypoint(0).position, Quaternion.identity);

            Enemy enemy = newEnemy.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector3[] waypoints = new Vector3[waveConfig.GetWaypointCount()];
                for (int j = 0; j < waypoints.Length; j++)
                {
                    waypoints[j] = waveConfig.GetWaypoint(j).position;
                }
                enemy.SetWaypoints(waypoints);
            }

            yield return new WaitForSeconds(waveConfig.spawnInterval);
        }
    }
}
