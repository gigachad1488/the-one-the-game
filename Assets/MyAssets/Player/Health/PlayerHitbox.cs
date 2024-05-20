using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public Player player;

    public Vector2 knockBack = new Vector2(1, 1.5f);
    public void DamageWithForce(int amount, float force)
    {
        if (player.health.iFrameTimer <= 0)
        {
            player.health.Damage(amount, 1, transform.position);
            player.playerMovement.rb.AddForce(new Vector2(knockBack.x * force, knockBack.y * Mathf.Abs(force)), ForceMode2D.Impulse);
        }
    }

    public void DamageWithForce(int amount, float force, Vector3 attackerPoint)
    {
        if (player.health.iFrameTimer <= 0)
        {
            player.health.Damage(amount, 1, transform.position);

            if (attackerPoint.x - transform.position.x >= 0)
            {
                force *= -1;
            }

            player.playerMovement.rb.AddForce(new Vector2(knockBack.x * force, knockBack.y * Mathf.Abs(force)), ForceMode2D.Impulse);
        }
    }
}
