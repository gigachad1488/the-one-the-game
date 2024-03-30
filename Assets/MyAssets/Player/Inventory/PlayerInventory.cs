using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private PlayerInventoryCell[] cells;

    [SerializeField]
    private Weapon[] weapons;

    private int currentCellId = 2;
    private InputManager inputManager;

    private float changeCd = 0.5f;
    private bool canChange = true;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.slotsAction.started += SlotsAction_started;

        for (int i = 0; i < weapons.Length; i++) 
        {
            cells[i].SetWeapon(weapons[i]);
            weapons[i].Unequip();
            weapons[i].enabled = false;
        }

        ChangeSlot(1);
    }

    private void SlotsAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (canChange)
        {
            int slot = Convert.ToInt32(obj.ReadValue<float>());
            ChangeSlot(slot);
            canChange = false;
            Invoke(nameof(ChangeCd), changeCd);
        }
    }

    private void ChangeCd()
    {
        canChange = true;
    }

    public void ChangeSlot(int id)
    {
        if (currentCellId != id)
        {           
            cells[currentCellId - 1].UnselectCell();           
            cells[id - 1].SelectCell();

            weapons[currentCellId - 1].Unequip();
            weapons[currentCellId - 1].enabled = false;

            weapons[id - 1].enabled = true;
            weapons[id - 1].Equip();

            currentCellId = id;
        }
    }
}
