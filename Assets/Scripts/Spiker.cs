using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiker : Ranger
{
    protected override void Fire()
    {
        Vector3 spawnPoint = transform.position + transform.up * projectileSpawnGap;
        spawnPoint.z = transform.position.z;
        var angle = transform.rotation;
        for (int i = -2; i < 2; i++)
        {
            var projectileInstance = Instantiate(projectile, spawnPoint, angle * Quaternion.Euler(0, 0, i * 20));
            projectileInstance.GetComponent<Projectile>().Launch(agent.velocity, projectileSpeed);
        }
        
    }
}
