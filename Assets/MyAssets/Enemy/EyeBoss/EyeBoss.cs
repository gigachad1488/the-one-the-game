using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EyeBoss : MonoBehaviour, IBoss, IBossDamage
{
    public StateMachine stateMachine;

    public Health health;

    [HideInInspector]
    public List<BaseState> attackStates = new List<BaseState>();

    public Rigidbody2D rb;

    [SerializeField]
    public Player aggroedPlayer { get; set; }

    public float mult = 1;

    public float firstPhaseMult = 0.8f;
    public float secondPhaseMult = 0.9f;
    public float thirdPhaseMult = 1f;

    public LayerMask groundLayer;
    public int damage { get; set; } = 50;

    [SerializeField]
    public int baseDamage { get; set; } = 50;

    [SerializeField]
    public float difficultyMult { get; set; } = 1.5f;

    [Header("Attacks")]
    public EyeBossLaser laserProjectilePrefab;
    public Transform shootPoint;

    public ParticleSystem beamParticles;

    public GameObject beamPrefab;

    [Space(5)]
    [Header("Death")]
    public GameObject deathParticles;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnDamage += Phases;
        health.OnDeath += Health_OnDeath;

        //aggroedPlayer = GameObject.FindGameObjectWithTag("aboba").GetComponent<Player>();     
    }

    private void Health_OnDeath()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        health.maxHealth = System.Convert.ToInt32(health.maxHealth * difficultyMult);
        health.currentHealth = health.maxHealth;

        GetComponent<HealthBar>().UpdateHealthBar();

        yield return new WaitForSeconds(0.5f);

        attackStates.Add(new EyeFlightState(stateMachine, this));
        attackStates.Add(new EyeBossLaserState(stateMachine, this));       
        attackStates.Add(new EyeDashState(stateMachine, this));
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

    public void ChangeToFlight()
    {
        stateMachine.SetState(attackStates[0]);
    }

    public void ChangeRandomAttackState()
    {
        stateMachine.SetState(attackStates[Random.Range(1, attackStates.Count)]);
    }

    private void Phases(float amount, float mult, Vector3 position)
    {
        if (this.mult < secondPhaseMult && health.currentHealth <= health.maxHealth * 0.6f)
        {
            this.mult = secondPhaseMult;
        }
        else if (this.mult >= secondPhaseMult && this.mult < thirdPhaseMult && health.currentHealth <= health.maxHealth * 0.3f)
        {
            this.mult = thirdPhaseMult;
        }
    }
}
