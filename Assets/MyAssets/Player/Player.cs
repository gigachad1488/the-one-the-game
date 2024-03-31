using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent (typeof(Health))]
public class Player : MonoBehaviour
{
    [Space(5)]
    [Header("Sprite")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    private Color damagedColor;

    public PlayerMovement playerMovement;
    public Health health;

    [Space(5)]
    [Header("Heath Canvas")]
    public Canvas healthCanvas;
    public Image healthBarFilling;
    public TextMeshProUGUI healthText;

    private void Awake()
    {
        defaultColor = spriteRenderer.color;
        damagedColor = new Color(damagedColor.r, defaultColor.g, defaultColor.b, 0.5f);

        health = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement>();

        health.OnDamage += OnDamage;
        health.OnHeal += UpdateHealthBar;

        
    }

    private void Start()
    {
        UpdateHealthBar(0);
    }

    private void OnDamage(float amount, float mult, Vector3 position)
    {
        spriteRenderer.color = defaultColor;
        Tween.Color(spriteRenderer, damagedColor, health.iFrameTime * 0.5f, Ease.OutExpo, 2, CycleMode.Rewind);
        UpdateHealthBar(amount);
    }

    private void UpdateHealthBar(float amount)
    {
        float hpPercent = (float)health.currentHealth / health.maxHealth;
        healthBarFilling.fillAmount = hpPercent;
        healthText.text = health.currentHealth.ToString();
    }

    private void ReturnColor()
    {
        spriteRenderer.color = defaultColor;
    }
}
