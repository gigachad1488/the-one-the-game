using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBoss : MonoBehaviour
{
    public StateMachine stateMachine;

    public Health health;

    [HideInInspector]
    public List<BaseState> attackStates = new List<BaseState>();

    public Rigidbody2D rb;

    public Player aggroedPlayer;

    public float mult = 1.2f;

    public CubeBordersFallAttack[] borderColliders;

    public AudioSource musicSource;

    public float firstPhaseMult = 1.2f;
    public float secondPhaseMult = 1.8f;
    public float thirdPhaseMult = 2.2f;

    public LayerMask groundLayer;

    [Space(5)]
    [Header("Attacks")]
    public ShockWaveAttack shockWaveAttackPrefab;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnDamage += Phases;
    }

    private IEnumerator Start()
    {
        foreach (var item in borderColliders)
        {
            item.canCollisionAttack = false;
        }

        yield return new WaitForSeconds(1);      
        attackStates.Add(new CubeRollState(stateMachine, this));
        attackStates.Add(new CubeJumpState(stateMachine, this));
        attackStates.Add(new CubeDashState(stateMachine, this));
        stateMachine.SetState(attackStates[0]);

        mult = firstPhaseMult;
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
        if (this.mult < secondPhaseMult && health.currentHealth <= health.maxHealth * 0.5f)
        {
            this.mult = secondPhaseMult;
        }
        else if (this.mult >= secondPhaseMult && this.mult < thirdPhaseMult &&  health.currentHealth <= health.maxHealth * 0.3f)
        {
            this.mult = thirdPhaseMult;
        }
    }
}
