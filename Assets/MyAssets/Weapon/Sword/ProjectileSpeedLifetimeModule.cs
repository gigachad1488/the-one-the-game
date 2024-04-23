using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpeedLifetimeModule : ProjectileModule
{
    public float time;

    public Ease ease = Ease.InSine;

    private Rigidbody2D projectileRb;

    private Vector3 startSpeed;
    public override void AfterSet()
    {
        projectileRb = projectile.GetComponent<Rigidbody2D>();
        startSpeed = projectileRb.velocity;

        Tween.Custom(1f, 0f, time, x => projectileRb.velocity = startSpeed * x, ease);
    }

    public override void ProjectileHit()
    {
    }

    public override void AfterLevelSet()
    {
    }

    public override ModuleData GetData()
    {
        ProjectileSpeedLifetimeModuleData data = new ProjectileSpeedLifetimeModuleData();
        data.className = className;
        data.level = level;
        data.time = time;
        data.ease = ease;

        return data;
    }

    public override void SetData(ModuleData data)
    {
        ProjectileSpeedLifetimeModuleData pdata = data as ProjectileSpeedLifetimeModuleData;
        level = pdata.level;
        time = pdata.time;
        ease = pdata.ease;
    }
}

public class ProjectileSpeedLifetimeModuleData : ModuleData
{
    public float time;
    public Ease ease;
}

