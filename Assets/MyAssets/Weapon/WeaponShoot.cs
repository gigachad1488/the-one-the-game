using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponShoot : MonoBehaviour, IModule<WeaponAction>
{
    [HideInInspector]
    public WeaponAction weaponAction;
    public Projectile projectile;

    public delegate void WeaponShootDelegate(Vector2 directionOffset);
    public event WeaponShootDelegate? OnWeaponShoot;

    public List<ShootModule> modules = new List<ShootModule>();
    public void Set(WeaponAction t)
    {
        weaponAction = t;

        ShootModule[] mods = GetComponentsInChildren<ShootModule>();

        foreach (var module in mods) 
        {
            modules.Add(module);
            module.Set(this);
        }

        weaponAction.OnWeaponAction += Shoot;

        AfterSet();
    }

    public abstract void AfterSet();

    public void SetLevel(int level)
    {
        foreach (ShootModule module in modules)
        {
            module.SetLevel(level);
        }

        AfterLevelSet(level);
    }

    public abstract void AfterLevelSet(int level);

    public abstract void UnpackButton();

    public abstract void Shoot(Vector2 directionOffset);

    public void InvokeAction(Vector2 directionOffset)
    {
        OnWeaponShoot?.Invoke(directionOffset);
    }
}

public abstract class ShootModule : MonoBehaviour, IModule<WeaponShoot>
{
    protected WeaponShoot shoot;
    public void Set(WeaponShoot t)
    {
        shoot = t;
        shoot.OnWeaponShoot += OnShoot;

        AfterSet();
    }

    public abstract void SetLevel(int level);

    public abstract void AfterSet();

    public void Dispose()
    {
        shoot.OnWeaponShoot -= OnShoot;
    }

    public abstract void OnShoot(Vector2 directionOffset);   
}