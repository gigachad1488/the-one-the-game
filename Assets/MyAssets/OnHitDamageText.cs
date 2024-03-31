using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class OnHitDamageText : MonoBehaviour
{
    public Health health;

    [SerializeField]
    private DamageText damageTextPrefab;

    private void Start()
    {
        health = GetComponent<Health>();
        health.OnDamage += CreateDamageText;
    }

    public void CreateDamageText(float damage, float mult, Vector3 position)
    {
        DamageText damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        damageText.damage = damage;
        damageText.mult = mult;
    }
}
