using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject player;
    private Health health;
    private Health playerHealth;
    [SerializeField] protected float chaseDistance = 10f;
    [SerializeField] protected float rotationDamping = 0.2f;
    [SerializeField] protected int meleeDamage = 1;
    [SerializeField] LootTable lootTable;

    protected float timeSinceLastSawPlayer = Mathf.Infinity;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerHealth = player.GetComponent<Health>();
        health = GetComponent<Health>();
        health.died += OnDeath;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        SampleAreaIfNotOnNavMesh();
    }

    protected virtual void Update()
    {
        if (InAttackRangeOfPlayer() && !playerHealth.IsDead)
        {
            Chase();
        }
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
    }

    protected virtual void Chase()
    {
        timeSinceLastSawPlayer = 0;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
        ApplyManualRotation();
    }

    protected void ApplyManualRotation()
    {
        var lookPos = player.transform.position - transform.position;
        var angle = Mathf.Atan2(lookPos.y, lookPos.x);
        var targetRotation = Quaternion.Euler(0, 0, (angle * Mathf.Rad2Deg) - 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationDamping * Time.deltaTime);
    }

    private void SampleAreaIfNotOnNavMesh()
    {
        if (agent.enabled && !agent.isOnNavMesh)
        {
            var position = transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, 10.0f, 0);
            position = hit.position; // usually this barely changes, if at all
            agent.Warp(position);
        }
    }

    private bool InAttackRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer < chaseDistance;
    }

    protected virtual void OnDeath()
    {
        if (lootTable.WillReceiveDrop())
        {
            Instantiate(lootTable.GetRandomPickup(), transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

    protected virtual IEnumerator DeathRoutine()
    {
        yield return null;
    }

    private void OnDisable()
    {
        health.died -= OnDeath;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(meleeDamage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
