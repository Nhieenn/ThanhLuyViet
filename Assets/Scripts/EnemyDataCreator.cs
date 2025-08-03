using UnityEngine;

public class EnemyDataCreator : MonoBehaviour
{
    [ContextMenu("Create EnemyData")]
    public void CreateEnemyData()
    {
        EnemyData enemyData = ScriptableObject.CreateInstance<EnemyData>();
        
        // Set default values
        enemyData.enemyName = "New Enemy";
        enemyData.enemyDescription = "Enemy description";
        enemyData.health = 100;
        enemyData.attackDamage = 10;
        enemyData.moveSpeed = 2f;
        enemyData.moneyReward = 20;
        enemyData.enemyColor = Color.red;
        
        // Save asset
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(enemyData, "Assets/NewEnemyData.asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("EnemyData created successfully!");
        #endif
    }
} 