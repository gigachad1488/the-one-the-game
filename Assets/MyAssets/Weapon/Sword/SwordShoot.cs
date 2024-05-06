using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShoot : WeaponShoot
{
    private bool canSwing = true;

    private Projectile currentProjectile;

    public override void AfterSet()
    {
        weaponAction.weapon.OnUnequip += Unequip;
    }

    public override void Shoot(Vector2 directionOffset)
    {
        if (canSwing)
        {
            canSwing = false;

            GameObject swingOffset = new GameObject("swingsword");
            swingOffset.transform.SetParent(weaponAction.weapon.transform);
            swingOffset.transform.localPosition = Vector3.zero;

            currentProjectile = Instantiate(projectile, swingOffset.transform);
            currentProjectile.transform.localPosition += new Vector3(0, 0.65f, 0);

            float playerRot = 1;

            if (directionOffset.x >= 0)
            {
                playerRot = 1;
            }
            else
            {
                playerRot = -1;
            }

            currentProjectile.gameObject.SetActive(true);
            currentProjectile.Set(this);

            weaponAction.weapon.player.armSolver.weight = 1;

            Tween.LocalEulerAngles(swingOffset.transform, Vector3.zero, new Vector3(0, 0, -360 * playerRot), new TweenSettings(weaponAction.weapon.currentAttackSpeed, Ease.Linear, useFixedUpdate: true)).OnUpdate(swingOffset.transform, (x, y) => weaponAction.weapon.player.armSolverTarget.position = currentProjectile.transform.position).OnComplete(this, x =>
            {
                weaponAction.weapon.player.armSolver.weight = 0;
                canSwing = true;
                Destroy(swingOffset);
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
    }

    public override ModuleDataType GetData()
    {
        ModuleData data = new ModuleData();
        data.className = className;
        data.level = level;

        ModuleDataType type = new ModuleDataType();
        type.data = data;

        return type;
    }

    public override void SetData(ModuleDataType data)
    {
        level = data.data.level;
    }
}
