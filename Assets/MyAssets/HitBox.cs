using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HitBox : MonoBehaviour
{
    public Health health;

    public float resistance;

    public AudioSource hitSource;

    private void Start()
    {
        hitSource = GetComponent<AudioSource>();
    }

    public void Damage(int damage, float mult, Vector3 position)
    {
        health.Damage(Mathf.RoundToInt(damage * (1 - resistance)), mult, position);
        hitSource.PlayDelayed(Random.Range(0, 0.05f));
    }    
}
