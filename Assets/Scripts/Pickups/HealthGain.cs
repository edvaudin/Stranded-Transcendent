using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGain : Pickup
{
    [SerializeField] int healthGain = 1;
    protected override void Payload(GameObject player)
    {
        base.Payload(player);
        player.GetComponent<PlayerCombat>().AdjustBaseHealth(healthGain);
    }
}
