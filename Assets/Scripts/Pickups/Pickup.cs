using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSound;
    public PickupData pickupData;
    public static Action<PickupData> pickupAdded;
    private AudioSource audioSource;
    private SpriteRenderer sr;
    private Collider2D col;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    protected virtual void Payload(GameObject player)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Payload(collision.gameObject);
            pickupAdded?.Invoke(pickupData);
            StartCoroutine(DestroyPickup());
        }
    }

    private IEnumerator DestroyPickup()
    {
        audioSource.PlayOneShot(pickUpSound);
        sr.enabled = false;
        col.enabled = false;
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
