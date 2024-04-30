using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectileModule : ProjectileModule
{
    public float baseRadius = 5;
    public float baseSnapPower = 0.2f;

    public float radius;
    public float snapPower = 0.5f;

    public LayerMask hitboxLayers;

    private Rigidbody2D rb;

    public override void AfterSet()
    {
        rb = projectile.GetComponent<Rigidbody2D>();
        StartCoroutine(FindEnemy());
    }

    private IEnumerator FindEnemy()
    {
        while (true)
        {
            Collider2D enemy = Physics2D.OverlapCircle(transform.position, radius, hitboxLayers);

            if (enemy != null && enemy.TryGetComponent<HitBox>(out HitBox hitbox))
            {
                StartCoroutine(Home(hitbox.transform));
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Home(Transform target)
    {
        while (true)
        {
            Vector3 dir = target.position - transform.position;
            Vector3 direction = Vector3.Lerp(rb.velocity, dir, snapPower);
            rb.velocity = direction.normalized * rb.velocity.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public override void ProjectileHit(Vector3 pos)
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);
    }

    public override void AfterLevelSet()
    {
        radius = baseRadius * (1 + level * 0.1f);
        snapPower = baseSnapPower * (1 + level * 0.05f);
    }

    public override ModuleDataType GetData()
    {
        HomingProjectileData data = new HomingProjectileData();
        data.className = className;
        data.level = level;
        data.radius = baseRadius;
        data.snapPower = baseSnapPower;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void SetData(ModuleDataType data)
    {
        HomingProjectileData pdata = (HomingProjectileData)data.data;
        level = pdata.level;
        baseRadius = pdata.radius;
        baseSnapPower = pdata.snapPower;
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseRadius = Random.Range(5, 15) * mult;
        baseSnapPower = Random.Range(0.1f, 0.3f) * mult;
    }
}

public class HomingProjectileData : ModuleData
{
    public float radius;
    public float snapPower;
}
