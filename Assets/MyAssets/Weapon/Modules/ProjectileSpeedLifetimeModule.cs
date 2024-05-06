using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpeedLifetimeModule : ProjectileModule
{
    public float baseTime = 0.5f;
    public float time;

    public float startValue;
    public float endValue;

    public Ease ease = Ease.InSine;

    private Rigidbody2D projectileRb;

    private Vector3 startSpeed;
    public override void AfterSet()
    {
        if (startValue == 0)
        {
            endValue = 1;
        }
        else
        {
            endValue = 0;
        }

        StartCoroutine(WaitFrame());
    }

    private IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();

        if (projectile.TryGetComponent<ProjectileDisapearModule>(out var disapearModule))
        {
            time = disapearModule.time * 0.9f;
        }

        projectileRb = projectile.GetComponent<Rigidbody2D>();
        startSpeed = projectileRb.velocity;

        Tween.Custom(startValue, endValue, time, x => projectileRb.velocity = startSpeed * x, ease);
    }

    public override void ProjectileHit(Vector3 pos)
    {
    }

    public override void AfterLevelSet()
    {       
        time = baseTime * (1 + level * 0.1f);
    }

    public override ModuleDataType GetData()
    {
        ProjectileSpeedLifetimeModuleData data = new ProjectileSpeedLifetimeModuleData();
        data.className = className;
        data.level = level;
        data.time = baseTime;
        data.ease = ease;
        data.startValue = startValue;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void SetData(ModuleDataType data)
    {
        ProjectileSpeedLifetimeModuleData pdata = (ProjectileSpeedLifetimeModuleData)data.data;
        level = pdata.level;
        time = pdata.time;
        ease = pdata.ease;
        startValue = pdata.startValue;
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseTime = Random.Range(0.5f, 1f) * mult;
        int length = System.Enum.GetNames(typeof(Ease)).Length;
        ease = (Ease)Random.Range(2, length - 3);

        startValue = Random.Range(0, 2);
    }
}

public class ProjectileSpeedLifetimeModuleData : ModuleData
{
    public float startValue;
    public float time;
    public Ease ease;
}

