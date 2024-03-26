using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private PlayerInventoryCell[] cells;

    private int currentCellId = 1;
    private InputManager inputManager;
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.slotsAction.started += SlotsAction_started;
        cells[0].SelectCell();
    }

    private void SlotsAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        int slot = Convert.ToInt32(obj.ReadValue<float>());
        ChangeSlot(slot);
    }

    public void ChangeSlot(int id)
    {
        if (currentCellId != id)
        {           
            cells[currentCellId - 1].UnselectCell();
            currentCellId = id;
            cells[id - 1].SelectCell();
        }
    }
}
