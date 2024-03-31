using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBordersFallAttack : MonoBehaviour
{
    public CubeBoss cubeBoss;
    public bool isStaying = false;
    public new Collider2D collider;
    public bool canCollisionAttack = true;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        cubeBoss = GetComponentInParent<CubeBoss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isStaying)
        {
            isStaying = true;
            if (canCollisionAttack)
            {
                if (cubeBoss.rb.velocityX > 0)
                {
                    CreateShockWave(1);
                }
                else
                {
                    CreateShockWave(-1);
                }
            }
        }
    }

    public void CreateShockWave(int side)
    {
        ShockWaveAttack attack = Instantiate(cubeBoss.shockWaveAttackPrefab, transform.position - Vector3.up * 0.2f, Quaternion.identity);
        attack.cubeBoss = cubeBoss;
        attack.side = side;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isStaying = false;
    }
}
