using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveAttack : MonoBehaviour
{
    public CubeBoss cubeBoss;

    public float side = 1f;
    public float speed = 40f;
    public float endSpeedMult = 0.25f;
    public float duration = 2f;

    public ContactDamage damage;

    private Rigidbody2D rb;

    public ParticleSystem[] particles;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        damage.boss = cubeBoss.gameObject;

        if (side > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }

        foreach (var particle in particles)
        {
            particle.Play();
        }

        Tween.Custom(speed, speed * endSpeedMult, duration, onValueChange: x => rb.velocityX = side * x * cubeBoss.mult, Ease.OutExpo).OnComplete(this, x => x.Destroy());
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
