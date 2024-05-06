using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IModule<WeaponShoot>
{
    public WeaponShoot weaponShoot;

    public string className;

    public List<ProjectileModule> projectileModules = new List<ProjectileModule>();

    public float baseMult = 1f;
    public float mult = 1f;

    public int level { get; set; }

    public delegate void ProjectileDelegate(Vector3 position);
    public event ProjectileDelegate OnProjectileHit;

    private void Start()
    {
        className = this.GetType().Name;
    }

    public void Set(WeaponShoot t)
    {
        weaponShoot = t;

        ProjectileModule[] mods = GetComponentsInChildren<ProjectileModule>();
        projectileModules.Clear();

        foreach (ProjectileModule mod in mods) 
        {
            mod.gameObject.SetActive(true);
            projectileModules.Add(mod);
            mod.Set(this);
        }

        AfterSet();     

        SetLevel(t.level);
    }

    public abstract void AfterSet();

    public void SetLevel(int level)
    {
        this.level = level;

        foreach (ProjectileModule module in projectileModules)
        {
            module.SetLevel(level);
        }

        AfterLevelSet();
    }

    public abstract void AfterLevelSet();

    public abstract void SetRandomBaseStats(float mult);

    public abstract void Hit();

    public void InvokeAction(Vector3 position)
    {
        OnProjectileHit.Invoke(position);
    }

    public abstract ModuleDataType GetData();

    public ModuleDataType GetAllData()
    {
        ModuleDataType data = GetData();
        data.data.className = this.GetType().Name;

        foreach (var module in projectileModules)
        {
            ModuleDataType type = module.GetData();
            type.data.className = module.GetType().Name;
            type.data.addressablesPath = module.addressablesPath;
            data.data.modules.Add(type);
        }

        return data;
    }

    public abstract void SetData(ModuleDataType data);
}

public abstract class ProjectileModule : MonoBehaviour, IModule<Projectile>
{
    [HideInInspector]
    public Projectile projectile;

    public string className;
    public string addressablesPath;

    public int level { get; set; }

    public void Set(Projectile t)
    {
        projectile = t;
        projectile.OnProjectileHit += ProjectileHit;

        AfterSet();
    }

    public void SetLevel(int level)
    {
        this.level = level;

        AfterLevelSet();
    }

    public abstract void AfterLevelSet();

    public abstract void AfterSet();

    public abstract void SetRandomBaseStats(float mult);

    public void Dispose()
    {
        projectile.OnProjectileHit -= ProjectileHit;
    }

    public abstract void ProjectileHit(Vector3 position);

    public abstract ModuleDataType GetData();

    public abstract void SetData(ModuleDataType data);
}
