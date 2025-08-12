using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<WaveConfigSO> waveConfigs;
    public Transform pathParentInScene;

    private int currentWave = 0;
    private GameWinManager gameWinManager;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        gameWinManager = FindObjectOfType<GameWinManager>();

        if (gameWinManager != null)
        {
            gameWinManager.totalWaves = waveConfigs.Count;
        }

        StartCoroutine(SpawnAllWaves());
    }

    IEnumerator SpawnAllWaves()
    {
        for (currentWave = 0; currentWave < waveConfigs.Count; currentWave++)
        {
            var waveConfig = waveConfigs[currentWave];
            Debug.Log($"Starting Wave {currentWave + 1}/{waveConfigs.Count}");

            waveConfig.pathParent = pathParentInScene;

            if (gameWinManager != null)
            {
                gameWinManager.SetEnemiesInCurrentWave(waveConfig.enemies.Count);
            }

            yield return StartCoroutine(SpawnWave(waveConfig));
            yield return StartCoroutine(WaitForWaveCompletion());

            Debug.Log($"Wave {currentWave + 1} finished!");
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnWave(WaveConfigSO waveConfig)
    {
        for (int i = 0; i < waveConfig.enemies.Count; i++)
        {
            GameObject enemyPrefab = waveConfig.enemies[i];
            GameObject newEnemy = Instantiate(enemyPrefab, waveConfig.GetWaypoint(0).position, Quaternion.identity);

            activeEnemies.Add(newEnemy);

            Enemy enemy = newEnemy.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector3[] waypoints = new Vector3[waveConfig.GetWaypointCount()];
                for (int j = 0; j < waypoints.Length; j++)
                {
                    waypoints[j] = waveConfig.GetWaypoint(j).position;
                }
                enemy.SetWaypoints(waypoints);

                StartCoroutine(MonitorEnemy(enemy));
            }

            yield return new WaitForSeconds(waveConfig.spawnInterval);
        }
    }

    IEnumerator WaitForWaveCompletion()
    {
        if (gameWinManager != null)
        {
            while (gameWinManager.enemiesKilledThisWave < gameWinManager.enemiesInCurrentWave)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            while (activeEnemies.Count > 0)
            {
                activeEnemies.RemoveAll(enemy => enemy == null);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator MonitorEnemy(Enemy enemy)
    {
        if (enemy == null) yield break;

        GameObject enemyObj = enemy.gameObject;

        while (enemyObj != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (gameWinManager != null)
        {
            gameWinManager.EnemyEliminated();
        }

        activeEnemies.Remove(enemyObj);
    }

    public int GetCurrentWave()
    {
        return currentWave + 1;
    }

    public int GetTotalWaves()
    {
        return waveConfigs != null ? waveConfigs.Count : 0;
    }

    public int GetActiveEnemyCount()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        return activeEnemies.Count;
    }

    [ContextMenu("Eliminate All Enemies")]
    public void EliminateAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies.ToArray())
        {
            if (enemy != null)
            {
                if (gameWinManager != null)
                {
                    gameWinManager.EnemyEliminated();
                }
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
        Debug.Log("All enemies eliminated manually");
    }
}
