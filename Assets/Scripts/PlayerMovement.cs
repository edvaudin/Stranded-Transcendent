using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerControls controls;

    private Rigidbody2D rb;

    [SerializeField] float moveSpeed = 10f;
    public Vector3 CurrentVelocity { get; private set; }

    InputAction move;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

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
    }

    private void FixedUpdate()
    {
        if (move.ReadValue<Vector2>().magnitude > Mathf.Epsilon)
        {
            Move(move.ReadValue<Vector2>());
        }
        CurrentVelocity = rb.velocity;
    }

    void Move(Vector2 direction)
    {
        rb.AddForce(direction.normalized * moveSpeed);
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
