using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected AudioSource audioSource;
    protected Collider2D collider;
    [SerializeField] protected float speed = 20f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float minPitch = 0.9f;
    [SerializeField] protected float maxPitch = 1.1f;
    protected float rangeTimer = 0;
    [SerializeField] protected float rangeInSeconds = 1.5f;

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

    public virtual void Launch(Vector3 velocity)
    {   
        rb.AddForce((transform.up * speed) + velocity.normalized, ForceMode2D.Impulse);

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (!collision.transform.parent.TryGetComponent<Health>(out _))
            {
                Destroy(gameObject);
            }
        }
        
    }
}
