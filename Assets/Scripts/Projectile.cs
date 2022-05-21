using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audioSource;
    [SerializeField] float speed = 20f;
    [SerializeField] int damage = 1;
    [SerializeField] float minPitch = 0.9f;
    [SerializeField] float maxPitch = 1.1f;
    private float rangeTimer = 0;
    [SerializeField] float rangeInSeconds = 1.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        rangeTimer += Time.deltaTime;
        if (rangeTimer > rangeInSeconds)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void Launch(Vector3 playerVelocity)
    {
        rb.AddForce((transform.up * speed) + playerVelocity.normalized, ForceMode2D.Impulse);

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Health>(out _))
        {
            Destroy(gameObject);
        }
    }
}
