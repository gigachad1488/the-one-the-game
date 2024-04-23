using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponShoot : MonoBehaviour, IModule<WeaponAction>
{
    [HideInInspector]
    public WeaponAction weaponAction;
    public Projectile projectile;

    public string className;

    public int projectileLevel = 1;

    public delegate void WeaponShootDelegate(Vector2 directionOffset);
    public event WeaponShootDelegate? OnWeaponShoot;

    public List<ShootModule> modules = new List<ShootModule>();

    public int level { get; set; }

    private void Start()
    {
        className = this.GetType().Name;
    }

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
        this.level = level;

        foreach (ShootModule module in modules)
        {
            module.SetLevel(level);
        }

        AfterLevelSet();
    }

    public abstract void AfterLevelSet();

    public abstract void UnpackButton();

    public abstract void Shoot(Vector2 directionOffset);

    public void InvokeAction(Vector2 directionOffset)
    {
        OnWeaponShoot?.Invoke(directionOffset);
    }

    public abstract ModuleData GetData();

    public ModuleData GetAllData()
    {
        ModuleData data = GetData();

        foreach (var module in modules)
        {
            data.modules.Add(module.GetData());
        }

        return data;
    }

    public abstract void SetData(ModuleData data);
}

public abstract class ShootModule : MonoBehaviour, IModule<WeaponShoot>
{
    public string className;

    protected WeaponShoot shoot;

    public int level { get; set; }

    private void Start()
    {
        className = this.GetType().Name;
    }

    public void Set(WeaponShoot t)
    {
        shoot = t;
        shoot.OnWeaponShoot += OnShoot;

        AfterSet();
    }

    public void SetLevel(int level)
    {
        this.level = level;

        AfterLevelSet();
    }

    public abstract void AfterLevelSet();

    public abstract void AfterSet();

    public void Dispose()
    {
        shoot.OnWeaponShoot -= OnShoot;
    }

    public abstract void OnShoot(Vector2 directionOffset);

    public abstract ModuleData GetData();

    public abstract void SetData(ModuleData data);
}