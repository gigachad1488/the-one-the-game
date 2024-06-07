using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent (typeof(Health))]
public class Player : MonoBehaviour
{
    [Header("Settings Menu")]
    [SerializeField]
    private Canvas settingsCanvas;
    private CanvasGroup settingCanvasGroup;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button toMenuButton;

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
    public PlayerUpgradesData playerData;

    private void Awake()
    {
        defaultColor = spriteRenderers[0].color;
        damagedColor = new Color(damagedColor.r, defaultColor.g, defaultColor.b, 0.5f);

        health = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement>();

        health.OnDamage += OnDamage;
        health.OnDeath += Health_OnDeath;

        armSolver.weight = 0;

        SetHealthLevel();
        SetMoveSpeedLevel();
        SetFlightForceLevel();
        SetFlightTimeLevel();
        SetDashCdLevel();
        SetDashForceLevel();

        playerMovement.inputManager.settingsAction.performed += SettingsSwitch;
        closeButton.onClick.AddListener(() => SettingsSwitch(false));
        toMenuButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        settingCanvasGroup = settingsCanvas.GetComponent<CanvasGroup>();
        settingsCanvas.gameObject.SetActive(true);
        SettingsSwitch(false);
    }

    private void SettingsSwitch(InputAction.CallbackContext context)
    {
        SettingsSwitch(!settingCanvasGroup.blocksRaycasts);
    }

    private void SettingsSwitch(bool open)
    {
        if (open)
        {
            Time.timeScale = 0;
            settingCanvasGroup.alpha = 1;
            settingCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            Time.timeScale = 1;
            settingCanvasGroup.alpha = 0;
            settingCanvasGroup.blocksRaycasts = false;
        }
    }

    private void SetHealthLevel()
    {
        health.maxHealth = playerData.baseHp + (playerData.hpLevel * playerData.baseHpScale);
        health.currentHealth = health.maxHealth;
    }

    /*
    public void SetRegenAmountLevel(int level)
    {
        health.regenAmount = regenAmount + (regenAmountLevelScale * level);
    }    
    */

    private void SetMoveSpeedLevel()
    {
        playerMovement.moveSpeed = playerData.baseMoveSpeed + (playerData.moveSpeedLevel * playerData.baseMoveSpeedScale);
        playerMovement.maxXSpeed = playerMovement.moveSpeed / 5;
    }

    private void SetFlightTimeLevel()
    {
        playerMovement.flyingTime = playerData.baseFlyTime + (playerData.flyTimeLevel * playerData.baseFlyTimeScale);
    }

    private void SetFlightForceLevel()
    {
        playerMovement.flyingForce = playerData.baseFlyForce + (playerData.flyForceLevel * playerData.baseFlyForceScale);
        playerMovement.maxYSpeed = playerMovement.flyingForce;
        playerMovement.downForce = -playerMovement.flyingForce * 0.3f;
    }

    private void SetDashForceLevel()
    {
        playerMovement.dashForce = playerData.baseDashForce + (playerData.dashForceLevel * playerData.baseDashForceScale);
    }

    private void SetDashCdLevel()
    {
        playerMovement.dashCD = playerData.baseDashCd - (playerData.dashCdLevel * playerData.baseDashCdScale);
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
