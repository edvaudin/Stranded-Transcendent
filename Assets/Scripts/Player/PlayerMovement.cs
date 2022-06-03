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
    private RandomAudioPlayer rap;

    private Rigidbody2D rb;

    [SerializeField] float currentMoveSpeed;
    [SerializeField] float footstepRate = 0.4f;
    private float timeSinceLastFootstep = Mathf.Infinity;
    public Vector3 CurrentVelocity { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        psm = FindObjectOfType<PlayerStatManager>();
        rap = GetComponent<RandomAudioPlayer>();
        psm.moveSpeed.valueChanged += UpdateMoveSpeed;
    }

    private void Start()
    {
        currentMoveSpeed = psm.moveSpeed.Value;
        Debug.Log($"{currentMoveSpeed} set to {psm.moveSpeed.Value}");
    }

    private void UpdateMoveSpeed(float newValue) { currentMoveSpeed = newValue; }

    private void OnEnable()
    {

        playerInput.actions["Restart"].performed += ReloadScene;
        playerInput.actions["Exit"].performed += Quit;
    }

    private void OnDisable()
    {
        playerInput.actions["Restart"].performed -= ReloadScene;
        playerInput.actions["Exit"].performed -= Quit;
        psm.moveSpeed.valueChanged -= UpdateMoveSpeed;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State != GameState.Playing) { return; }
        var movement = playerInput.actions["Move"].ReadValue<Vector2>();
        if (movement.magnitude > Mathf.Epsilon)
        {
            timeSinceLastFootstep += Time.deltaTime;
            if (timeSinceLastFootstep > footstepRate)
            {
                timeSinceLastFootstep = 0;
                rap.Play("footsteps");
            }
            Move(movement);
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
