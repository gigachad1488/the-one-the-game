using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootProjectileModule : ShootModule
{
    public Projectile projectileModel;

    public float baseProjectileSpeed = 10f;
    public float projectileSpeed = 10f;

    public ProjectileData projectileData;

    public override void AfterLevelSet()
    {
        projectileSpeed = baseProjectileSpeed * (1 + level * 0.1f);
        this.level = level;
    }

    public override void AfterSet()
    {
        projectileModel.SetLevel(level);
        projectileData = projectileModel.GetAllData();
        Rigidbody2D rb = projectileModel.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.mass = 0;
        projectileModel.gameObject.SetActive(false);
    }

    public override ModuleData GetData()
    {
        ShootProjectileModuleData data = new ShootProjectileModuleData();
        data.className = className;
        data.level = level;

        return data;
    }

    public override void OnShoot(Vector2 directionOffset)
    {
        float angle = Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg;
        Projectile pr = Instantiate(projectileModel, shoot.transform.position, Quaternion.Euler(0, 0, angle));   
        
        pr.GetComponent<Rigidbody2D>().velocity = pr.transform.right * projectileSpeed;
        pr.Set(shoot);
    }

    public override void SetData(ModuleData data)
    {
        ShootProjectileModuleData pdata = (ShootProjectileModuleData)data;

        baseProjectileSpeed = pdata.projectileSpeed;
        projectileData = pdata.projectileData;
        level = pdata.level;
        projectileModel.SetData(projectileData);
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseProjectileSpeed = Random.Range(5, 20) * mult;
        projectileModel.SetRandomBaseStats(0.5f * mult);
        projectileData = projectileModel.GetAllData();
    }
}

public class ShootProjectileModuleData : ModuleData
{
    public float projectileSpeed;

    public ProjectileData projectileData;
}
