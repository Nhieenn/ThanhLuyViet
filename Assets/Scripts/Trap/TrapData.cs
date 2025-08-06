using UnityEngine;

public enum TrapType { Spike, Mine }

[CreateAssetMenu(fileName = "TrapData", menuName = "Tower Defense/Trap Data")]
public class TrapData : ScriptableObject
{
    public string trapName;
    public TrapType trapType;
    public Sprite icon;
    public int cost;
    public GameObject trapPrefab;
    public string description;
}