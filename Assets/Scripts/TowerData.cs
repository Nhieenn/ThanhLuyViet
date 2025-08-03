using UnityEngine;

// Enum định nghĩa loại tháp
public enum TowerType
{
    AK,     // Tháp AK - súng trường tự động
    B41,    // Tháp B41 - súng chống tăng
    SAM2    // Tháp SAM2 - tên lửa phòng không
}

[CreateAssetMenu(fileName = "TowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    [System.Serializable]
    public class TowerLevel
    {
        public Sprite sprite;
        public int damage = 10;
        public float range = 3f;
        public float fireRate = 1f;
        public int upgradeCost = 50;
        public int sellValue = 30; // Số tiền nhận được khi bán tháp
        public string description = "Level description";
    }
    
    [Header("Tower Information")]
    public string towerName = "Tower";
    public string towerDescription = "Tower description";
    public int buildCost = 100;
    
    [Header("Tower Levels")]
    public TowerLevel[] levels = new TowerLevel[3]; // 3 level: 0, 1, 2
    
    [Header("Tower Type")]
    public TowerType towerType = TowerType.AK;
    
    [Header("Visual Effects")]
    public Color towerColor = Color.white;
    public GameObject upgradeEffectPrefab;
    public AudioClip upgradeSound;
} 