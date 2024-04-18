using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class WeaponBuilder : MonoBehaviour
{
    [Space(5)]
    [Header("AddresableLabels")]
    [Header("Ranged")]
    public AssetLabelReference rangedProjectileLabel;
    public AssetLabelReference rangedModelsLabel;
    [Header("MeleeDefault")]
    public AssetLabelReference meleeDefaultProjectileLabel;
    [Header("MeleeSwing")]
    public AssetLabelReference meleeSwingProjectileLabel;
    [Header("MeleeSpear")]
    public AssetLabelReference meleeSpearProjectileLabel;

    public Weapon BuildWeapon(WeaponType type, int level = 1)
    {
        return null;
    }

    public WeaponAction BuildWeaponAction(WeaponType type, int level = 1)
    {
        return null;
    }

    public WeaponAction BuildWeaponShoot(WeaponType type, int level = 1)
    {
        return null;
    }

    public Projectile BuildWeaponProjectile(WeaponType type, int level = 1)
    {
        List<Projectile> projectiles = new List<Projectile>();
        Projectile projectile = null;

        switch (type)
        {
            case WeaponType.Ranged:
                Addressables.LoadAssetsAsync<Projectile>(rangedProjectileLabel, (a) => projectiles.Add(a));
                projectile = projectiles[Random.Range(0, projectiles.Count)];

                break;
            case WeaponType.MeleeSwing:
                break;
            case WeaponType.MeleeDelault:
                break;
            case WeaponType.MeleeSpear:
                break;
        }

        return projectile;
    }
}

public enum WeaponType
{
    Ranged,
    MeleeDelault,
    MeleeSwing,
    MeleeSpear
}
