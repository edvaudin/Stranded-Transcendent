using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dasher : Enemy
{
    private bool isDashing = false;
    protected override void Chase()
    {
        timeSinceLastSawPlayer = 0;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
        if (!isDashing)
        {
            StartCoroutine(Dash());
        }
        ApplyManualRotation();
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startingSpeed = agent.speed;
        while (Vector3.Distance(transform.position, agent.destination) > 5)
        {
            agent.speed += Vector3.Distance(transform.position, agent.destination);
            yield return null;
        }
        agent.speed = startingSpeed;
        isDashing = false;
    }
}
