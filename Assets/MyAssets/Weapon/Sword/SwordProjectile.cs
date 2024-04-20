using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile : Projectile
{
    public float hitCd = 0.3f;

    private Dictionary<HitBox, bool> hitBoxes = new Dictionary<HitBox, bool>();

    public override void AfterLevelSet(int level)
    {
        hitCd = hitCd * (1 - level * 0.05f);
    }

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
            if (hitBoxes.TryGetValue(health, out bool can))
            {
                if (can)
                {
                    health.Damage(weaponShoot.weaponAction.weapon.baseDamage, 1, collision.ClosestPoint(transform.position));
                    hitBoxes[health] = false;
                    StartCoroutine(ResetHit(health));
                }
            }
            else
            {
                health.Damage(weaponShoot.weaponAction.weapon.baseDamage, 1, collision.ClosestPoint(transform.position));
                hitBoxes.Add(health, false);
                StartCoroutine(ResetHit(health));
            }
        }
    }

    private IEnumerator ResetHit(HitBox health)
    {
        yield return new WaitForSeconds(hitCd);
        hitBoxes[health] = true;
    }
}
