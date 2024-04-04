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
                    CreateShockWave(1, cubeBoss.mult * cubeBoss.mult);
                }
                else
                {
                    CreateShockWave(-1, cubeBoss.mult * cubeBoss.mult);
                }
            }
        }
    }

    public void CreateShockWave(int side)
    {
        RaycastHit2D hit = Physics2D.Raycast(cubeBoss.transform.position, -Vector2.up, Mathf.Abs(cubeBoss.transform.position.y - collider.transform.position.y) + 6f, cubeBoss.groundLayer);
        ShockWaveAttack attack = Instantiate(cubeBoss.shockWaveAttackPrefab, new Vector3(transform.position.x - (5 * side), hit.collider.bounds.center.y + (hit.collider.bounds.size.y * 0.5f), transform.position.z), Quaternion.identity);
        attack.cubeBoss = cubeBoss;
        attack.side = side;
    }

    public void CreateShockWave(int side, float scale)
    {
        RaycastHit2D hit = Physics2D.Raycast(cubeBoss.transform.position, -Vector2.up, Mathf.Abs(cubeBoss.transform.position.y - collider.transform.position.y) + 6f, cubeBoss.groundLayer);
        ShockWaveAttack attack = Instantiate(cubeBoss.shockWaveAttackPrefab, new Vector3(transform.position.x - (5 * side), hit.collider.bounds.center.y + (hit.collider.bounds.size.y * 0.5f), transform.position.z), Quaternion.identity);
        attack.cubeBoss = cubeBoss;
        attack.side = side;
        attack.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isStaying = false;
    }
}
