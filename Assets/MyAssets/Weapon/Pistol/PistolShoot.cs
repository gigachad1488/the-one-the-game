using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PistolShoot : WeaponShoot
{
    private Transform shootPoint;

    public float projectileSpeed = 50f;
    public float attackSpeed = 0.5f;
    private float attackTimer;

    private float attackCdTickTime = 0.1f;
    private WaitForSeconds attackCdTick;
    public override void AfterSet()
    {
        attackTimer = 0;
        attackCdTick = new WaitForSeconds(attackCdTickTime);
        shootPoint = weaponAction.weaponModel.GetComponentsInChildren<Transform>().Where(s => s.name == "ShootPoint").First();
    }

    public override void Shoot(Vector2 directionOffset)
    {
        if (attackTimer <= 0)
        {
            attackTimer = attackSpeed;
            StartCoroutine(AttackCd());
            Projectile pr = Instantiate(projectile, shootPoint.position, weaponAction.weaponModel.transform.localRotation);
            pr.Set(this);
            Rigidbody2D rb = pr.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.mass = 0;
            rb.velocity = pr.transform.right * projectileSpeed;
        }
    }

    private IEnumerator AttackCd()
    {
       yield return new WaitForSeconds(attackSpeed);
       attackTimer = 0;
    }

    public override void UnpackButton()
    {
    }
}
