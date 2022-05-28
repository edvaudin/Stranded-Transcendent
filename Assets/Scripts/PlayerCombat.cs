using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject psmPrefab;
    [SerializeField] GameObject projectile;
    [SerializeField] float currentFireDelay;
    [SerializeField] float currentProjectileSpeed = 20f;
    [SerializeField] float currentProjectileRange = 1.5f;
    private float timeSinceLastFired = Mathf.Infinity;
    [SerializeField] float projectileSpawnGap = 2f;
    [SerializeField] AudioClip hurt;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite upSprite;
    [SerializeField] Sprite downSprite;
    [SerializeField] Sprite leftSprite;
    private PlayerInput playerInput;
    private PlayerControls controls;
    private PlayerMovement playerMovement;
    private Health health;
    private AudioSource audioSource;
    private bool firing = false;
    private PlayerStatManager psm;
    private Quaternion currentFireDirection;
    private Vector3 currentFireSpawnPoint;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        psm = FindObjectOfType<PlayerStatManager>();
        if (psm == null) { psm = Instantiate(psmPrefab).GetComponent<PlayerStatManager>(); }
        psm.fireDelay.valueChanged += UpdateFireDelay;
        psm.projectileSpeed.valueChanged += UpdateProjectileSpeed;
        psm.projectileRange.valueChanged += UpdateProjectileRange;
    }

    private void Start()
    {
        currentFireDelay = psm.fireDelay.Value;
        currentProjectileSpeed = psm.projectileSpeed.Value;
        currentProjectileRange = psm.projectileRange.Value;
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
        Vector3 spawnPoint = transform.position + currentFireSpawnPoint * projectileSpawnGap;
        spawnPoint.z = transform.position.z;
        var projectileInstance = Instantiate(projectile, spawnPoint, transform.rotation * currentFireDirection);
        projectileInstance.GetComponent<Projectile>().SetSpeed(currentProjectileSpeed);
        projectileInstance.GetComponent<Projectile>().Launch(playerMovement.CurrentVelocity, currentProjectileRange);
    }

    private void RotatePlayer(Vector2 fireDirection)
    {
        if (fireDirection.x > 0)
        {
            currentFireDirection =  Quaternion.Euler(0, 0, -90);
            currentFireSpawnPoint = transform.right;
            sr.sprite = leftSprite;
            sr.flipX = true;
        }
        else if (fireDirection.x < 0)
        {
            currentFireDirection = Quaternion.Euler(0, 0, 90);
            currentFireSpawnPoint = -transform.right;
            sr.sprite = leftSprite;
            sr.flipX = false;
        }
        else if (fireDirection.y > 0)
        {
            currentFireDirection = Quaternion.Euler(0, 0, 0);
            currentFireSpawnPoint = transform.up;
            sr.sprite = upSprite;
            sr.flipX = false;
        }
        else if (fireDirection.y < 0)
        {
            currentFireDirection = Quaternion.Euler(0, 0, 180);
            currentFireSpawnPoint = -transform.up;
            sr.sprite = downSprite;
            sr.flipX = false;
        }
    }

    public void UpdateFireDelay(float newValue) { currentFireDelay = newValue; }
    private void UpdateProjectileSpeed(float newValue) { currentProjectileSpeed = newValue; }
    private void UpdateProjectileRange(float newValue) { currentProjectileRange = newValue; }
    public void AdjustFireDelay(float delta)
    {
        psm.fireDelay.AdjustValue(delta);
    }

    public void AdjustProjectileRange(float delta)
    {
        psm.projectileRange.AdjustValue(delta);
    }

    private void PlayHurt(int heatlh)
    {
        audioSource.PlayOneShot(hurt);
    }
    private void OnDeath()
    {
        if (SceneManager.GetActiveScene().name.Contains("Underworld"))
        {
            SceneManager.LoadScene("Overworld 1");
        }
        else
        {
            SceneManager.LoadScene("Underworld 1");
        }
        
    }

    private void OnDisable()
    {
        controls.MK.Fire.performed -= OnFire;
        health.died -= OnDeath;
        psm.fireDelay.valueChanged -= UpdateFireDelay;
        psm.projectileSpeed.valueChanged -= UpdateProjectileSpeed;
        psm.projectileRange.valueChanged -= UpdateProjectileRange;
    }
}
