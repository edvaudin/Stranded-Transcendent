using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Enemy
{
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float projectileSpawnGap = 2f;
    [SerializeField] protected float projectileSpeed = 20f;
    private float timeSinceFired = Mathf.Infinity;
    protected override void Chase()
    {
        base.Chase();
        if (timeSinceFired > fireRate)
        {
            Fire();
            timeSinceFired = 0;
        }
    }

    protected override void Update()
    {
        base.Update();

        timeSinceFired += Time.deltaTime;
    }

    protected virtual void Fire()
    {
        Vector3 spawnPoint = transform.position + transform.up * projectileSpawnGap;
        spawnPoint.z = transform.position.z;
        var projectileInstance = Instantiate(projectile, spawnPoint, transform.rotation);
        projectileInstance.GetComponent<Projectile>().Launch(agent.velocity, projectileSpeed);
    }
}
