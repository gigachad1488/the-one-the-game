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

    public int baseDamage = 1;
    public float baseAttackSpeed = 0.5f;
    public float baseScale = 1;

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
    private float damageMult = 1;
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

    private float attackSpeedMult = 1;
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

    private float scaleMult = 1;
    private float scaleFlat = 0;

    public void CalcDamage()
    {

    }

    public void CalcAttackSpeed()
    {

    }
    public void CalcScale()
    {

    }

    public void Init(Player player)
    {     
        this.player = player;
        weaponAction.Set(this);
        weaponShoot.Set(weaponAction);

        inputManager = player.playerMovement.inputManager;

        currentAttackSpeed = baseAttackSpeed;
        currentDamage = baseDamage;
        currentScale = baseScale;
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    private void Update()
    {
        if (inputManager != null && inputManager.leftMouse)
        {
            Debug.Log("LEFT ACTION");
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
