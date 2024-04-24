using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : Projectile
{
    public float baseHitCd = 0.3f;
    public float hitCd = 0.2f;

    private Dictionary<HitBox, bool> hitBoxes = new Dictionary<HitBox, bool>();

    public override void AfterLevelSet()
    {
        hitCd = baseHitCd * (1 - level * 0.05f);
        mult = baseMult * (1 + level * 0.1f);
    }

    public override void AfterSet()
    {
    }

    public override ModuleData GetData()
    {
        DefaultProjectileData data = new DefaultProjectileData();
        data.className = className;
        data.level = level;
        data.hitCd = baseHitCd;
        data.mult = baseMult;

        return data;
    }

    public override void Hit()
    {
        InvokeAction();
    }

    public override void SetData(ModuleData data)
    {
        DefaultProjectileData pdata = data as DefaultProjectileData;
        baseHitCd = pdata.hitCd;
        baseMult = pdata.mult;
        level = data.level;
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseHitCd = Random.Range(0.2f, 0.8f) / mult;
        baseMult = Random.Range(0.8f, 1.2f) * mult;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HitBox>(out HitBox health))
        {
            if (hitBoxes.TryGetValue(health, out bool can))
            {
                if (can)
                {
                    Hit();
                    health.Damage(weaponShoot.weaponAction.weapon.currentDamage * mult, 1, collision.ClosestPoint(transform.position));
                    hitBoxes[health] = false;
                    StartCoroutine(ResetHit(health));
                }
            }
            else
            {
                Hit();
                health.Damage(weaponShoot.weaponAction.weapon.currentDamage * mult, 1, collision.ClosestPoint(transform.position));
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

public class DefaultProjectileData : ModuleData
{
    public float hitCd;
    public float mult;
}
