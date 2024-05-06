using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponShoot : MonoBehaviour, IModule<WeaponAction>
{
    [HideInInspector]
    public WeaponAction weaponAction;
    public Projectile projectile;

    public string projectileAddressablesPath;

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
        modules.Clear();

        foreach (var module in mods) 
        {
            module.gameObject.SetActive(true);
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

    public abstract ModuleDataType GetData();

    public ModuleDataType GetAllData()
    {
        ModuleDataType data = GetData();

        foreach (var module in modules)
        {
            data.data.modules.Add( module.GetData());
        }

        return data;
    }

    public abstract void SetData(ModuleDataType data);
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

        SetLevel(t.level);
    }

    public abstract void SetRandomBaseStats(float mult);

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

    public abstract ModuleDataType GetData();

    public abstract void SetData(ModuleDataType data);
}