using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEditor.Recorder.OutputPath;


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
    [Header("Module Chances")]
    public float baseModuleChance = 0.2f;
    public float nextModuleChanceDecrease = 0.5f;

    [Space(5)]
    [Header("PartsAddresableLabels")]
    [Header("Ranged")]
    public AssetLabelReference rangedProjectileLabel;
    public AssetLabelReference rangedModelsLabel;
    [Header("MeleeDefault")]
    public AssetLabelReference meleeDefaultProjectileLabel;
    [Header("MeleeSwing")]
    public AssetLabelReference meleeSwingProjectileLabel;
    [Header("MeleeSpear")]
    public AssetLabelReference meleeSpearProjectileLabel;

    [Space(5)]
    [Header("ModulesAddressablesLabels")]
    [Space(3)]
    [Header("Shoot")]
    [Header("Ranged")]
    public AssetLabelReference rangedShootModuleLabel;
    [Header("Melee")]
    public AssetLabelReference meleeShootModuleLabel;

    [Space(1)]
    [Header("Projectile")]
    [Header("Ranged")]
    public AssetLabelReference rangeProjectileModuleLabel;
    [Header("Melee")]
    public AssetLabelReference meleeProjectileModuleLabel;

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
            Projectile pr = Instantiate(projectile, weapon.weaponShoot.transform);
            weapon.projectileAddressablesPath = AssetDatabase.GetAssetPath(projectile);
            weapon.weaponShoot.projectile = pr;
            pr.gameObject.SetActive(false);

            AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(rangedModelsLabel, null);
            yield return new WaitUntil(() => handle.IsDone);
            GameObject res = handle.Result[Random.Range(0, handle.Result.Count)];
            weapon.weaponModelPrefab = res;
            weapon.weaponModelAddressablesPath = AssetDatabase.GetAssetPath(res);
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
            Projectile pr = Instantiate(projectile, weapon.weaponShoot.transform);

            weapon.projectileAddressablesPath = AssetDatabase.GetAssetPath(projectile);
            weapon.weaponShoot.projectile = pr;
            weapon.weaponModelAddressablesPath = weapon.projectileAddressablesPath;
            weapon.weaponModelPrefab = pr.gameObject;
            pr.gameObject.SetActive(false);
        }

        float shootChanceNumber = 100 * baseModuleChance * level;

        while (true)
        {
            if (Random.Range(0, 100) <= shootChanceNumber)
            {
                yield return BuildShootModule(type, (mod) =>
                {
                    if (mod != null)
                    {
                        ShootModule module = Instantiate(mod, weapon.weaponShoot.transform);
                        module.gameObject.SetActive(true);
                        module.SetRandomBaseStats(1);
                        module.SetLevel(mod.level);                   
                        weapon.weaponShoot.modules.Add(module);
                    }
                });
            }
            else
            {
                break;
            }

            shootChanceNumber *= nextModuleChanceDecrease;
        }

        float projectileChanceNumber = 100 * baseModuleChance * level;

        while (true)
        {
            if (Random.Range(0, 100) <= projectileChanceNumber)
            {
                yield return BuildProjectileModule(type, (mod, path) =>
                {
                    if (mod != null)
                    {
                        ProjectileModule module = Instantiate(mod, weapon.weaponShoot.projectile.transform);
                        module.gameObject.SetActive(true);
                        module.SetRandomBaseStats(1);
                        module.SetLevel(mod.level);
                        module.addressablesPath = path;
                        weapon.weaponShoot.projectile.projectileModules.Add(module);
                    }
                });
            }
            else
            {
                break;
            }

            projectileChanceNumber *= nextModuleChanceDecrease;
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

        Object res = objs.Result[Random.Range(0, objs.Result.Count)];
        projectile = res.GetComponent<Projectile>();
        callback(projectile);

        yield return null;
    }

    public IEnumerator BuildShootModule(WeaponType type, System.Action<ShootModule> callback, int level = 1)
    {
        ShootModule module = null;
        AsyncOperationHandle<IList<Object>> objs = new AsyncOperationHandle<IList<Object>>();

        if (type == WeaponType.Ranged)
        {
            objs = Addressables.LoadAssetsAsync<Object>(rangedShootModuleLabel, null);
        }
        else
        {
            objs = Addressables.LoadAssetsAsync<Object>(meleeShootModuleLabel, null);
        }

        yield return new WaitUntil(() => objs.IsDone);

        if (objs.Result != null) 
        {
            Object res = objs.Result[Random.Range(0, objs.Result.Count)];
            module = res.GetComponent<ShootModule>();
            module.SetLevel(level);
        }

        callback(module);

        yield return null;
    }

    public IEnumerator BuildProjectileModule(WeaponType type, System.Action<ProjectileModule, string> callback, int level = 1)
    {
        ProjectileModule module = null;
        AsyncOperationHandle<IList<Object>> objs = new AsyncOperationHandle<IList<Object>>();

        if (type == WeaponType.Ranged)
        {
            objs = Addressables.LoadAssetsAsync<Object>(rangeProjectileModuleLabel, null);
        }
        else
        {
            objs = Addressables.LoadAssetsAsync<Object>(meleeProjectileModuleLabel, null);
        }

        string path = "";

        yield return new WaitUntil(() => objs.IsDone);

        if (objs.Result != null)
        {
            Object res = objs.Result[Random.Range(0, objs.Result.Count)];
            module = res.GetComponent<ProjectileModule>();
            path = AssetDatabase.GetAssetPath(res);
        }

        callback(module, path);

        yield return null;
    }

    public IEnumerator BuildWeaponFromJson(WeaponBaseData data, System.Action<Weapon> callback) 
    {
        Weapon weapon = new GameObject("Weapon").AddComponent<Weapon>();
        weapon.SetData(data);

        WeaponAction action = Instantiate(new GameObject("Weapon Action"), weapon.transform).AddComponent(System.Type.GetType(data.actionData.data.className)) as WeaponAction;
        action.SetData(data.actionData);
        
        foreach (ModuleDataType item in data.actionData.data.modules)
        {
            ActionModule module = Instantiate(new GameObject(item.data.className), action.transform).AddComponent(System.Type.GetType(item.data.className)) as ActionModule;
            module.SetData(item);
        }

        WeaponShoot shoot = Instantiate(new GameObject("Weapon Shoot"), weapon.transform).AddComponent(System.Type.GetType(data.shootData.data.className)) as WeaponShoot;
        shoot.SetData(data.shootData);

        foreach (ModuleDataType item in data.shootData.data.modules)
        {
            ShootModule module = Instantiate(new GameObject(item.data.className), shoot.transform).AddComponent(System.Type.GetType(item.data.className)) as ShootModule;
            module.SetData(item);
        }

        Projectile projectile = null;
        var handle = Addressables.LoadAssetAsync<GameObject>(data.projectileAddressablesPath);

        yield return handle;

        projectile = Instantiate(handle.Result, shoot.transform).GetComponent<Projectile>();
        projectile.SetData(data.projectileData);
        projectile.gameObject.SetActive(false);

        foreach (ModuleDataType item in data.projectileData.data.modules)
        {
            var pmhandle = Addressables.LoadAssetAsync<GameObject>(item.data.addressablesPath);
            yield return pmhandle;
            ProjectileModule module = Instantiate(pmhandle.Result, projectile.transform).GetComponent<ProjectileModule>();
            module.SetData(item);
        }

        weapon.weaponAction = action;
        weapon.weaponShoot = shoot;
        weapon.weaponShoot.projectile = projectile;

        var modelHandle = Addressables.LoadAssetAsync<GameObject>(data.weaponModelAddressablesPath);

        yield return handle;

        weapon.weaponModelPrefab = modelHandle.Result;

        callback(weapon);

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
