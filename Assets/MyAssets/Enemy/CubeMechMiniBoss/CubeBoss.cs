using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBoss : MonoBehaviour
{
    public StateMachine stateMachine;

    [HideInInspector]
    public List<BaseState> attackStates = new List<BaseState>();

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
            item.canCollisionAttack = false;
        }

        yield return new WaitForSeconds(1);
        attackStates.Add(new CubeRollState(stateMachine, this));
        attackStates.Add(new CubeJumpState(stateMachine, this));
        stateMachine.SetState(attackStates[0]);
    }

    private void Update()
    {
        stateMachine.currentState?.OnUpdate();
    }

    public void ChangeRandomAttackState()
    {
        stateMachine.SetState(attackStates[Random.Range(0, attackStates.Count)]);
    }
}
