using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitExplosionModule : ProjectileModule
{
    public ParticleSystem explosionParticles;

    public float baseMult = 0.5f;
    public float mult = 0.5f;

    public float baseExplosionCd = 1f;
    public float explosionCd = 0.5f;
    private bool canExplode;

    public float baseRadius = 2f;
    public float radius = 2f;

    public LayerMask hitLayers;
    public override void AfterSet()
    {
        canExplode = true;
        gameObject.SetActive(true);
    }

    public override void ProjectileHit(Vector3 pos)
    {
        if (canExplode)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(pos, radius, hitLayers);

            if (hits.Length > 0)
            {
                canExplode = false;
                ParticleSystem particles = Instantiate(explosionParticles, pos, Quaternion.identity);
                particles.gameObject.SetActive(true);
                particles.transform.localScale *= radius;
                particles.Emit(20);
                Invoke(nameof(ExplosionReload), explosionCd);
            }

            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<HitBox>(out HitBox enemy))
                {
                    enemy.Damage(projectile.weaponShoot.weaponAction.weapon.currentDamage * mult, 1, hit.ClosestPoint(transform.position));
                }
            }
        }
    }

    public void ExplosionReload()
    {
        canExplode = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }

    public override void AfterLevelSet()
    {
        mult = baseMult * (1 + level * 0.1f);
        radius = baseRadius * (1 + level * 0.1f);
        explosionCd = baseExplosionCd * (1 + level * 0.1f);
    }

    public override ModuleDataType GetData()
    {
        OnHitExplosionModuleData data = new OnHitExplosionModuleData();
        data.className = className;
        data.level = level;
        data.explosionCd = baseExplosionCd;
        data.radius = baseRadius;
        data.mult = baseMult;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void SetData(ModuleDataType data)
    {
        OnHitExplosionModuleData pdata = (OnHitExplosionModuleData)data.data;
        baseExplosionCd = pdata.explosionCd;
        baseMult = pdata.mult;
        baseRadius = pdata.radius;
        level = pdata.level;
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseMult = Random.Range(0.2f, 1) * mult;
        baseRadius = Random.Range(2, 3) * mult;
        explosionCd = Random.Range(0.4f, 1) / mult;
    }
}

public class OnHitExplosionModuleData : ModuleData
{
    public float explosionCd;
    public float radius;
    public float mult;
}
