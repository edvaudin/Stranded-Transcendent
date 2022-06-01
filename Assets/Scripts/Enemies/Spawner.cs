using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [Header("Enemies that can spawn")]
    [SerializeField] List<GameObject> enemies;

    [Header("Spawn Rate")]
    [SerializeField] float spawnRate = 5f;
    [SerializeField] float spawnAcceleration = 0.01f;
    [SerializeField, Range(0f, 1f)] float minSpawnRate = 0.2f;
    [SerializeField] bool increasing = false;
    private float timeSinceLastSpawn = Mathf.Infinity;

    [Header("Spawn Locations")]
    [SerializeField] float offScreenBuffer = 5f;

    private Camera cam;
    private bool bossAlive = false;

    private void Awake()
    {
        Boss.bossSpawned += () => bossAlive = true;
        Boss.bossDied += () => bossAlive = false;
        cam = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) { return; }
        if (increasing)
        {
            spawnRate = Mathf.Max(spawnRate - (spawnAcceleration * Time.deltaTime), minSpawnRate);
        }
        SpawnEnemiesAtRate();
    }

    private void SpawnEnemiesAtRate()
    {
        if (timeSinceLastSpawn > spawnRate && !bossAlive)
        {
            Spawn();
            timeSinceLastSpawn = 0;
        }
        timeSinceLastSpawn += Time.deltaTime;
    }

    private void Spawn()
    {
        GameObject enemy = enemies[Random.Range(0, enemies.Count)];
        Vector2 position = GetRandomPosition();
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            Instantiate(enemy, position, Quaternion.identity);
            Debug.Log("Spawned");
        }
        else
        {
            Debug.Log("Point was not on navmesh");
        }
    }

    // ISSUE: If the player is in a corner of the room, then spawn rate will go down.
    private Vector2 GetRandomPosition()
    {
        float spawnY = Random.Range(0, cam.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);

        Vector2 spawnPosition = new Vector2(cam.ScreenToWorldPoint(
            new Vector2(Random.Range(0, 2) == 1 ? Screen.width + offScreenBuffer : 0 - offScreenBuffer, 0)).x, spawnY);
        return spawnPosition;

    }

    private void OnDisable()
    {
        Boss.bossSpawned -= () => bossAlive = true;
        Boss.bossDied -= () => bossAlive = false;
    }
}
