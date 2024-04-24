using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public override void AfterLevelSet()
    {

    }

    public override void AfterSet()
    {
    }

    public override ModuleData GetData()
    {
        return new ModuleData();
    }

    public override void Hit()
    {
    }

    public override void SetData(ModuleData data)
    {

    }

    public override void SetRandomBaseStats(float mult)
    {
    }
}
