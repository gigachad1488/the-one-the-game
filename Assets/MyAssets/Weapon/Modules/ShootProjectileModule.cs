using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ShootProjectileModule : ShootModule
{
    public Projectile projectileModel;

    public float baseProjectileSpeed = 10f;
    public float projectileSpeed = 10f;

    public ModuleDataType projectileData;

    public string projectileAddressablesPath;

    public AssetLabelReference reference;

    public override void AfterLevelSet()
    {
        projectileSpeed = baseProjectileSpeed * (1 + level * 0.1f);
    }

    public override void AfterSet()
    {             
    }

    public void InitProjectile()
    {
        projectileModel = Instantiate(projectileModel, transform);

        Rigidbody2D rb = projectileModel.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.mass = 0;

        projectileModel.SetData(projectileData);
        projectileModel.SetLevel(level);

        projectileModel.gameObject.SetActive(false);
    }

    public override ModuleDataType GetData()
    {
        ShootProjectileModuleData data = new ShootProjectileModuleData();
        data.className = className;
        data.level = level;
        data.projectileData = projectileData;
        data.projectileSpeed = baseProjectileSpeed;
        data.projectileAddressablesPath = projectileAddressablesPath;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void OnShoot(Vector2 directionOffset)
    {
        float angle = Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg;
        Projectile pr = Instantiate(projectileModel, shoot.transform.position, Quaternion.Euler(0, 0, angle));
        pr.gameObject.SetActive(true);
        
        pr.GetComponent<Rigidbody2D>().velocity = pr.transform.right * projectileSpeed;
        pr.Set(shoot);
    }

    public override void SetData(ModuleDataType data)
    {
        ShootProjectileModuleData pdata = (ShootProjectileModuleData)data.data;

        baseProjectileSpeed = pdata.projectileSpeed;
        projectileData = pdata.projectileData;
        level = pdata.level;
        projectileAddressablesPath = pdata.projectileAddressablesPath;

        var obj = Addressables.LoadAssetAsync<GameObject>(projectileAddressablesPath);
        obj.WaitForCompletion();

        projectileModel = obj.Result.GetComponent<Projectile>();
        InitProjectile();

        projectileModel.SetData(projectileData);
    }

    public override void SetRandomBaseStats(float mult)
    {
        reference.labelString = "uniqueProjectiles";

        baseProjectileSpeed = Random.Range(5, 60) * mult;

        var objs = Addressables.LoadAssetsAsync<GameObject>(reference, null);
        objs.WaitForCompletion();

        GameObject res = objs.Result[Random.Range(0, objs.Result.Count)];
        projectileModel = res.GetComponent<Projectile>();       

        projectileAddressablesPath = res.GetComponent<AddressablePath>().path;

        projectileModel = Instantiate(projectileModel, transform);

        projectileModel.SetRandomBaseStats(0.3f * mult);
        projectileData = projectileModel.GetAllData();
    }
}

public class ShootProjectileModuleData : ModuleData
{
    public float projectileSpeed;

    public ModuleDataType projectileData;

    public string projectileAddressablesPath;
}
