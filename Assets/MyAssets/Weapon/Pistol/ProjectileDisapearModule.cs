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
}
