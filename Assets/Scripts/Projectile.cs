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
    [SerializeField] protected GameObject impactParticles;
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

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public virtual void Launch(Vector3 velocity, float range)
    {
        rangeInSeconds = range;
        Vector3 adjustment = velocity.magnitude > 1f ? velocity.normalized : Vector3.zero;
        rb.AddForce((transform.up * speed) + adjustment, ForceMode2D.Impulse);

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
        Instantiate(impactParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (!collision.transform.parent.TryGetComponent<Health>(out _))
            {
                Instantiate(impactParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
