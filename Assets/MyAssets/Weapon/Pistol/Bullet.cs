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

    public override ModuleDataType GetData()
    {
        return new ModuleDataType();
    }

    public override void Hit()
    {
    }

    public override void SetData(ModuleDataType data)
    {

    }

    public override void SetRandomBaseStats(float mult)
    {
    }
}
