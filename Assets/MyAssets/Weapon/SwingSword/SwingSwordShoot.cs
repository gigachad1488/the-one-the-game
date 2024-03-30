using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSwordShoot : WeaponShoot
{
    private bool canSwing = true;
    private bool fromUp = true;
    public float attackSpeed = 0.6f;
    public Ease swingEase = Ease.OutExpo;

    private Projectile currentProjectile;

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
            Vector3 up = new Vector3(0, 0, angle + 30);
            Vector3 down = new Vector3(0, 0, angle - 30 - 180);

            if (fromUp)
            {
                currentProjectile.transform.localScale = Vector3.one;
                Tween.LocalEulerAngles(currentProjectile.transform, up, down, attackSpeed, swingEase).OnComplete(this, x =>
                {
                    canSwing = true;
                    Destroy(currentProjectile.gameObject);
                });
                fromUp = false;
            }
            else
            {
                currentProjectile.transform.localScale = new Vector3(-1, 1, 1);
                Tween.LocalEulerAngles(currentProjectile.transform, down, up, attackSpeed, swingEase).OnComplete(this, x =>
                {
                    canSwing = true;
                    Destroy(currentProjectile.gameObject);
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
}
