using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class WeaponBuilder : MonoBehaviour
{
    [Header("Stats Range")]
    [Header("Ranged")]
    public int minRangedDamage = 1;
    public int maxRangedDamage = 50;
    [Space(3)]
    public float minRangedAttackSpeed = 0.1f;
    public float maxRangedAttackSpeed = 1f;

    [Space(5)]
    [Header("Melee")]
    public int minMeleeDamage = 20;
    public int maxMeleeDamage = 100;
    [Space(3)]
    public float minMeleeAttackSpeed = 0.5f;
    public float maxMeleeAttackSpeed = 2f;

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

    public IEnumerator BuildWeapon(WeaponType type, System.Action<Weapon> callback, int level = 1)
    {
        Weapon weapon = new GameObject("Weapon").AddComponent<Weapon>();

        if (type == WeaponType.Ranged)
        {
            int damage = Random.Range(minRangedDamage, maxRangedDamage);
            float percentDamageLuck = (float)damage / maxRangedDamage;

            float attackSpeed = Random.Range(minRangedAttackSpeed * (1 + percentDamageLuck * 0.5f), maxRangedAttackSpeed);

            weapon.baseDamage = damage;
            weapon.baseAttackSpeed = attackSpeed;
            weapon.baseScale = 1;

            PistolAction action = new GameObject("Weapon Action").AddComponent<PistolAction>();
            weapon.weaponAction = action;
            action.transform.SetParent(weapon.transform);

            WeaponShoot shoot = BuildWeaponShoot(type, level);
            shoot.transform.SetParent(weapon.transform);
            weapon.weaponShoot = shoot;

            Projectile projectile = null;
            yield return BuildWeaponProjectile(type, (item) => projectile = item, 1);
            weapon.weaponShoot.projectile = projectile;

            AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(rangedModelsLabel, null);
            yield return new WaitUntil(() => handle.IsDone);
            weapon.weaponModelPrefab = handle.Result[Random.Range(0, handle.Result.Count)];
        }
        else
        {
            int damage = Random.Range(minRangedDamage, maxRangedDamage);
            float percentDamageLuck = (float)damage / maxRangedDamage;

            float attackSpeed = Random.Range(minRangedAttackSpeed * (1 + percentDamageLuck * 0.5f), maxRangedAttackSpeed);

            weapon.baseDamage = damage;
            weapon.baseAttackSpeed = attackSpeed;
            weapon.baseScale = 1;

            SwordAction action = new GameObject("Weapon Action").AddComponent<SwordAction>();
            weapon.weaponAction = action;
            action.transform.SetParent(weapon.transform);

            WeaponShoot shoot = BuildWeaponShoot(type, level);
            shoot.transform.SetParent(weapon.transform);
            weapon.weaponShoot = shoot;

            Projectile projectile = null;
            yield return BuildWeaponProjectile(type, (item) => projectile = item, 1);
            weapon.weaponShoot.projectile = projectile;
            weapon.weaponModelPrefab = projectile.gameObject;
        }

        weapon.enabled = false;

        callback(weapon);
    }

    public WeaponShoot BuildWeaponShoot(WeaponType type, int level = 1)
    {
        GameObject root = new GameObject("Weapon Shoot");

        WeaponShoot weaponShoot = null;

        switch (type)
        {
            case WeaponType.Ranged:
                weaponShoot = root.AddComponent<PistolShoot>();
                break;
            case WeaponType.MeleeSwing:
                weaponShoot = root.AddComponent<SwingSwordShoot>();
                break;
            case WeaponType.MeleeDelault:
                weaponShoot = root.AddComponent<SwordShoot>();
                break;
            case WeaponType.MeleeSpear:
                weaponShoot = root.AddComponent<SpearShoot>();
                break;
        }

        weaponShoot.SetLevel(level);

        return weaponShoot;
    }

    public IEnumerator BuildWeaponProjectile(WeaponType type, System.Action<Projectile> callback, int level = 1)
    {
        AsyncOperationHandle<IList<Object>> objs = new AsyncOperationHandle<IList<Object>>();
        Projectile projectile = null;

        switch (type)
        {
            case WeaponType.Ranged:
                objs = Addressables.LoadAssetsAsync<Object>(rangedProjectileLabel, null);
                break;
            case WeaponType.MeleeSwing:
                objs = Addressables.LoadAssetsAsync<Object>(meleeSwingProjectileLabel, null);
                break;
            case WeaponType.MeleeDelault:
                objs = Addressables.LoadAssetsAsync<Object>(meleeDefaultProjectileLabel, null);
                break;
            case WeaponType.MeleeSpear:
                objs = Addressables.LoadAssetsAsync<Object>(meleeSpearProjectileLabel, null);
                break;
        }

        yield return new WaitUntil(() => objs.IsDone);

        projectile = objs.Result[Random.Range(0, objs.Result.Count)].GetComponent<Projectile>();

        callback(projectile);

        yield return null;
    }
}

public enum WeaponType
{
    Ranged,
    MeleeDelault,
    MeleeSwing,
    MeleeSpear
}
