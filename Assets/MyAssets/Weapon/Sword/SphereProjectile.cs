using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProjectile : Projectile
{
    public override void AfterSet()
    {

    }

    public override void Hit()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.TryGetComponent<HitBox>(out HitBox health)) 
        {
            Debug.Log(":GET");
            health.Damage(20, 1, transform.position);
        }
    }
}
