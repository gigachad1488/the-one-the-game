using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootProjectileModule : ShootModule
{
    public Projectile projectileModel;
    public float projectileSpeed = 10f;
    public override void AfterSet()
    {
    }

    public override void OnShoot(Vector2 directionOffset)
    {
        float angle = Mathf.Atan2(directionOffset.y, directionOffset.x) * Mathf.Rad2Deg;
        Projectile pr = Instantiate(projectileModel, shoot.transform.position, Quaternion.Euler(0, 0, angle));              
        Rigidbody2D rb = pr.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.mass = 0;
        rb.velocity = pr.transform.right * projectileSpeed;
        pr.Set(shoot);
    }
}
