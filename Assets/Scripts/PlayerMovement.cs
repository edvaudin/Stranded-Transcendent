using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using VaudinGames.Audio;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStatManager psm;
    private PlayerInput playerInput;
    private PlayerControls controls;
    private RandomAudioPlayer rap;

    private Rigidbody2D rb;

    [SerializeField] float currentMoveSpeed;
    [SerializeField] float footstepRate = 0.4f;
    private float timeSinceLastFootstep = Mathf.Infinity;
    public Vector3 CurrentVelocity { get; private set; }
    private bool playerDead = false;

    InputAction move;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = FindObjectOfType<PlayerInput>();
        psm = FindObjectOfType<PlayerStatManager>();
        rap = GetComponent<RandomAudioPlayer>();
        psm.moveSpeed.valueChanged += UpdateMoveSpeed;
        PlayerCombat.playerDied += OnPlayerDeath;
        BossSpawner.allBossesKilled += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        playerDead = true;
    }
    private void Start()
    {
        currentMoveSpeed = psm.moveSpeed.Value;
        Debug.Log($"{currentMoveSpeed} set to {psm.moveSpeed.Value}");
    }

    private void UpdateMoveSpeed(float newValue) { currentMoveSpeed = newValue; }

    private void OnEnable()
    {
        if (!playerInput.ControlsInitialized)
        {
            playerInput.InitializeControls();
        }
        controls = playerInput.PlayerController;
        //controls.MK.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        move = controls.MK.Move;
        move.Enable();

        controls.MK.Restart.performed += ReloadScene;
        controls.MK.Exit.performed += Quit;
    }

    private void OnDisable()
    {
        move.Disable();
        controls.MK.Restart.performed -= ReloadScene;
        controls.MK.Exit.performed -= Quit;
        psm.moveSpeed.valueChanged -= UpdateMoveSpeed;
        BossSpawner.allBossesKilled -= OnPlayerDeath;
    }

    private void FixedUpdate()
    {
        if (playerDead) { return; }
        if (move.ReadValue<Vector2>().magnitude > Mathf.Epsilon)
        {
            timeSinceLastFootstep += Time.deltaTime;
            if (timeSinceLastFootstep > footstepRate)
            {
                timeSinceLastFootstep = 0;
                rap.Play("footsteps");
            }
            Move(move.ReadValue<Vector2>());
        }
        CurrentVelocity = rb.velocity;
    }

    public void AdjustMoveSpeed(float delta)
    {
        psm.moveSpeed.AdjustValue(delta);
    }

    void Move(Vector2 direction)
    {
        rb.AddForce(direction.normalized * currentMoveSpeed);
    }

    void ReloadScene(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Quit(InputAction.CallbackContext ctx)
    {
        Application.Quit();
    }
}
