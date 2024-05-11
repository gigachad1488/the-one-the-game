using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBoss : MonoBehaviour, IBoss, IBossDamage
{
    public StateMachine stateMachine;

    public Health health;

    [HideInInspector]
    public List<BaseState> attackStates = new List<BaseState>();

    public Rigidbody2D rb;

    public Player aggroedPlayer {get; set;}

    public float mult = 1;

    public CubeBordersFallAttack[] borderColliders;
    public Thruster[] thrusters;

    public AudioSource musicSource;

    public float firstPhaseMult = 1.2f;
    public float secondPhaseMult = 1.8f;
    public float thirdPhaseMult = 2.2f;

    public LayerMask groundLayer;
    public int damage = 50;
    [SerializeField]
    public int baseDamage { get; set; } = 50;

    [SerializeField]
    public float difficultyMult { get; set; } = 1.5f;

    [Space(5)]
    [Header("Attacks")]  
    public ShockWaveAttack shockWaveAttackPrefab;

    [Space(5)]
    [Header("Death")]
    public GameObject deathParticles;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnDamage += Phases;
        health.OnDeath += Health_OnDeath;
    }

    private void Health_OnDeath()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        foreach (var item in borderColliders)
        {
            item.canCollisionAttack = false;
        }

        health.maxHealth = System.Convert.ToInt32(health.maxHealth * difficultyMult);
        health.currentHealth = health.maxHealth;

        GetComponent<HealthBar>().UpdateHealthBar();

        yield return new WaitForSeconds(2);      
        attackStates.Add(new CubeRollState(stateMachine, this));
        attackStates.Add(new CubeJumpState(stateMachine, this));
        attackStates.Add(new CubeDashState(stateMachine, this));
        stateMachine.SetState(attackStates[0]);

        firstPhaseMult *= difficultyMult;
        secondPhaseMult *= difficultyMult;
        thirdPhaseMult *= difficultyMult;       

        mult = firstPhaseMult;
        baseDamage = System.Convert.ToInt32(baseDamage * difficultyMult);

    }

    private void Update()
    {
        stateMachine.currentState?.OnUpdate();
    }

    public void ChangeRandomAttackState()
    {
        float distance = (transform.position - aggroedPlayer.transform.position).magnitude;

        if (distance >= 55f)
        {
            stateMachine.SetState(attackStates[1]);
        }
        else
        {
            stateMachine.SetState(attackStates[Random.Range(0, attackStates.Count)]);
        }
    }

    private void Phases(float amount, float mult, Vector3 position)
    {
        if (this.mult < secondPhaseMult && health.currentHealth <= health.maxHealth * 0.6f)
        {
            this.mult = secondPhaseMult;
        }
        else if (this.mult >= secondPhaseMult && this.mult < thirdPhaseMult &&  health.currentHealth <= health.maxHealth * 0.3f)
        {
            this.mult = thirdPhaseMult;
        }
    }
}
