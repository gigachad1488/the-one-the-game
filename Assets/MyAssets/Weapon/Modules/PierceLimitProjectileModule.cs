using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceLimitProjectileModule : ProjectileModule
{
    public int basePierceCount = 1;
    public int pierceCount = 1;

    public override void AfterSet()
    {
    }

    public override ModuleData GetData()
    {
        PierceLimitProjectileModuleData data = new PierceLimitProjectileModuleData();
        data.className = className;
        data.level = level;
        data.pierceCount = pierceCount;

        return data;
    }

    public override void ProjectileHit()
    {
        pierceCount--;

        if (pierceCount <= 0)
        {
            Destroy(projectile.gameObject);
        }    
    }

    public override void SetData(ModuleData data)
    {
        PierceLimitProjectileModuleData pdata = data as PierceLimitProjectileModuleData;
        basePierceCount = pdata.pierceCount;
        level = pdata.level;
    }

    public override void AfterLevelSet()
    {
        pierceCount = Mathf.RoundToInt(basePierceCount * (1 + level * 0.01f)); 
    }

    public override void SetRandomBaseStats(float mult)
    {
        basePierceCount = Random.Range(1, 3);
    }
}

public class PierceLimitProjectileModuleData : ModuleData
{
    public int pierceCount;
}
