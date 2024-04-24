using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDisapearModule : ProjectileModule
{
    public float baseTime = 0.5f;
    public float time = 1f;
    public override void AfterSet()
    {
        Invoke(nameof(Destroy), time);
    }

    private void Destroy()
    {
        Destroy(projectile.gameObject);
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
        ProjectileDisapearModuleData data = new ProjectileDisapearModuleData();
        data.className = className;
        data.level = level;
        data.time = baseTime;

        return data;
    }

    public override void SetData(ModuleData data)
    {
        ProjectileDisapearModuleData pdata = data as ProjectileDisapearModuleData;
        level = data.level;
        baseTime = pdata.time;
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseTime = Random.Range(0.5f, 1f) * mult;
    }
}

public class ProjectileDisapearModuleData : ModuleData
{
    public float time;
}
