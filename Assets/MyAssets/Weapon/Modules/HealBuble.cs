using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuble : MonoBehaviour
{
    public int healAmount = 10;

    public float speed = 1;

    public Player player;

    private WaitForEndOfFrame wfs = new WaitForEndOfFrame();

    private void Start()
    {
        StartCoroutine(MoveToPlayer());
    }

    public IEnumerator MoveToPlayer()
    {
        while ((player.transform.position - transform.position).sqrMagnitude > 0.5f) 
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);

            yield return wfs;
        }

        player.health.Heal(healAmount);

        Destroy(gameObject);
    }
}
