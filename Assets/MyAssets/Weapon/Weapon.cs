using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Guid guid;

    public Player player;
    private InputManager inputManager;

    public GameObject weaponModelPrefab;
    public string weaponModelAddressablesPath;
    public string projectileAddressablesPath;

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

    public int level = 1;

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

    private float scaleMult = 0;
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

        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    public void Init(Player player)
    {
        inputManager = player.playerMovement.inputManager;
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        this.player = player;

        weaponAction.Set(this);
        weaponAction.level = level;

        weaponShoot.Set(weaponAction);
        weaponShoot.SetLevel(level);

        weaponShoot.projectile.SetLevel(level);

        CalcDamage();
        CalcAttackSpeed();
        CalcScale();
    }

    private void Update()
    {
        if (inputManager != null && inputManager.leftMouse)
        {
            LeftAction();
        }
    }

    public void SetLevel()
    {
        weaponAction.level = level;
        weaponShoot.SetLevel(level);
        weaponShoot.projectileLevel = level;
        weaponShoot.projectile.SetLevel(level);
    }

    public void LeftAction()
    {
        weaponAction.Action();
    }

    public void Equip()
    {
        weaponAction.enabled = true;
        weaponShoot.enabled = true;
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

    public WeaponBaseData GetData()
    {
        WeaponBaseData data = GetWeaponStats();

        data.actionData = weaponAction.GetAllData();
        data.shootData = weaponShoot.GetAllData();

        ModuleDataType type = new ModuleDataType();
        type = weaponShoot.projectile.GetAllData();

        data.projectileData = type;

        return data;
    }

    public WeaponBaseData GetWeaponStats()
    {
        WeaponBaseData data = new WeaponBaseData();

        data.weaponModelAddressablesPath = weaponModelAddressablesPath;
        data.projectileAddressablesPath = projectileAddressablesPath;

        data.scaleFlat = scaleFlat;
        data.scaleMult = scaleMult;
        data.attackSpeedMult = attackSpeedMult;
        data.damageMult = damageMult;

        data.baseDamage = baseDamage;
        data.baseAttackSpeed = baseAttackSpeed;
        data.baseScale = baseScale;

        data.level = level;

        return data;
    }

    public void SetData(WeaponBaseData data)
    {
        if (data.weaponModelAddressablesPath != null)
        {
            weaponModelAddressablesPath = data.weaponModelAddressablesPath;
        }

        projectileAddressablesPath = data.projectileAddressablesPath;

        scaleFlat = data.scaleFlat;
        scaleMult = data.scaleMult;

        damageFlat = data.damageFlat;
        damageMult = data.damageMult;

        attackSpeedFlat = data.attackSpeedFlat;
        attackSpeedMult = data.attackSpeedMult;

        baseAttackSpeed = data.baseAttackSpeed;
        baseScale = data.baseScale;
        baseDamage = data.baseDamage;

        level = data.level;
    }
}
