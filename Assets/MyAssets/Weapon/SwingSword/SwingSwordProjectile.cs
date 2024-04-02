using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSwordProjectile : Projectile
{
    private Dictionary<HitBox, bool> hitBoxes = new Dictionary<HitBox, bool>();
    public override void AfterSet()
    {
    }

    public override void Hit()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HitBox>(out HitBox health))
        {
            if (hitBoxes.TryGetValue(health, out bool can))
            {
                if (can)
                {
                    health.Damage(20, 1, collision.ClosestPoint(transform.position));
                    hitBoxes[health] = false;
                }
            }
            else
            {
                health.Damage(20, 1, collision.ClosestPoint(transform.position));
                hitBoxes.Add(health, false);
            }
        }
    }
}
