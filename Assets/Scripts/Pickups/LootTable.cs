using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new loot table")]
public class LootTable : ScriptableObject
{
    [SerializeField] float dropRate = 0.5f;
    [SerializeField] List<Pickup> pickups;

    public bool WillReceiveDrop()
    {
        return Random.Range(0f, 1f) > dropRate;
    }

    public GameObject GetRandomPickup()
    {
        float total = 0;
        Dictionary<Pickup, float> pickupDictionary= new Dictionary<Pickup, float>();
        foreach (Pickup pickup in pickups)
        {
            pickupDictionary.Add(pickup, total + pickup.pickupData.dropRate);
            total += pickup.pickupData.dropRate;
        }
        float random = Random.Range(0, total);
        foreach (KeyValuePair<Pickup, float> pair in pickupDictionary)
        {
            if (random <= pair.Value)
            {
                return pair.Key.gameObject;
            }
        }
        Debug.LogWarning("Could not get random pickup");
        return pickups[0].gameObject;
    }
}
