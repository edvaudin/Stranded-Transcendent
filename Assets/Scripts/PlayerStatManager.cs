using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;

    private void SavePlayerStats()
    {
        playerStats.SaveAllStats();
    }
}
