using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSwordShoot : WeaponShoot
{
    private bool canSwing = true;
    private bool fromUp = true;
    public Ease swingEase = Ease.OutSine;

    private Projectile currentProjectile;

    private float attackAngle = 20;

    public override void AfterSet()
    {
        weaponAction.weapon.OnUnequip += Unequip;
    }

    public override void Shoot(Vector2 directionOffset)
    {
        if (canSwing) 
        {
            GameObject swingOffset = new GameObject("swingsword");
            swingOffset.transform.SetParent(weaponAction.weapon.transform);
            swingOffset.transform.position = weaponAction.weapon.transform.position;            

            currentProjectile = Instantiate(projectile, swingOffset.transform);
            currentProjectile.transform.position += new Vector3(0, 1.5f, 0);
            currentProjectile.gameObject.SetActive(true);
            currentProjectile.Set(this);           
            canSwing = false;
            float angle = Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg;
            Vector3 up = new Vector3(0, 0, angle + attackAngle);
            Vector3 down = new Vector3(0, 0, angle - attackAngle - 180);

            weaponAction.weapon.player.armSolver.weight = 1;

            if (fromUp)
            {
                swingOffset.transform.localScale = new Vector3(swingOffset.transform.localScale.x, swingOffset.transform.localScale.y, swingOffset.transform.localScale.z);
                Tween.LocalEulerAngles(swingOffset.transform, up, down, new TweenSettings(weaponAction.weapon.currentAttackSpeed, swingEase, useFixedUpdate: false)).OnUpdate(swingOffset.transform, (x, y) => weaponAction.weapon.player.armSolverTarget.position = currentProjectile.transform.position).OnComplete(this, x =>
                {
                    weaponAction.weapon.player.armSolver.weight = 0;
                    canSwing = true;
                    Destroy(swingOffset);
                });
                fromUp = false;
            }
            else
            {
                swingOffset.transform.localScale = new Vector3(-swingOffset.transform.localScale.x, swingOffset.transform.localScale.y, swingOffset.transform.localScale.z);
                Tween.LocalEulerAngles(swingOffset.transform, down, up, new TweenSettings(weaponAction.weapon.currentAttackSpeed, swingEase, useFixedUpdate: false)).OnUpdate(swingOffset.transform, (x, y) => weaponAction.weapon.player.armSolverTarget.position = currentProjectile.transform.position).OnComplete(this, x =>
                {
                    weaponAction.weapon.player.armSolver.weight = 0;
                    canSwing = true;
                    Destroy(swingOffset);
                });
                fromUp = true;
            }

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
        attackAngle = attackAngle * (1 + level * 0.15f);
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
