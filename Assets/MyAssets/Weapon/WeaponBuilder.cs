using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            int damage = System.Convert.ToInt32(Random.Range(minRangedDamage, maxRangedDamage) * 1 + (level * 0.5f));
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
            yield return BuildWeaponProjectile(type, (item) => projectile = item, level);
            Projectile pr = Instantiate(projectile, weapon.weaponShoot.transform);
            weapon.projectileAddressablesPath = projectile.GetComponent<AddressablePath>().path;
            weapon.weaponShoot.projectile = pr;
            pr.gameObject.SetActive(false);

            AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(rangedModelsLabel, null);
            yield return handle;
            GameObject res = handle.Result[Random.Range(0, handle.Result.Count)];
            weapon.weaponModelPrefab = res;
            weapon.weaponModelAddressablesPath = res.GetComponent<AddressablePath>().path;
        }
        else
        {
            int damage = System.Convert.ToInt32(Random.Range(minMeleeDamage, maxMeleeDamage) * 1 + (level * 0.5f));
            float percentDamageLuck = (float)damage / maxMeleeDamage;

            float attackSpeed = Random.Range(minMeleeAttackSpeed * (1 + percentDamageLuck * 0.5f), maxMeleeAttackSpeed);

            weapon.baseDamage = damage;
            weapon.baseAttackSpeed = attackSpeed;
            weapon.baseScale = 1;

            if (attackSpeed / maxMeleeAttackSpeed > 0.5f)
            {
                weapon.ScaleMult = Random.Range(0.2f, 0.8f);
            }

            SwordAction action = new GameObject("Weapon Action").AddComponent<SwordAction>();
            weapon.weaponAction = action;
            action.transform.SetParent(weapon.transform);

            WeaponShoot shoot = BuildWeaponShoot(type, level);
            shoot.transform.SetParent(weapon.transform);
            weapon.weaponShoot = shoot;

            Projectile projectile = null;
            yield return BuildWeaponProjectile(type, (item) => projectile = item, level);
            Projectile pr = Instantiate(projectile, weapon.weaponShoot.transform);

            weapon.projectileAddressablesPath = projectile.GetComponent<AddressablePath>().path;
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
                        if (!weapon.weaponShoot.modules.Any(x => x.GetType() == mod.GetType()))
                        {
                            ShootModule module = Instantiate(mod, weapon.weaponShoot.transform);
                            module.gameObject.SetActive(true);
                            module.SetRandomBaseStats(1);
                            weapon.weaponShoot.modules.Add(module);
                        }
                        else
                        {
                            shootChanceNumber *= nextModuleChanceDecrease;
                        }
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
                        if (!weapon.weaponShoot.projectile.projectileModules.Any(x => x.GetType() == mod.GetType()))
                        {
                            ProjectileModule module = Instantiate(mod, weapon.weaponShoot.projectile.transform);
                            module.gameObject.SetActive(true);
                            module.SetRandomBaseStats(1);
                            module.addressablesPath = path;
                            weapon.weaponShoot.projectile.projectileModules.Add(module);
                        }
                        else
                        {
                            projectileChanceNumber *= nextModuleChanceDecrease;
                        }
                    }
                });
            }
            else
            {
                break;
            }

            projectileChanceNumber *= nextModuleChanceDecrease;
        }

        weapon.guid = System.Guid.NewGuid();

        weapon.level = level;
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
        AsyncOperationHandle<IList<GameObject>> objs = new AsyncOperationHandle<IList<GameObject>>();
        Projectile projectile = null;

        switch (type)
        {
            case WeaponType.Ranged:
                objs = Addressables.LoadAssetsAsync<GameObject>(rangedProjectileLabel, null);
                break;
            case WeaponType.MeleeSwing:
                objs = Addressables.LoadAssetsAsync<GameObject>(meleeSwingProjectileLabel, null);
                break;
            case WeaponType.MeleeDelault:
                objs = Addressables.LoadAssetsAsync<GameObject>(meleeDefaultProjectileLabel, null);
                break;
            case WeaponType.MeleeSpear:
                objs = Addressables.LoadAssetsAsync<GameObject>(meleeSpearProjectileLabel, null);
                break;
        }

        yield return new WaitUntil(() => objs.IsDone);

        GameObject res = objs.Result[Random.Range(0, objs.Result.Count)];
        projectile = res.GetComponent<Projectile>();
        callback(projectile);

        yield return null;
    }

    public IEnumerator BuildShootModule(WeaponType type, System.Action<ShootModule> callback, int level = 1)
    {
        ShootModule module = null;
        AsyncOperationHandle<IList<GameObject>> objs = new AsyncOperationHandle<IList<GameObject>>();
        bool cancel = false;

        try
        {
            if (type == WeaponType.Ranged)
            {
                objs = Addressables.LoadAssetsAsync<GameObject>(rangedShootModuleLabel, null);
            }
            else
            {
                objs = Addressables.LoadAssetsAsync<GameObject>(meleeShootModuleLabel, null);
            }
        }
        catch
        {
            cancel = true;
        }

        if (cancel)
        {
            callback(null);
            yield return null;
        }

        yield return new WaitUntil(() => objs.IsDone);

        if (objs.Result != null)
        {
            GameObject res = objs.Result[Random.Range(0, objs.Result.Count)];
            module = res.GetComponent<ShootModule>();
            module.SetLevel(level);
        }

        callback(module);

        yield return null;
    }

    public IEnumerator BuildProjectileModule(WeaponType type, System.Action<ProjectileModule, string> callback, int level = 1)
    {
        ProjectileModule module = null;
        AsyncOperationHandle<IList<GameObject>> objs = new AsyncOperationHandle<IList<GameObject>>();

        bool cancel = false;

        try
        {
            if (type == WeaponType.Ranged)
            {
                objs = Addressables.LoadAssetsAsync<GameObject>(rangeProjectileModuleLabel, null);
            }
            else
            {
                objs = Addressables.LoadAssetsAsync<GameObject>(meleeProjectileModuleLabel, null);
            }
        }
        catch
        {
            cancel = true;
        }

        string path = "";

        if (cancel)
        {
            callback(null, null);
            yield return null;
        }

        yield return new WaitUntil(() => objs.IsDone);

        if (objs.Result != null)
        {
            GameObject res = objs.Result[Random.Range(0, objs.Result.Count)];
            module = res.GetComponent<ProjectileModule>();
            path = res.GetComponent<AddressablePath>().path;
        }

        callback(module, path);

        yield return null;
    }

    public IEnumerator BuildWeaponFromJson(WeaponBaseData data, string name, System.Action<Weapon> callback)
    {
        Weapon weapon = new GameObject("Weapon").AddComponent<Weapon>();
        weapon.SetData(data);
        weapon.guid = System.Guid.Parse(name.Remove(0, 1));

        WeaponAction action = new GameObject("Weapon Action").AddComponent(System.Type.GetType(data.actionData.data.className)) as WeaponAction;
        action.transform.SetParent(weapon.transform);
        action.SetData(data.actionData);

        foreach (ModuleDataType item in data.actionData.data.modules)
        {
            ActionModule module = new GameObject(item.data.className).AddComponent(System.Type.GetType(item.data.className)) as ActionModule;
            module.transform.SetParent(weapon.transform);
            module.SetData(item);
        }

        WeaponShoot shoot = new GameObject("Weapon Shoot").AddComponent(System.Type.GetType(data.shootData.data.className)) as WeaponShoot;
        shoot.transform.SetParent(weapon.transform);
        shoot.SetData(data.shootData);

        foreach (ModuleDataType item in data.shootData.data.modules)
        {
            ShootModule shootModule = null;
            var obj = Addressables.LoadAssetAsync<GameObject>(item.data.addressablesPath);
            yield return obj;
            shootModule = Instantiate(obj.Result, shoot.transform).GetComponent<ShootModule>();
            shootModule.SetData(item);
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

        yield return modelHandle;

        weapon.weaponModelPrefab = modelHandle.Result;

        callback(weapon);

        yield return null;
    }

    public IEnumerator BuildStartWeapon(System.Action<Weapon> callback)
    {
        Weapon weapon = new GameObject("Weapon").AddComponent<Weapon>();
        weapon.baseDamage = 40;
        weapon.baseAttackSpeed = 0.3f;
        weapon.baseScale = 1;

        WeaponAction action = new GameObject("Weapon Action").AddComponent<PistolAction>();
        action.transform.SetParent(weapon.transform);
        weapon.weaponAction = action;

        var weaponModel = Addressables.LoadAssetAsync<GameObject>("Assets/MyAssets/Weapon/Parts/Ranged/PistolModel.prefab");
        yield return weaponModel;
        weapon.weaponModelPrefab = weaponModel.Result;

        WeaponShoot shoot = new GameObject("Weapon Shoot").AddComponent<PistolShoot>();
        shoot.transform.SetParent(weapon.transform);
        weapon.weaponShoot = shoot;

        var proj = Addressables.LoadAssetAsync<GameObject>("Assets/MyAssets/Weapon/Parts/Projectiles/Ranged/Bullet.prefab");
        yield return proj;
        weapon.weaponShoot.projectile = proj.Result.GetComponent<Projectile>();

        weapon.weaponModelAddressablesPath = weaponModel.Result.GetComponent<AddressablePath>().path;
        weapon.projectileAddressablesPath = proj.Result.GetComponent<AddressablePath>().path;

        weapon.level = 1;

        weapon.guid = System.Guid.NewGuid();

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
