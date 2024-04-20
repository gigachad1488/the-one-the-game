using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceLimitProjectileModule : ProjectileModule
{
    public int pierceCount = 1;

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

    public override void SetLevel(int level)
    {
        pierceCount = Mathf.RoundToInt(pierceCount * (1 + level * 0.01f)); 
    }
}
