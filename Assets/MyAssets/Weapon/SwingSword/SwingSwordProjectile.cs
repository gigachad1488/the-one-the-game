using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSwordProjectile : Projectile
{
    private Dictionary<HitBox, bool> hitBoxes = new Dictionary<HitBox, bool>();

    public override void AfterLevelSet()
    {
        mult = mult * (1 + level * 0.1f);
    }

    public override void AfterSet()
    {
    }

    public override ModuleData GetData()
    {
        ModuleData data = new ModuleData();
        data.className = className;
        data.level = level;

        return data;
    }

    public override void Hit()
    {
    }

    public override void SetData(ModuleData data)
    {
        level = data.level;
    }

    public override void SetRandomBaseStats(float mult)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HitBox>(out HitBox health))
        {
            if (hitBoxes.TryGetValue(health, out bool can))
            {
                if (can)
                {
                    health.Damage(weaponShoot.weaponAction.weapon.currentDamage * mult, 1, collision.ClosestPoint(transform.position));
                    hitBoxes[health] = false;
                }
            }
            else
            {
                health.Damage(weaponShoot.weaponAction.weapon.currentDamage * mult, 1, collision.ClosestPoint(transform.position));
                hitBoxes.Add(health, false);
            }
        }
    }
}
