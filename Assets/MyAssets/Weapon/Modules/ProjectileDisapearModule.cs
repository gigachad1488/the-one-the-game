using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDisapearModule : ProjectileModule
{
    private float baseTime = 0.8f;
    public float time = 1f;
    public override void AfterSet()
    {
        Invoke(nameof(Destroy), time);
    }

    private void Destroy()
    {
        Destroy(projectile.gameObject);
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
        ProjectileDisapearModuleData data = new ProjectileDisapearModuleData();
        data.className = className;
        data.level = level;
        data.time = baseTime;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void SetData(ModuleDataType data)
    {
        ProjectileDisapearModuleData pdata = (ProjectileDisapearModuleData)data.data;
        level = pdata.level;
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
