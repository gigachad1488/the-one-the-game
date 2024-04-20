using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IModule<WeaponShoot>
{
    public WeaponShoot weaponShoot;

    public List<ProjectileModule> projectileModules = new List<ProjectileModule>();

    public float mult = 1f;

    public delegate void ProjectileDelegate();
    public event ProjectileDelegate OnProjectileHit;
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
        foreach (ProjectileModule module in projectileModules)
        {
            module.SetLevel(level);
        }

        AfterLevelSet(level);
    }

    public abstract void AfterLevelSet(int level);  

    public abstract void Hit();

    public void InvokeAction()
    {
        OnProjectileHit.Invoke();
    }
}

public abstract class ProjectileModule : MonoBehaviour, IModule<Projectile>
{
    [HideInInspector]
    public Projectile projectile;
    public void Set(Projectile t)
    {
        projectile = t;
        projectile.OnProjectileHit += ProjectileHit;

        AfterSet();
    }

    public abstract void SetLevel(int level);

    public abstract void AfterSet();

    public void Dispose()
    {
        projectile.OnProjectileHit -= ProjectileHit;
    }

    public abstract void ProjectileHit();
}
