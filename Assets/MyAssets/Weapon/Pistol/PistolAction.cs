using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAction : WeaponAction
{
    [HideInInspector]
    public Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public override void AfterSet()
    {
        weaponModel = Instantiate(weapon.weaponModelPrefab, weapon.transform);
        weaponModel.SetActive(false);
    }

    public override void Action()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position;
        weaponModel.SetActive(true);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponModel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (angle > 90 || angle < -90)
        {
            weaponModel.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            weaponModel.transform.localScale = new Vector3(1, 1, 1);
        }

        weapon.weaponShoot.Shoot(direction);

        InvokeAction(direction);       
    }   
}
