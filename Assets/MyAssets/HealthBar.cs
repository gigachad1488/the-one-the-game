using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBar : MonoBehaviour
{
    public Health health;
    public Canvas healthCanvas;
    public Image healthBarFilling;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        health = GetComponent<Health>();
        health.OnDamage += OnDamage;
        health.OnHeal += OnHeal;
        UpdateHealthBar();
    }

    private void OnHeal(float amount)
    {
        UpdateHealthBar();
    }

    private void OnDamage(float amount, float mult, Vector3 position)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float hpPercent = (float)health.currentHealth / health.maxHealth;
        healthBarFilling.fillAmount = hpPercent;
        healthText.text = health.currentHealth.ToString();
    }
}
