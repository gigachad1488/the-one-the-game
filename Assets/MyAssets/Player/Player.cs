using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Collider2D hitboxCollider;
    public Camera camera;

    private void Awake()
    {
        defaultColor = spriteRenderer.color;
        damagedColor = new Color(damagedColor.r, defaultColor.g, defaultColor.b, 0.5f);

        health = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement>();

        health.OnDamage += OnDamage;
        health.OnDeath += Health_OnDeath;
    }

    private void Health_OnDeath()
    {
        SceneManager.LoadScene("end");
    }

    private void OnDamage(float amount, float mult, Vector3 position)
    {
        spriteRenderer.color = defaultColor;
        hitboxCollider.enabled = false;
        Tween.Color(spriteRenderer, damagedColor, health.iFrameTime * 0.5f, Ease.OutExpo, 2, CycleMode.Rewind).OnComplete(this, x => hitboxCollider.enabled = true);     
    }

    private void ReturnColor()
    {
        spriteRenderer.color = defaultColor;
        hitboxCollider.enabled = true;
    }   
}
