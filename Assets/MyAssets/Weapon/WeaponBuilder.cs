using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


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

    public IEnumerator BuildWeapon(WeaponType type, System.Action<Weapon> callback, int level = 1)
    {
        Weapon weapon = new GameObject("Weapon").AddComponent<Weapon>();

        if (type == WeaponType.Ranged)
        {
            weapon.baseDamage = Random.Range(5, 30);
            weapon.baseAttackSpeed = Random.Range(0.2f, 2f);

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
            weapon.baseDamage = Random.Range(30, 70);
            weapon.baseAttackSpeed = Random.Range(0.5f, 3f);

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
            case WeaponType.MeleeDelault:
            case WeaponType.MeleeSpear:
                weaponShoot = root.AddComponent<SwordShoot>();
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

        projectile.SetLevel(level);

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
