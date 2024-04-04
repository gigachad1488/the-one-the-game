using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public GameObject boss;

    private IBossDamage bossDamage;

    public float damageCd = 0.5f;
    private bool canDamage = true;

    public float mult = 1f;
    public float force = 5f;

    private void Start()
    {
        bossDamage = boss.GetComponent<IBossDamage>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canDamage && collision.TryGetComponent<PlayerHitbox>(out PlayerHitbox player))
        {
            float side;

            if (transform.position.x - player.transform.position.x >= 0) 
            {
                side = -1;
            }
            else
            {
                side = 1;
            }
            player.DamageWithForce(Convert.ToInt32(bossDamage.baseDamage * mult), side * force);
            Invoke(nameof(DamageCd), damageCd);
        }
    }

    public void DamageCd()
    {
        canDamage = true;
    }
}
