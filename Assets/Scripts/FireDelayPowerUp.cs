using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDelayPowerUp : PowerUp
{
    [SerializeField] float delta = -0.2f;
    protected override void PowerUpPayload(GameObject player)
    {
        base.PowerUpPayload(player);
        player.GetComponent<PlayerCombat>().AdjustFireDelay(delta);
    }
}
