using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Space(5)]
    [Header("Health")]
    public int maxHealth = 2000;
    [SerializeField]
    private int currenthealth;
    public int currentHealth
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
            else
            {
                currenthealth = value;
            }
        }
    }

    public delegate void DeathDelegate();
    public event DeathDelegate? OnDeath;

    [Space(5)]
    [Header("Damage")]   
    public float iFrameTime = 0.2f;
    public float iFrameTimer;

    public delegate void DamageDelegate(float amount, float mult, Vector3 position);
    public event DamageDelegate? OnDamage;

    [Space(5)]
    [Header("Regen")]
    public int regenAmount = 10;
    [SerializeField]
    private float regenTime = 5f;
    private float regenTimer;

    public delegate void HealDelegate(float amount);
    public event HealDelegate? OnHeal;

    private void Awake()
    {
        currentHealth = maxHealth;
        iFrameTimer = 0;
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

    public void Damage(int damage, float mult, Vector3 position)
    {
        if (iFrameTimer <= 0)
        {
            ResetIFrame();
            currentHealth -= Mathf.RoundToInt(damage);

            if (currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }

            OnDamage?.Invoke(damage, mult, position);
        }
    }

    public void Heal(int heal)
    {
        if (heal > 0 && currentHealth < maxHealth && currentHealth > 0)
        {
            currentHealth += heal;
            OnHeal?.Invoke(heal);
        }
    }

    public void ResetIFrame()
    {
        iFrameTimer = iFrameTime;
    }

    public void ResetIFrame(float time)
    {
        iFrameTimer = time;
    }

}
