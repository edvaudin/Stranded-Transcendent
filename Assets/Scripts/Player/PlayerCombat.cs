using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite upSprite;
    [SerializeField] Sprite downSprite;
    [SerializeField] Sprite leftSprite;

    [Header("Combat")]
    [SerializeField] GameObject psmPrefab;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpawnGap = 2f;
    private float currentFireDelay;
    private float currentProjectileSpeed = 20f;
    private float currentProjectileRange = 1.5f;
    private int currentBaseHealth;
    private float timeSinceLastFired = Mathf.Infinity;

    [Header("Audio")]
    [SerializeField] AudioClip hurt;

    private PlayerInput playerInput;
    private PlayerControls controls;
    private PlayerMovement playerMovement;
    private Health health;
    private AudioSource audioSource;
    private PlayerStatManager psm;
    private Quaternion currentFireDirection;
    private Vector3 currentFireSpawnPoint;
    public static Action playerDied;

    private void Awake()
    {
        GetComponents();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        health.died += OnDeath;
        health.changed += PlayHurt;
        psm.health.valueChanged += UpdateHealth;
        psm.fireDelay.valueChanged += UpdateFireDelay;
        psm.projectileSpeed.valueChanged += UpdateProjectileSpeed;
        psm.projectileRange.valueChanged += UpdateProjectileRange;
    }

    private void GetComponents()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        psm = FindObjectOfType<PlayerStatManager>();
        if (psm == null) { psm = Instantiate(psmPrefab).GetComponent<PlayerStatManager>(); }
    }

    private void Start()
    {
        currentFireDelay = psm.fireDelay.Value;
        currentProjectileSpeed = psm.projectileSpeed.Value;
        currentProjectileRange = psm.projectileRange.Value;
        currentBaseHealth = (int)psm.health.Value;
        health.SetBaseHealth(currentBaseHealth);
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) { return; }
        var fire = playerInput.actions["Fire"].ReadValue<Vector2>();
        if (fire.magnitude > Mathf.Epsilon)
        {
            RotatePlayer(fire);
            if (timeSinceLastFired > currentFireDelay)
            {
                Fire();
                timeSinceLastFired = 0;
            }
        }
        timeSinceLastFired += Time.deltaTime;
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

    public void UpdateHealth(float newValue) 
    { 
        currentBaseHealth = (int)newValue;
        health.SetBaseHealth(currentBaseHealth);
        health.GainHealth(currentBaseHealth - health.CurrentHealth);
    }
    public void UpdateFireDelay(float newValue) => currentFireDelay = newValue;
    private void UpdateProjectileSpeed(float newValue) => currentProjectileSpeed = newValue; 
    private void UpdateProjectileRange(float newValue) => currentProjectileRange = newValue; 
    public void AdjustBaseHealth(int delta) => psm.health.AdjustValue(delta);
    public void AdjustFireDelay(float delta) => psm.fireDelay.AdjustValue(delta);
    public void AdjustProjectileRange(float delta) => psm.projectileRange.AdjustValue(delta);

    private void PlayHurt(int newHeatlh)
    {
        if (health.BaseHealth == newHeatlh) { return; }
        audioSource.PlayOneShot(hurt);
    }
    private void OnDeath()
    {
        playerDied?.Invoke();
        GameManager.Instance.UpdateGameState(GameState.GameOver);
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        health.died -= OnDeath;
        health.changed -= PlayHurt;
        psm.health.valueChanged -= UpdateHealth;
        psm.fireDelay.valueChanged -= UpdateFireDelay;
        psm.projectileSpeed.valueChanged -= UpdateProjectileSpeed;
        psm.projectileRange.valueChanged -= UpdateProjectileRange;
    }
}
