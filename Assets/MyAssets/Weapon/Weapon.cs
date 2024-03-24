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

    [Space(5)]
    [Header("Parts")]
    public WeaponAction weaponAction;
    public WeaponShoot weaponShoot;

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
}
