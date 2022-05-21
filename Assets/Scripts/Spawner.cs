using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    [SerializeField] float spawnRate = 5f;
    [SerializeField] float offScreenBuffer = 5f;
    private float timeSinceLastSpawn = Mathf.Infinity;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (timeSinceLastSpawn > spawnRate)
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
}
