using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class OnHitThunderModule : ProjectileModule
{
    public DefaultProjectile thunderPrefab;

    public float baseMult = 0.5f;
    public float mult = 0.5f;

    public float baseExplosionCd = 1f;
    public float explosionCd = 0.5f;
    private bool canExplode;

    public float baseRadius = 2f;
    public float radius = 2f;

    public float baseLightingCount = 1;
    public int lightingCount = 1;

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
            canExplode = false;

            for (int i = 0; i < lightingCount; i++) 
            {
                float angle = Random.Range(0, 360);
                DefaultProjectile projectile = Instantiate(thunderPrefab, pos, Quaternion.Euler(0, 0, angle));
                projectile.gameObject.SetActive(true);
                projectile.Set(this.projectile.weaponShoot);
                projectile.SetLevel(level);
                projectile.mult = mult * 0.5f;
                projectile.transform.localScale = Vector3.one;
                projectile.transform.position += projectile.transform.right * 1.5f;
            }

            Invoke(nameof(ExplosionReload), explosionCd);
        }
    }

    public void InitProjectile()
    {
        thunderPrefab = Instantiate(thunderPrefab, transform);

        thunderPrefab.gameObject.SetActive(false);
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
        lightingCount = Mathf.RoundToInt(baseLightingCount * (1 + level * 0.1f));
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

        var obj = Addressables.LoadAssetAsync<GameObject>("Assets/MyAssets/Weapon/Modules/ThunderModuleProjectilePrefab.prefab");
        obj.WaitForCompletion();

        thunderPrefab = obj.Result.GetComponent<DefaultProjectile>();

        InitProjectile();
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseMult = Random.Range(0.1f, 0.4f) * mult;
        baseRadius = Random.Range(2, 3) * mult;
        baseExplosionCd = Random.Range(0.4f, 1) / mult;
        baseLightingCount = Random.Range(0.8f, 1.6f) / mult;

        var obj = Addressables.LoadAssetAsync<GameObject>("Assets/MyAssets/Weapon/Modules/ThunderModuleProjectilePrefab.prefab");
        obj.WaitForCompletion();

        thunderPrefab = obj.Result.GetComponent<DefaultProjectile>();

        InitProjectile();
    }
}

public class OnHitThunderModuleData : ModuleData
{
    public float explosionCd;
    public float radius;
    public float mult;
    public int thunderCount;
}
