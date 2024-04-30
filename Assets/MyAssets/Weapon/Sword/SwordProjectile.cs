using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SwordProjectile : Projectile
{
    public float baseHitCd = 0.3f;
    public float hitCd = 0.2f;

    private Dictionary<HitBox, bool> hitBoxes = new Dictionary<HitBox, bool>();

    public override void AfterLevelSet()
    {
        hitCd = baseHitCd * (1 - level * 0.05f);
    }

    public override void AfterSet()
    {
    }

    public override ModuleDataType GetData()
    {
        SwordProjectileData data = new SwordProjectileData();
        data.className = className;
        data.level = level;
        data.hitCd = baseHitCd;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void Hit()
    {     
    }

    public override void SetData(ModuleDataType data)
    {
        SwordProjectileData pdata = (SwordProjectileData)data.data;
        baseHitCd = pdata.hitCd;
        level = pdata.level;
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseHitCd = Random.Range(0.2f, 0.8f) / mult;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HitBox>(out HitBox health))
        {
            if (hitBoxes.TryGetValue(health, out bool can))
            {
                if (can)
                {
                    Vector3 closestPoint = collision.ClosestPoint(transform.position);

                    InvokeAction(closestPoint);
                    health.Damage(weaponShoot.weaponAction.weapon.currentDamage, 1, closestPoint);
                    hitBoxes[health] = false;
                    StartCoroutine(ResetHit(health));
                }
            }
            else
            {
                Vector3 closestPoint = collision.ClosestPoint(transform.position);

                InvokeAction(closestPoint);
                health.Damage(weaponShoot.weaponAction.weapon.currentDamage, 1, closestPoint);
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

public class SwordProjectileData : ModuleData
{
    public float hitCd;
}
