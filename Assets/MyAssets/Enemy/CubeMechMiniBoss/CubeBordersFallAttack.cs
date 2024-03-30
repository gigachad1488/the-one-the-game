using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBordersFallAttack : MonoBehaviour
{
    public CubeBoss cubeBoss;
    public bool isStaying = false;

    private void Start()
    {
        cubeBoss = GetComponentInParent<CubeBoss>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isStaying)
        {
            isStaying = true;
            ShockWaveAttack attack = Instantiate(cubeBoss.shockWaveAttackPrefab, transform.position - Vector3.up * 0.2f, Quaternion.identity);
            attack.cubeBoss = cubeBoss;

            if (cubeBoss.rb.velocityX > 0)
            {
                attack.side = 1;
            }
            else
            {
                attack.side = -1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isStaying = false;
    }
}
