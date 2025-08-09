using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    public List<WaveConfigSO> waves;
}
