using UnityEngine;

// Enum định nghĩa loại kẻ địch
public enum EnemyType
{
    Infantry,   // Bộ binh
    Tank,       // Xe tăng
    Aircraft,   // Máy bay
    Boss        // Boss
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Tower Defense/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Information")]
    public string enemyName = "Enemy";
    public string enemyDescription = "Enemy description";
    public EnemyType enemyType = EnemyType.Infantry;

    [Header("Stats")]
    public int health = 100;
    public int attackDamage = 10;
    public float moveSpeed = 2f;
    public int moneyReward = 20;

    [Header("Visual")]
    public Sprite sprite;
    public Color enemyColor = Color.red;

    [Header("Visual Effects")]
    public GameObject deathEffectPrefab;
    public AudioClip deathSound;
    public AudioClip hitSound;

    [Header("AI Behavior")]
    public float detectionRange = 5f;
    public bool canFly = false; // Cho máy bay
    public bool isArmored = false; // Cho xe tăng

    [Header("Special Abilities")]
    public bool canHeal = false;
    public float healRate = 0f;
    public bool canSpawnMinions = false;
    public GameObject minionPrefab;
}