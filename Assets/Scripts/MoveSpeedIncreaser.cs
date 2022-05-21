using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedIncreaser : Pickup
{
    [SerializeField] float delta = 1f;
    protected override void Payload(GameObject player)
    {
        player.GetComponent<PlayerMovement>().AdjustMoveSpeed(delta);
    }
}
