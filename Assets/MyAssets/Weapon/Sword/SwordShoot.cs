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
            currentProjectile = Instantiate(projectile, weaponAction.weapon.transform);
            float playerRot = weaponAction.weapon.player.playerMovement.sprite.transform.localScale.x;
            currentProjectile.Set(this);
            Tween.LocalEulerAngles(currentProjectile.transform, Vector3.zero, new Vector3(0, 0, -360 * playerRot), 0.2f, Ease.Linear).OnComplete(this, x =>
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
}
