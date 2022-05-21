using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new loot table")]
public class LootTable : ScriptableObject
{
    [SerializeField] float dropRate = 0.5f;
    [SerializeField] List<GameObject> pickups;

    public bool WillReceiveDrop()
    {
        return Random.Range(0f, 1f) > dropRate;
    }

    public GameObject GetRandomPickup()
    {
        return pickups[Random.Range(0, pickups.Count)];
    }
}
