using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    protected virtual void PowerUpPayload(GameObject player)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PowerUpPayload(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
