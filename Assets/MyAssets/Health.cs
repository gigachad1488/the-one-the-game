using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Space(5)]
    [Header("Health")]
    [SerializeField]
    private int maxHealth = 2000;
    private int currenthealth;
    private int currentHealth
    {
        get
        {
            return currenthealth;
        }
        set
        {
            if (value < maxHealth) 
            {
                currenthealth = value;
            }
            else if (value > maxHealth) 
            {
                currenthealth = maxHealth;
            }
        }
    }

    public delegate void DeathDelegate();
    public event DeathDelegate OnDeath;

    [Space(5)]
    [Header("Damage")]
    [SerializeField]
    private float iFrameTime = 0.2f;
    private float iFrameTimer;

    public delegate void DamageDelegate();
    public event DamageDelegate OnDamage;


    [Space(5)]
    [Header("Resistance")]
    [SerializeField]
    private float resistance = 0.1f;

    [Space(5)]
    [Header("Regen")]
    [SerializeField]
    private int regenAmount = 10;
    [SerializeField]
    private float regenTime = 5f;
    private float regenTimer;

    private void Start()
    {
        currentHealth = maxHealth;
        iFrameTimer = iFrameTime;
        regenTimer = regenTime;
    }

    private void Update()
    {
        iFrameTimer -= Time.deltaTime;
        regenTimer -= Time.deltaTime;

        if (regenTimer < 0)
        {
            Heal(regenAmount);
            regenTimer = regenTime;
        }
    }

    public void Damage(int damage)
    {
        if (iFrameTimer > 0)
        {
            ResetIFrame();
            currentHealth -= Mathf.RoundToInt(damage * (1 - resistance));

            if (currentHealth <= 0)
            {
                OnDeath.Invoke();
                return;
            }

            OnDamage.Invoke();
        }
    }

    public void Heal(int heal)
    {
        if (heal > 0 && currentHealth < maxHealth && currentHealth > 0)
        {
            currentHealth += heal;
        }
    }

    public void ResetIFrame()
    {
        iFrameTimer = iFrameTime;
    }

}
