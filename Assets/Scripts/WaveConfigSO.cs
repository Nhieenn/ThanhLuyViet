using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Defense/ScriptableObjects/WaveConfig")]
public class WaveConfigSO : ScriptableObject
{
    public List<GameObject> enemies;
    public float spawnInterval = 1f;

    // KHÔNG serialize đường path trong SO
    [HideInInspector] public Transform pathParent;

    public Transform GetWaypoint(int index)
    {
        return pathParent.GetChild(index);
    }

    public int GetWaypointCount()
    {
        return pathParent.childCount;
    }
}
