using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Player player;
    private InputManager inputManager;

    public GameObject weaponModelPrefab;

    public delegate void LeftActionDelegate();
    public event LeftActionDelegate? OnLeftAction;

    public delegate void UnequipDelegate();
    public event UnequipDelegate? OnUnequip;

    public float baseDamage = 1;
    public float baseAttackSpeed = 0.5f;
    public float baseScale = 1;

    [Space(5)]
    [Header("Parts")]
    public WeaponAction weaponAction;
    public WeaponShoot weaponShoot;

    [Space(5)]
    [Header("Attributes")]
    public float damageMult = 1;
    public float damageFlat = 0;
    private float currentDamage;
    public float attackSpeedMult = 1;
    public float attackSpeedFlat = 0;
    private float currentAttackSpeed = 1;
    public float scaleMult = 1;
    public float scaleFlat = 0;
    private float currentScale = 1;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        inputManager = player.playerMovement.inputManager;

        weaponAction.Set(this);
        weaponShoot.Set(weaponAction);
    }

    private void Update()
    {
        if (inputManager.leftMouse)
        {
            LeftAction();
        }
    }

    public void LeftAction()
    {
        weaponAction.Action();
    }

    public void Equip()
    {
        weaponAction.enabled = false;
        weaponShoot.enabled = false;
    }

    public void Unequip()
    {
        OnUnequip?.Invoke();
        if (weaponAction.weaponModel != null)
        {          
            weaponAction.weaponModel.SetActive(false);
            weaponAction.enabled = false;
            weaponShoot.enabled = false;
        }
    }
}
