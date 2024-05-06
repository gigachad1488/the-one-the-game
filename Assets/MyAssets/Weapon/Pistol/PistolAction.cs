using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAction : WeaponAction
{
    [HideInInspector]
    private Camera cam;

    private float time = 0.5f;
    private float timer;
    private float timerTickTime = 0.1f;
    private WaitForSeconds timerTick;

    private GameObject weaponModelObject;

    public override void AfterSet()
    {
        weaponModel = new GameObject("weapon model");
        weaponModel.transform.SetParent(weapon.transform);
        weaponModel.transform.localPosition = Vector3.zero;
     
        weaponModelObject = Instantiate(weapon.weaponModelPrefab, weaponModel.transform);
        weaponModelObject.transform.localPosition += new Vector3(0.8f, 0, 0);

        weaponModel.SetActive(false);
        cam = weapon.player.camera;
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

        weapon.player.armSolver.weight = 1;
        weapon.player.armSolverTarget.position = weaponModelObject.transform.position;

        weapon.weaponShoot.Shoot(direction);

        if (timer <= 0)
        {
            timer = time;
            StartCoroutine(DisapearTimer());
        }
        else
        {
            timer = time;
        }

        InvokeAction(direction);       
    }

    private IEnumerator DisapearTimer()
    {
        while (timer > 0)
        {
            timer -= timerTickTime;
            yield return timerTick;
        }

        weapon.player.armSolver.weight = 0f;

        weaponModel.SetActive(false);
    }


    public override ModuleDataType GetData()
    {
        ModuleDataType data = new ModuleDataType();
        data.data.className = className;
        data.data.level = level;

        return data;
    }

    public override void SetData(ModuleDataType data)
    {
        level = data.data.level; 
    }
}
