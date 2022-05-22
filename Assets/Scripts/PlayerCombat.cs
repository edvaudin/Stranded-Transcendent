using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] float currentFireDelay;
    [SerializeField] float currentProjectileSpeed = 20f;
    private float timeSinceLastFired = Mathf.Infinity;
    [SerializeField] float projectileSpawnGap = 2f;
    [SerializeField] AudioClip hurt;
    private PlayerInput playerInput;
    private PlayerControls controls;
    private PlayerMovement playerMovement;
    private Health health;
    private AudioSource audioSource;
    private bool firing = false;
    private PlayerStatManager psm;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        psm = FindObjectOfType<PlayerStatManager>();
        psm.fireDelay.valueChanged += UpdateFireDelay;
        psm.projectileSpeed.valueChanged += UpdateProjectileSpeed;
    }

    private void Start()
    {
        currentFireDelay = psm.fireDelay.Value;
        currentProjectileSpeed = psm.projectileSpeed.Value;
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
        if (firing && timeSinceLastFired > currentFireDelay)
        {
            Fire();
            timeSinceLastFired = 0;
        }
        timeSinceLastFired += Time.deltaTime;
    }

    // Is currently sticky as holding one key blocks event invocation for other keys in composite.
    private void OnFire(InputAction.CallbackContext ctx)
    {
        firing = ctx.control.IsPressed();
        RotatePlayer(ctx.ReadValue<Vector2>());
    }
    private void Fire()
    {
        if (!gameObject.scene.IsValid()) { return; }
        Vector3 spawnPoint = transform.position + transform.up * projectileSpawnGap;
        spawnPoint.z = transform.position.z;
        var projectileInstance = Instantiate(projectile, spawnPoint, transform.rotation);
        projectileInstance.GetComponent<Projectile>().SetSpeed(currentProjectileSpeed);
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

    public void UpdateFireDelay(float newValue) { currentFireDelay = newValue; }
    private void UpdateProjectileSpeed(float newValue) { currentProjectileSpeed = newValue; }
    public void AdjustFireDelay(float delta)
    {
        psm.fireDelay.AdjustValue(delta);
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
        psm.fireDelay.valueChanged -= UpdateFireDelay;
        psm.projectileSpeed.valueChanged -= UpdateProjectileSpeed;
    }
}
