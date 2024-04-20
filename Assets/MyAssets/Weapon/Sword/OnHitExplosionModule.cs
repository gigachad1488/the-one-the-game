using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitExplosionModule : ProjectileModule
{
    public ParticleSystem explosionParticles;

    public float mult = 0.5f;

    public float explosionCd = 0.5f;
    private bool canExplode;

    public float radius = 2f;

    public LayerMask hitLayers;
    public override void AfterSet()
    {
        canExplode = true;
    }

    public override void ProjectileHit()
    {
        if (canExplode)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, hitLayers);

            if (hits.Length > 0)
            {
                canExplode = false;
                ParticleSystem particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
                particles.transform.localScale *= radius;
                particles.Emit(20);
                Invoke(nameof(ExplosionReload), explosionCd);
            }

            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<HitBox>(out HitBox enemy))
                {
                    enemy.Damage(projectile.weaponShoot.weaponAction.weapon.baseDamage * mult, 1, hit.ClosestPoint(transform.position));
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

    public override void SetLevel(int level)
    {
        mult = mult * (1 + level * 0.1f);
        radius = radius * (1 + level * 0.1f);
        explosionCd = explosionCd * (1 + level * 0.1f);
    }
}
