using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    private Health playerHealth;
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] float rotationDamping = 0.2f;
    private float timeSinceLastSawPlayer = Mathf.Infinity;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerHealth = player.GetComponent<Health>();
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
        //ApplyManualRotation();
    }

    private void ApplyManualRotation()
    {
        var lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        lookPos.x = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationDamping * Time.deltaTime);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
