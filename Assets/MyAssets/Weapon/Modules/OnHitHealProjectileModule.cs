using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnHitHealProjectileModule : ProjectileModule
{
    public float baseHealCd = 1f;
    public int baseHealAmount = 10;

    public float healCd = 5;
    public int healAmount;

    public HealBuble healBublePrefab;

    public OnHitHealCdTimer timer;

    public override void AfterLevelSet()
    {
        healCd = baseHealCd * (1 - level * 0.01f);
        healAmount = System.Convert.ToInt32(baseHealAmount * (1 + level * 0.1f));
    }

    public override void AfterSet()
    {
    }

    public override ModuleDataType GetData()
    {
        HealProjectileModuleData data = new HealProjectileModuleData();
        data.healAmount = baseHealAmount;
        data.healCd = baseHealCd;
        data.level = level;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void ProjectileHit(Vector3 position)
    {
        if (projectile.weaponShoot.TryGetComponent<OnHitHealCdTimer>(out OnHitHealCdTimer hitTimer))
        {
            timer = hitTimer;
        }
        else
        {
            timer = projectile.weaponShoot.AddComponent<OnHitHealCdTimer>();
            timer.time = healCd;
        }

        if (timer.CanHeal()) 
        {
            HealBuble healBuble = Instantiate(healBublePrefab, position, Quaternion.identity);
            healBuble.healAmount = healAmount;
            healBuble.speed = level * 0.02f;
            healBuble.player = projectile.weaponShoot.weaponAction.weapon.player;
        }
    }

    public override void SetData(ModuleDataType data)
    {
        HealProjectileModuleData pdata = data.data as HealProjectileModuleData;
        baseHealCd = pdata.healCd;
        baseHealAmount = pdata.healAmount;
        level = pdata.level;
    }

    public override void SetRandomBaseStats(float mult)
    {
        baseHealCd = Random.Range(4f, 5f * mult);
        baseHealAmount = System.Convert.ToInt32(Random.Range(3, 10) * mult);
    }
}

public class HealProjectileModuleData : ModuleData
{
    public float healCd;
    public int healAmount;
}
