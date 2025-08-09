using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count = 1;
    public float spawnInterval = 1f;
}
