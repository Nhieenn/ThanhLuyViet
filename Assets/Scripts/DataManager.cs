using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = "ScriptableObject/DataManager", order = 1)]
public class DataManager : ScriptableObject
{
    [Header("GameManager")]
    public int Currlevel;

    public int MaxWave;
    public int CurWave;

    public int EnemyWave;
    public int CurrEnemy;

    public int MaxHp;
    public int CurrHp;

    public int CurrCoin;

}
