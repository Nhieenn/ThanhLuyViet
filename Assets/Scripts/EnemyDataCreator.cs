using UnityEngine;

public class EnemyDataCreator : MonoBehaviour
{
    [Header("Enemy Creation")]
    public bool createEnemiesOnStart = true;
    public string enemyFolderPath = "Assets/EnemyData/";
    
    [Header("Enemy Templates")]
    public EnemyTemplate[] enemyTemplates;
    
    [System.Serializable]
    public class EnemyTemplate
    {
        public string enemyName;
        public EnemyType enemyType;
        public int health;
        public int attackDamage;
        public float moveSpeed;
        public int moneyReward;
        public Color enemyColor;
        public string description;
    }
    
    void Start()
    {
        if (createEnemiesOnStart)
        {
            CreateEnemyTemplates();
            CreateAllEnemies();
        }
    }
    
    void CreateEnemyTemplates()
    {
        enemyTemplates = new EnemyTemplate[]
        {
            // Infantry - Bộ binh
            new EnemyTemplate
            {
                enemyName = "Infantry",
                enemyType = EnemyType.Infantry,
                health = 100,
                attackDamage = 10,
                moveSpeed = 2f,
                moneyReward = 20,
                enemyColor = Color.red,
                description = "Basic infantry unit. Fast but weak."
            },
            
            // Tank - Xe tăng
            new EnemyTemplate
            {
                enemyName = "Tank",
                enemyType = EnemyType.Tank,
                health = 300,
                attackDamage = 25,
                moveSpeed = 1f,
                moneyReward = 50,
                enemyColor = Color.gray,
                description = "Heavy armored tank. Slow but strong."
            },
            
            // Aircraft - Máy bay
            new EnemyTemplate
            {
                enemyName = "Aircraft",
                enemyType = EnemyType.Aircraft,
                health = 150,
                attackDamage = 15,
                moveSpeed = 3f,
                moneyReward = 35,
                enemyColor = Color.blue,
                description = "Fast aircraft. Can fly over obstacles."
            },
            
            // Boss - Boss
            new EnemyTemplate
            {
                enemyName = "Boss",
                enemyType = EnemyType.Boss,
                health = 1000,
                attackDamage = 50,
                moveSpeed = 1.5f,
                moneyReward = 200,
                enemyColor = Color.purple,
                description = "Powerful boss unit. Very strong and tough."
            },
            
            // Elite Infantry - Bộ binh tinh nhuệ
            new EnemyTemplate
            {
                enemyName = "Elite Infantry",
                enemyType = EnemyType.Infantry,
                health = 200,
                attackDamage = 20,
                moveSpeed = 2.5f,
                moneyReward = 40,
                enemyColor = Color.orange,
                description = "Elite infantry unit. Stronger than regular infantry."
            },
            
            // Heavy Tank - Xe tăng hạng nặng
            new EnemyTemplate
            {
                enemyName = "Heavy Tank",
                enemyType = EnemyType.Tank,
                health = 500,
                attackDamage = 40,
                moveSpeed = 0.8f,
                moneyReward = 80,
                enemyColor = Color.darkGray,
                description = "Heavy tank with extra armor. Very slow but extremely tough."
            },
            
            // Fighter Jet - Máy bay chiến đấu
            new EnemyTemplate
            {
                enemyName = "Fighter Jet",
                enemyType = EnemyType.Aircraft,
                health = 250,
                attackDamage = 30,
                moveSpeed = 4f,
                moneyReward = 60,
                enemyColor = Color.cyan,
                description = "Advanced fighter jet. Very fast and dangerous."
            },
            
            // Mega Boss - Boss khổng lồ
            new EnemyTemplate
            {
                enemyName = "Mega Boss",
                enemyType = EnemyType.Boss,
                health = 2000,
                attackDamage = 100,
                moveSpeed = 1f,
                moneyReward = 500,
                enemyColor = Color.magenta,
                description = "Ultimate boss unit. Extremely powerful and nearly indestructible."
            }
        };
    }
    
    [ContextMenu("Create All Enemies")]
    public void CreateAllEnemies()
    {
        foreach (EnemyTemplate template in enemyTemplates)
        {
            CreateEnemyData(template);
        }
        
        Debug.Log($"Created {enemyTemplates.Length} enemy data files!");
    }
    
    void CreateEnemyData(EnemyTemplate template)
    {
        // Tạo EnemyData ScriptableObject
        EnemyData enemyData = ScriptableObject.CreateInstance<EnemyData>();
        
        // Set properties
        enemyData.enemyName = template.enemyName;
        enemyData.enemyDescription = template.description;
        enemyData.enemyType = template.enemyType;
        enemyData.health = template.health;
        enemyData.attackDamage = template.attackDamage;
        enemyData.moveSpeed = template.moveSpeed;
        enemyData.moneyReward = template.moneyReward;
        enemyData.enemyColor = template.enemyColor;
        
        // Set special properties based on type
        switch (template.enemyType)
        {
            case EnemyType.Tank:
                enemyData.isArmored = true;
                enemyData.canFly = false;
                break;
                
            case EnemyType.Aircraft:
                enemyData.canFly = true;
                enemyData.isArmored = false;
                break;
                
            case EnemyType.Boss:
                enemyData.isArmored = true;
                enemyData.canHeal = true;
                enemyData.healRate = 5f;
                break;
                
            default:
                enemyData.isArmored = false;
                enemyData.canFly = false;
                break;
        }
        
        // Set detection range based on type
        switch (template.enemyType)
        {
            case EnemyType.Aircraft:
                enemyData.detectionRange = 8f;
                break;
            case EnemyType.Boss:
                enemyData.detectionRange = 10f;
                break;
            default:
                enemyData.detectionRange = 5f;
                break;
        }
        
        // Tạo file name
        string fileName = template.enemyName.Replace(" ", "");
        string filePath = enemyFolderPath + fileName + ".asset";
        
        // Lưu file
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(enemyData, filePath);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log($"Created enemy data: {filePath}");
        #else
        Debug.Log($"Enemy data created in memory: {template.enemyName}");
        #endif
    }
    
    [ContextMenu("Create Single Enemy")]
    public void CreateSingleEnemy()
    {
        if (enemyTemplates.Length > 0)
        {
            CreateEnemyData(enemyTemplates[0]);
        }
    }
    
    [ContextMenu("Clear Enemy Templates")]
    public void ClearEnemyTemplates()
    {
        enemyTemplates = new EnemyTemplate[0];
        Debug.Log("Cleared enemy templates!");
    }
    
    void OnGUI()
    {
        if (!Application.isPlaying) return;
        
        GUILayout.BeginArea(new Rect(10, 220, 300, 200));
        GUILayout.Label("Enemy Data Creator");
        
        if (GUILayout.Button("Create All Enemies"))
        {
            CreateAllEnemies();
        }
        
        if (GUILayout.Button("Create Single Enemy"))
        {
            CreateSingleEnemy();
        }
        
        if (GUILayout.Button("Clear Templates"))
        {
            ClearEnemyTemplates();
        }
        
        GUILayout.Label($"Templates: {enemyTemplates.Length}");
        
        GUILayout.EndArea();
    }
} 