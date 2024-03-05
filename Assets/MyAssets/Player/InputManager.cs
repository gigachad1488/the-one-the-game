using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    #region DefaultMap
   
    private InputActionMap defaultMap;

    private InputAction moveAction;
    public InputAction jumpAction;
    private InputAction dashAction;
    private InputAction downAction;

    #endregion

    [Space(5)]
    [Header("ActionValues")]
    public Vector2 move;
    public bool jump;
    public bool dash;
    public bool down;

    private void OnEnable()
    {
        defaultMap.Enable();
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        defaultMap = playerInput.currentActionMap;

        moveAction = defaultMap.FindAction("Move");
        jumpAction = defaultMap.FindAction("Jump");
        dashAction = defaultMap.FindAction("Dash");
        downAction = defaultMap.FindAction("Down");

        moveAction.performed += MoveAction_performed;
        jumpAction.performed += JumpAction_performed;
        dashAction.performed += DashAction_performed;
        downAction.performed += DownAction_performed;

        moveAction.canceled += MoveAction_performed;
        jumpAction.canceled += JumpAction_performed;
        dashAction.canceled += DashAction_performed;
        downAction.canceled += DownAction_performed;
    }

    private void DownAction_performed(InputAction.CallbackContext obj)
    {
        down = obj.ReadValueAsButton();
    }

    private void DashAction_performed(InputAction.CallbackContext obj)
    {
        dash = obj.ReadValueAsButton();
    }

    private void MoveAction_performed(InputAction.CallbackContext obj)
    {
        move = obj.ReadValue<Vector2>();
    }
    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        jump = obj.ReadValueAsButton();
    }

    private void OnDisable()
    {
        defaultMap.Disable();
    }
}
