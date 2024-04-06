using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceLimitProjectileModule : ProjectileModule
{
    public float pierceCount = 1f;

    public override void AfterSet()
    {
    }

    public override void ProjectileHit()
    {
        pierceCount--;

        if (pierceCount <= 0)
        {
            Destroy(projectile.gameObject);
        }    
    }
}
