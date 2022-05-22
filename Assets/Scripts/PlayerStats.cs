using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new Player Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] List<Stat> stats;

    public void SaveAllStats()
    {
        foreach (Stat stat in stats)
        {
            stat.SetBaseToValue();
        }
    }
}
