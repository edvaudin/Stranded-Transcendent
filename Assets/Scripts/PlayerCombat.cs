using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [field: SerializeField] public float FireDelay { get; private set; } = 0.5f;
    [SerializeField] float minFireDelay = 0.1f;
    [SerializeField] float maxFireDelay = 1f;
    private float timeSinceLastFired = Mathf.Infinity;
    [SerializeField] float projectileSpawnGap = 2f;
    [SerializeField] AudioClip hurt;
    private PlayerInput playerInput;
    private PlayerControls controls;
    private PlayerMovement playerMovement;
    private Health health;
    private AudioSource audioSource;
    private Vector2 currentFireDirection = Vector2.up;
    private bool firing = false;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (!playerInput.ControlsInitialized) { playerInput.InitializeControls(); }
        controls = playerInput.PlayerController;
        controls.MK.Fire.performed += OnFire;
        health.died += OnDeath;
        health.changed += PlayHurt;
    }

    private void Update()
    {
        if (firing && timeSinceLastFired > FireDelay)
        {
            Fire(currentFireDirection);
            timeSinceLastFired = 0;
        }
        timeSinceLastFired += Time.deltaTime;
    }
    private void OnFire(InputAction.CallbackContext ctx)
    {
        firing = ctx.control.IsPressed();
        currentFireDirection = ctx.ReadValue<Vector2>();
        RotatePlayer(ctx.ReadValue<Vector2>());
    }
    private void Fire(Vector2 fireDirection)
    {
        if (!gameObject.scene.IsValid()) { return; }
        Vector3 spawnPoint = transform.position + transform.up * projectileSpawnGap;
        spawnPoint.z = transform.position.z;
        var projectileInstance = Instantiate(projectile, spawnPoint, transform.rotation);
        projectileInstance.GetComponent<Projectile>().Launch(playerMovement.CurrentVelocity);
    }

    private void RotatePlayer(Vector2 fireDirection)
    {
        if (fireDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (fireDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (fireDirection.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (fireDirection.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    public void AdjustFireDelay(float delta)
    {
        FireDelay = Mathf.Clamp(FireDelay + delta, minFireDelay, maxFireDelay);
    }

    private void PlayHurt(int heatlh)
    {
        audioSource.PlayOneShot(hurt);
    }
    private void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        controls.MK.Fire.performed -= OnFire;
        health.died -= OnDeath;
    }
}
