using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpeedLifetimeModule : ProjectileModule
{
    public float baseTime = 0.5f;
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
        time = baseTime * (1 + level * 0.1f);
    }

    public override ModuleData GetData()
    {
        ProjectileSpeedLifetimeModuleData data = new ProjectileSpeedLifetimeModuleData();
        data.className = className;
        data.level = level;
        data.time = baseTime;
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

    public override void SetRandomBaseStats(float mult)
    {
        baseTime = Random.Range(0.5f, 1f) * mult;
        int length = System.Enum.GetNames(typeof(Ease)).Length;
        ease = (Ease)Random.Range(2, length - 3);
    }
}

public class ProjectileSpeedLifetimeModuleData : ModuleData
{
    public float time;
    public Ease ease;
}

