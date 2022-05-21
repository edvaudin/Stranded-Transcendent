using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new loot table")]
public class LootTable : ScriptableObject
{
    [SerializeField] List<GameObject> pickups;
    public GameObject GetRandomPickup()
    {
        return pickups[Random.Range(0, pickups.Count)];
    }
}
