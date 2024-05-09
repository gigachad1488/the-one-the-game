using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameManager gameManager;

    public List<Weapon> selectedWeapons;

    [SerializeField]
    private PlayerInventoryCell[] cells;

    private Weapon[] weapons;

    private int currentCellId = 2;
    private InputManager inputManager;

    public Transform weaponPosition;

    private float changeCd = 0.5f;
    private bool canChange = true;

    private void Start()
    {
        selectedWeapons = gameManager.multiScenedata.selectedWeapons;

        weapons = new Weapon[selectedWeapons.Count];       
        inputManager = GetComponent<InputManager>();
        inputManager.slotsAction.started += SlotsAction_started;
        Player player = inputManager.gameObject.GetComponent<Player>();

        for (int i = 0; i < cells.Length; i++) 
        {
            if (selectedWeapons.Count > i)
            {
                Weapon weapon = Instantiate(selectedWeapons[i], transform);
                weapon.transform.localPosition = weaponPosition.localPosition;  
                weapon.SetData(selectedWeapons[i].GetWeaponStats());
                weapon.Init(player);
                weapons[i] = weapon;                
                cells[i].SetWeapon(weapons[i]);
                weapons[i].Unequip();
                weapons[i].enabled = false;
            }
            else
            {
                cells[i].gameObject.SetActive(false);
            }
        }

        ChangeSlot(1);
    }

    private void SlotsAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (canChange)
        {
            int slot = Convert.ToInt32(obj.ReadValue<float>());
            if (cells[slot - 1].gameObject.activeInHierarchy)
            {
                ChangeSlot(slot);
                canChange = false;
                Invoke(nameof(ChangeCd), changeCd);
            }
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
            if (cells[id - 1].gameObject.activeInHierarchy)
            {

                if (cells[currentCellId - 1].gameObject.activeInHierarchy)
                {
                    cells[currentCellId - 1].UnselectCell();
                    weapons[currentCellId - 1].Unequip();
                    weapons[currentCellId - 1].enabled = false;
                }

                cells[id - 1].SelectCell();
                weapons[id - 1].enabled = true;
                weapons[id - 1].Equip();

                currentCellId = id;
            }
        }
    }
}
