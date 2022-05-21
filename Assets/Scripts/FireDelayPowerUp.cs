using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDelayPowerUp : Pickup
{
    [SerializeField] float delta = -0.2f;
    protected override void Payload(GameObject player)
    {
        base.Payload(player);
        player.GetComponent<PlayerCombat>().AdjustFireDelay(delta);
    }
}
