using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDisapearModule : ProjectileModule
{
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
        time = time * (1 + level * 0.1f);
    }

    public override ModuleData GetData()
    {
        ModuleData data = new ModuleData();
        data.className = className;
        data.level = level;

        return data;
    }

    public override void SetData(ModuleData data)
    {
        level = data.level;
    }
}
