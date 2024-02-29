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
    private InputAction jumpAction;
    private InputAction dashAction;

    #endregion

    [Space(5)]
    [Header("ActionValues")]
    public Vector2 move;
    public bool jump;
    public bool dash;

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

        moveAction.performed += MoveAction_performed;
        jumpAction.performed += JumpAction_performed;
        dashAction.performed += DashAction_performed;

        moveAction.canceled += MoveAction_performed;
        jumpAction.canceled += JumpAction_performed;
        dashAction.canceled += DashAction_performed;
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
