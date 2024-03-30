using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBoss : Health
{
    public StateMachine stateMachine;

    [HideInInspector]
    public List<BaseState> states = new List<BaseState>();

    public Rigidbody2D rb;

    public Player aggroedPlayer;

    public float mult = 1f;

    public CubeBordersFallAttack[] borderColliders;

    [Space(5)]
    [Header("Attacks")]
    public ShockWaveAttack shockWaveAttackPrefab;

    private IEnumerator Start()
    {
        foreach (var item in borderColliders)
        {
            item.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1);
        states.Add(new CubeRollState(stateMachine, this));
        stateMachine.SetState(states[0]);
    }

    private void Update()
    {
        stateMachine.currentState?.OnUpdate();
    }
}
