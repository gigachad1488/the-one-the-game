using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.AddressableAssets;

public class OnHitLaserModule : ProjectileModule
{
    public DefaultProjectile laserPrefab;

    public float baseMult = 0.5f;
    public float mult = 0.5f;

    private bool canStrike = true;

    public float baseRadius = 2f;
    public float radius = 2f;

    private Gradient gradient;
    public Color color;

    public LayerMask hitLayers;
    public override void AfterSet()
    {
        canStrike = true;
        gameObject.SetActive(true);

        gradient = new Gradient();
        gradient.mode = GradientMode.Fixed;
        gradient.colorKeys = new GradientColorKey[1] { new GradientColorKey(color, 1) };
    }

    public override void ProjectileHit(Vector3 pos)
    {
        if (canStrike)
        {
            canStrike = false;
            Vector3 weaponPos = projectile.weaponShoot.transform.position;
            float randX = Random.Range(weaponPos.x - 2, weaponPos.x + 2);
            float randY = Random.Range(weaponPos.y - 4, weaponPos.y + 4);
            DefaultProjectile proj = Instantiate(laserPrefab, new Vector3(randX, randY, 0), Quaternion.identity);
            proj.gameObject.SetActive(true);
            proj.baseMult = mult;
            proj.Set(projectile.weaponShoot);
            proj.SetLevel(level);
            proj.hitCd = 2f;
            OnHitLaserProjectile pr = proj.GetComponent<OnHitLaserProjectile>();
            pr.destination = pos;
            pr.trailRenderer.widthMultiplier *= radius;
            pr.trailRenderer.colorGradient = gradient;
            pr.transform.localScale = new Vector3(radius, radius, radius);
        }
    }

    public void InitProjectile()
    {
        laserPrefab = Instantiate(laserPrefab, transform);

        laserPrefab.gameObject.SetActive(false);
    }

    public override void AfterLevelSet()
    {
        mult = baseMult * (1 + level * 0.1f);
        radius = baseRadius * (1 + level * 0.1f);
    }

    public override ModuleDataType GetData()
    {
        OnHitLaserModuleData data = new OnHitLaserModuleData();
        data.className = className;
        data.level = level;
        data.radius = baseRadius;
        data.mult = baseMult;
        data.color = color;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void SetData(ModuleDataType data)
    {
        OnHitLaserModuleData pdata = (OnHitLaserModuleData)data.data;
        baseMult = pdata.mult;
        baseRadius = pdata.radius;
        color = pdata.color;
        level = pdata.level;

        var obj = Addressables.LoadAssetAsync<GameObject>("Assets/MyAssets/Weapon/Modules/OnHitLaser.prefabb");
        obj.WaitForCompletion();

        laserPrefab = obj.Result.GetComponent<DefaultProjectile>();

        InitProjectile();
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseMult = Random.Range(0.1f, 0.4f) * mult;
        baseRadius = Random.Range(0.8f, 1.2f) * mult;
        int rand = Random.Range(0, 5);

        switch (rand)
        {
            case 0:
                color = Color.white;
                break;
            case 1:
                color = Color.red;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.blue;
                break;
            case 4:
                color = Color.magenta;
                break;
            case 5:
                color = Color.yellow;
                break;
            case 6:
                color = Color.cyan;
                break;
        }

        var obj = Addressables.LoadAssetAsync<GameObject>("Assets/MyAssets/Weapon/Modules/OnHitLaser.prefab");
        obj.WaitForCompletion();

        laserPrefab = obj.Result.GetComponent<DefaultProjectile>();

        InitProjectile();
    }

    public class OnHitLaserModuleData : ModuleData
    {
        public float mult;
        public float radius;
        public Color color;
    }
}
