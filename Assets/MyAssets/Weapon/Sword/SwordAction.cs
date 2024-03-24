using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : WeaponAction
{
    private Camera cam;
    public override void AfterSet()
    {
        cam = Camera.main;
    }

    public override void Action()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //weaponModel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        weapon.weaponShoot.Shoot(direction);

        InvokeAction(direction);
    }   
}
