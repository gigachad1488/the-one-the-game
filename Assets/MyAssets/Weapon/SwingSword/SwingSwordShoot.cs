using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSwordShoot : WeaponShoot
{
    private bool canSwing = true;
    private bool fromUp = true;
    public Ease swingEase = Ease.OutCubic;

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
            currentProjectile = Instantiate(projectile, weaponAction.weapon.transform.position + (Vector3)directionOffset.normalized * 0.5f, Quaternion.identity, weaponAction.weapon.transform);
            canSwing = false;
            float angle = Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg;
            Vector3 up = new Vector3(0, 0, angle + attackAngle);
            Vector3 down = new Vector3(0, 0, angle - attackAngle - 180);

            if (fromUp)
            {
                currentProjectile.transform.localScale = new Vector3(currentProjectile.transform.localScale.x, currentProjectile.transform.localScale.y, currentProjectile.transform.localScale.z);
                Tween.LocalEulerAngles(currentProjectile.transform, up, down, weaponAction.weapon.currentAttackSpeed, swingEase).OnComplete(this, x =>
                {
                    canSwing = true;
                    Destroy(currentProjectile.gameObject);
                });
                fromUp = false;
            }
            else
            {
                currentProjectile.transform.localScale = new Vector3(-currentProjectile.transform.localScale.x, currentProjectile.transform.localScale.y, currentProjectile.transform.localScale.z);
                Tween.LocalEulerAngles(currentProjectile.transform, down, up, weaponAction.weapon.currentAttackSpeed, swingEase).OnComplete(this, x =>
                {
                    canSwing = true;
                    Destroy(currentProjectile.gameObject);
                });
                fromUp = true;
            }

            projectile.Set(this);

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

    public override void AfterLevelSet(int level)
    {
        attackAngle = attackAngle * (1 + level * 0.15f);
    }
}
