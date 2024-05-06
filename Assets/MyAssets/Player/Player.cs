using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent (typeof(Health))]
public class Player : MonoBehaviour
{
    [Space(5)]
    [Header("Sprite")]
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    private Color defaultColor;
    private Color damagedColor;

    public PlayerMovement playerMovement;
    public Health health;
    public Collider2D hitboxCollider;
    public Camera camera;

    public LimbSolver2D armSolver;
    public Transform armSolverTarget;

    [Space(5)]
    [Header("Base Stats")]
    public int maxHealth = 100;
    public int healthLevelScale = 20;
    [Space(3)]
    public int regenAmount = 10;
    public int regenAmountLevelScale = 2;
    [Space(3)]
    public float moveSpeed = 25f;
    public float moveSpeedLevelScale = 2f;
    [Space(3)]
    public float flyingForce = 15f;
    public float flyingForceLevelScale = 1f;
    [Space(3)]
    public float flightTime = 1f;
    public float flightTimeLevelScale = 0.2f;
    [Space(3)]
    public float dashForce = 30f;
    public float dashForceLevelScale = 2f;

    private void Awake()
    {
        defaultColor = spriteRenderers[0].color;
        damagedColor = new Color(damagedColor.r, defaultColor.g, defaultColor.b, 0.5f);

        health = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement>();

        health.OnDamage += OnDamage;
        health.OnDeath += Health_OnDeath;

        armSolver.weight = 0;
    }

    public void SetHealthLevel(int level)
    {
        health.maxHealth = maxHealth + (healthLevelScale * level);
        health.currentHealth = maxHealth;
    }

    public void SetRegenAmountLevel(int level)
    {
        health.regenAmount = regenAmount + (regenAmountLevelScale * level);
    }    

    public void SetMoveSpeedLevel(int level)
    {
        playerMovement.moveSpeed = moveSpeed + (moveSpeedLevelScale * level);
        playerMovement.maxXSpeed = playerMovement.moveSpeed / 5;
    }

    public void SetFlightTimeLevel(int level)
    {
        playerMovement.flyingTime = flightTime + (flightTimeLevelScale * level);
    }

    public void SetFlightForceLevel(int level)
    {
        playerMovement.flyingForce = flyingForce + (flyingForceLevelScale * level);
    }

    public void SetDashForceLevel(int level)
    {
        playerMovement.dashForce = dashForce + (dashForceLevelScale * level);
    }

    private void Health_OnDeath()
    {
        SceneManager.LoadScene("end");
    }

    private void OnDamage(float amount, float mult, Vector3 position)
    {
        spriteRenderers[0].color = damagedColor;
        spriteRenderers[1].color = damagedColor;
        spriteRenderers[2].color = damagedColor;
        spriteRenderers[3].color = damagedColor;
        spriteRenderers[4].color = damagedColor;
        spriteRenderers[5].color = damagedColor;

        hitboxCollider.enabled = false;
        Invoke(nameof(ReturnColor), health.iFrameTime);
        //Tween.Color(spriteRenderer, damagedColor, health.iFrameTime * 0.5f, Ease.OutExpo, 2, CycleMode.Rewind).OnComplete(this, x => hitboxCollider.enabled = true);     
    }

    private void ReturnColor()
    {
        spriteRenderers[0].color = defaultColor;
        spriteRenderers[1].color = defaultColor;
        spriteRenderers[2].color = defaultColor;
        spriteRenderers[3].color = defaultColor;
        spriteRenderers[4].color = defaultColor;
        spriteRenderers[5].color = defaultColor;

        hitboxCollider.enabled = true;
    }   
}
