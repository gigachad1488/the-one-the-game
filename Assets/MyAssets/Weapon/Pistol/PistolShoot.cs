using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PistolShoot : WeaponShoot
{
    private Transform shootPoint;

    public float projectileSpeed = 50f;
    private float attackTimer;

    private float attackCdTickTime = 0.1f;
    private WaitForSeconds attackCdTick;

    public override void AfterSet()
    {     
        Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.mass = 0;
        attackTimer = 0;
        attackCdTick = new WaitForSeconds(attackCdTickTime);
        shootPoint = weaponAction.weaponModel.GetComponentsInChildren<Transform>().Where(s => s.name == "ShootPoint").First();
    }

    public override void Shoot(Vector2 directionOffset)
    {
        if (attackTimer <= 0)
        {
            attackTimer = weaponAction.weapon.currentAttackSpeed;
            StartCoroutine(AttackCd());
            Projectile pr = Instantiate(projectile, shootPoint.position, weaponAction.weaponModel.transform.localRotation);
            pr.gameObject.SetActive(true);
            Rigidbody2D rb = pr.GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = pr.transform.right * projectileSpeed;
            pr.Set(this);        
        }
    }

    private IEnumerator AttackCd()
    {
       yield return new WaitForSeconds(weaponAction.weapon.currentAttackSpeed);
       attackTimer = 0;
    }

    public override void UnpackButton()
    {
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
