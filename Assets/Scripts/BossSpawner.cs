using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] List<BossMilestone> bosses;
    private int enemyDeathCount = 0;

    [SerializeField] float offScreenBuffer = 5f;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        Enemy.died += OnEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.died -= OnEnemyDeath;
    }

    private void OnEnemyDeath()
    {
        foreach (BossMilestone milestone in bosses)
        {
            if (enemyDeathCount >= milestone.killCountRequired && !milestone.spawned)
            {
                Spawn(milestone.bossPrefab);
                milestone.spawned = true;
            }
        }
        enemyDeathCount++;
    }

    private void Spawn(GameObject boss)
    {
        Vector2 position = GetRandomPosition();
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            Instantiate(boss, position, Quaternion.identity);
            Debug.Log("Spawned");
        }
        else
        {
            Debug.Log("Point was not on navmesh");
        }
    }

    private Vector2 GetRandomPosition()
    {
        float spawnY = Random.Range(0, cam.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);

        Vector2 spawnPosition = new Vector2(cam.ScreenToWorldPoint(
            new Vector2(Random.Range(0, 2) == 1 ? Screen.width + offScreenBuffer : 0 - offScreenBuffer, 0)).x, spawnY);
        return spawnPosition;

    }
}

[System.Serializable]
public class BossMilestone
{
    public int killCountRequired;
    public GameObject bossPrefab;
    public bool spawned = false;
}
