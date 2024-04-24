using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearShoot : WeaponShoot
{
    private bool canSwing = true;

    private float lungeLength = 2f;

    private Projectile currentProjectile;

    public override void AfterSet()
    {
        weaponAction.weapon.OnUnequip += Unequip;
    }

    public override void Shoot(Vector2 directionOffset)
    {
        if (canSwing)
        {
            Vector3 dirNorm = Vector3.Normalize(directionOffset);
            canSwing = false;
            float angle = Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg - 90;
            currentProjectile = Instantiate(projectile, weaponAction.weapon.transform.position, Quaternion.Euler(0, 0, angle), weaponAction.weapon.transform);
            currentProjectile.gameObject.SetActive(true);
            currentProjectile.Set(this);
            Tween.LocalPosition(currentProjectile.transform, dirNorm * lungeLength, weaponAction.weapon.currentAttackSpeed * 0.5f, Ease.Linear, 2, CycleMode.Rewind).OnComplete(this, x =>
            {
                canSwing = true;
                Destroy(currentProjectile.gameObject);
                currentProjectile = null;
            });

            InvokeAction(directionOffset);
        }
    }

    public override void UnpackButton()
    {
    }

    public void Unequip()
    {
        if (currentProjectile != null)
        {
            Destroy(currentProjectile.gameObject);
            currentProjectile = null;
            canSwing = true;
        }
    }

    public override void AfterLevelSet()
    {
        lungeLength = lungeLength * (1 + level / 0.1f);
    }

    public override ModuleData GetData()
    {
        ModuleData data = new ModuleData();
        data.className = className;
        data.level = level;

        return data;
    }

    public override void SetData(ModuleData data)
    {
        level = data.level;
    }
}
