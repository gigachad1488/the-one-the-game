using System;
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

    public int baseDamage { get; set; }
    public float baseAttackSpeed { get; set; }
    public float baseScale { get; set; }

    public int currentDamage;
    public float currentAttackSpeed;
    public float currentScale;

    public float maxLevelMult = 10;

    private int level = 1;

    [Space(5)]
    [Header("Parts")]
    public WeaponAction weaponAction;
    public WeaponShoot weaponShoot;

    [Header("Attributes")]
    private float damageMult = 0;
    public float DamageMult
    {
        get
        {
            return damageMult;
        }
        set
        {
            damageMult = value;
            CalcDamage();
        }
    }

    private float damageFlat = 0;
    public float DamageFlat
    {
        get
        {
            return damageFlat;
        }
        set
        {
            damageFlat = value;
            CalcDamage();
        }
    }

    private float attackSpeedMult = 0;
    public float AttackSpeedMult
    {
        get
        {
            return attackSpeedMult;
        }
        set
        {
            attackSpeedMult = value;
            CalcAttackSpeed();
        }
    }

    private float attackSpeedFlat = 0;
    public float AttackSpeedFlat
    {
        get
        {
            return attackSpeedFlat;
        }
        set
        {
            attackSpeedFlat = value;
            CalcAttackSpeed();
        }
    }

    private float scaleMult = 3;
    public float ScaleMult
    {
        get
        {
            return scaleMult;
        }
        set
        {
            scaleMult = value;
            CalcScale();
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }

    private float scaleFlat = 0;
    public float ScaleFlat
    {
        get 
        {
            return scaleFlat;
        }
        set
        {
            scaleFlat = value;
            CalcScale();
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }

    public void CalcDamage()
    {
        currentDamage = Convert.ToInt32((baseDamage * (1 + damageMult)) + damageFlat);
    }

    public void CalcAttackSpeed()
    {
        currentAttackSpeed = (baseAttackSpeed * (1 - attackSpeedMult)) - attackSpeedFlat;
    }
    public void CalcScale()
    {
        currentScale = (baseScale * (1 + scaleMult)) + scaleFlat;
    }

    public void Init(Player player)
    {     
        this.player = player;

        weaponAction.Set(this);
        weaponShoot.Set(weaponAction);

        inputManager = player.playerMovement.inputManager;

        CalcDamage();
        CalcAttackSpeed();
        CalcScale();

        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    private void Update()
    {
        if (inputManager != null && inputManager.leftMouse)
        {
            LeftAction();
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
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
