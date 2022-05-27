using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIncreaser : Pickup
{
    [SerializeField] [Tooltip("Duration of projectile in seconds")] float delta = 0.5f;

    protected override void Payload(GameObject player)
    {
        base.Payload(player);
        player.GetComponent<PlayerCombat>().AdjustProjectileRange(delta);
    }
}
