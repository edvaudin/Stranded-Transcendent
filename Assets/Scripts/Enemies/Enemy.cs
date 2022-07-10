using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject player;
    protected Health health;
    public static Action died;
    [SerializeField] protected float chaseDistance = 10f;
    [SerializeField] protected float rotationDamping = 0.2f;
    [SerializeField] protected int meleeDamage = 1;
    [SerializeField] LootTable lootTable;

    [Header("Audio")]
    [SerializeField] AudioClip hurt;
    [SerializeField] GameObject deathParticles;
    protected AudioSource audioSource;

    [Header("Blood")]
    [SerializeField] List<Sprite> bloodSprites;
    [SerializeField] GameObject bloodDecal;

    protected float timeSinceLastSawPlayer = Mathf.Infinity;
    protected bool shouldAttack = true;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        health.died += OnDeath;
        health.changed += OnHurt;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        SampleAreaIfNotOnNavMesh();
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) { return; }
        if (InAttackRangeOfPlayer() && shouldAttack)
        {
            Chase();
        }
        UpdateTimers();
    }

    private void OnHurt(int value)
    {
        audioSource.PlayOneShot(hurt);
        SpawnBlood();
    }

    private void SpawnBlood()
    {
        SpriteRenderer sr = Instantiate(bloodDecal, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
        sr.sprite = bloodSprites[UnityEngine.Random.Range(0, bloodSprites.Count)];
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
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        if (lootTable.WillReceiveDrop())
        {
            Instantiate(lootTable.GetRandomPickup(), transform.position, Quaternion.identity);
        }
        died?.Invoke();
        Destroy(gameObject);
    }

    protected virtual IEnumerator DeathRoutine()
    {
        yield return null;
    }

    private void OnDisable()
    {
        health.died -= OnDeath;
        health.changed -= OnHurt;
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
