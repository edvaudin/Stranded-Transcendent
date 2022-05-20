using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerControls PlayerController { get; private set; }
    public bool ControlsInitialized { get; private set; } = false;

    public void InitializeControls()
    {
        if (ControlsInitialized) { return; }
        PlayerController = new PlayerControls();
        ControlsInitialized = true;
        PlayerController.Enable();
    }
}
