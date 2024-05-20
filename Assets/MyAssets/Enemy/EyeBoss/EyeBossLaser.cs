using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EyeBossLaser : MonoBehaviour
{
    public Rigidbody2D rb;

    private IBossDamage damage;
    private float mult;

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    public void Init(IBossDamage damage, float speed, float mult = 0.5f)
    {
        rb.velocity = transform.right * speed;
        this.damage = damage;
        this.mult = mult;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHitbox>(out PlayerHitbox health))
        {
            health.DamageWithForce(Mathf.FloorToInt(damage.damage * mult), 10f, transform.position);
        }
    }
}

