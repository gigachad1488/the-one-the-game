using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IModule<WeaponShoot>
{
    public WeaponShoot weaponShoot;

    public string projectileAddressablesPath;

    public string className;

    public List<ProjectileModule> projectileModules = new List<ProjectileModule>();

    public float baseMult = 1f;
    public float mult = 1f;

    public int level { get; set; }

    public delegate void ProjectileDelegate();
    public event ProjectileDelegate OnProjectileHit;

    private void Start()
    {
        className = this.GetType().Name;
    }


    public void Set(WeaponShoot t)
    {
        weaponShoot = t;

        ProjectileModule[] mods = GetComponentsInChildren<ProjectileModule>();

        foreach (ProjectileModule mod in mods) 
        {
            projectileModules.Add(mod);
            mod.Set(this);
        }

        AfterSet();
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

    public void InvokeAction()
    {
        OnProjectileHit.Invoke();
    }

    public abstract ModuleData GetData();

    public ProjectileData GetAllData()
    {
        ModuleData data = GetData();

        foreach (var module in projectileModules)
        {
            data.modules.Add(module.GetData());
        }

        ProjectileData pdata = new ProjectileData();
        pdata.projectileAddressablesPath = projectileAddressablesPath;
        pdata.level = data.level;
        pdata.modules = data.modules;
        pdata.className = data.className;

        return pdata;
    }

    public abstract void SetData(ModuleData data);
}

public abstract class ProjectileModule : MonoBehaviour, IModule<Projectile>
{
    [HideInInspector]
    public Projectile projectile;

    public string className;

    public int level { get; set; }

    private void Start()
    {
        className = this.GetType().Name;
    }

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

    public abstract void ProjectileHit();

    public abstract ModuleData GetData();

    public abstract void SetData(ModuleData data);
}
