using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStatManager psm;
    private PlayerInput playerInput;
    private PlayerControls controls;

    private Rigidbody2D rb;

    [SerializeField] float currentMoveSpeed;
    public Vector3 CurrentVelocity { get; private set; }

    InputAction move;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = FindObjectOfType<PlayerInput>();
        psm = FindObjectOfType<PlayerStatManager>();
        psm.moveSpeed.valueChanged += UpdateMoveSpeed;
    }

    private void Start()
    {
        currentMoveSpeed = psm.moveSpeed.Value;
    }

    private void UpdateMoveSpeed(float newValue) { currentMoveSpeed = newValue; }

    private void OnEnable()
    {
        if (!playerInput.ControlsInitialized)
        {
            playerInput.InitializeControls();
        }
        controls = playerInput.PlayerController;
        controls.MK.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
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
    }

    private void FixedUpdate()
    {
        if (move.ReadValue<Vector2>().magnitude > Mathf.Epsilon)
        {
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
