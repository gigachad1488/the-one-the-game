using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShoot : WeaponShoot
{
    private bool canSwing = true;
    public override void AfterSet()
    {
    }

    public override void Shoot(Vector2 directionOffset)
    {
        if (canSwing)
        {
            canSwing = false;
            Projectile pr = Instantiate(projectile, weaponAction.weapon.transform);
            float playerRot = weaponAction.weapon.player.playerMovement.sprite.transform.localScale.x;
            pr.Set(this);
            Tween.LocalEulerAngles(pr.transform, Vector3.zero, new Vector3(0, 0, -360 * playerRot), 0.2f, Ease.Linear).OnComplete(this, x =>
            {
                canSwing = true;
                Destroy(pr.gameObject);
            });

            InvokeAction(directionOffset);
        }
    }

    public override void UnpackButton()
    {
    }
}
