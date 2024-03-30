using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;

    public float resistance;

    public void Damage(int damage)
    {

        health.Damage(Mathf.RoundToInt(damage * (1 - resistance)));
    }    
}
