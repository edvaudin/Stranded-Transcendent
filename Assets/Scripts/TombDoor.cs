using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombDoor : MonoBehaviour
{
    public static bool PlayerInTomb { get; private set; } = false;
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerInTomb = !PlayerInTomb;
            Debug.Log("Player in tomb? " + PlayerInTomb);
        }
    }
}
