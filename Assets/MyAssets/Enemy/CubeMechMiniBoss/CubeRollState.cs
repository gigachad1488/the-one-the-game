using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRollState : BaseState
{
    public CubeBoss cubeBoss;

    public float rollCd = 4f;

    private float rollTimer;
    public CubeRollState(StateMachine stateMachine, CubeBoss cubeBoss) : base(stateMachine)
    {
        this.cubeBoss = cubeBoss;
    }

    public override void OnEnter()
    {
        rollTimer = rollCd / cubeBoss.mult;
        foreach (var item in cubeBoss.borderColliders)
        {
            item.gameObject.SetActive(true);
        }
    }

    public override void OnExit()
    {
        foreach (var item in cubeBoss.borderColliders)
        {
            item.gameObject.SetActive(false);
        }
    }
    
    public override void OnUpdate()
    {
        Debug.Log("SHET");
        if (rollTimer <= 0 && Mathf.Abs(cubeBoss.rb.angularVelocity) <= 3)
        {
            rollTimer = rollCd / cubeBoss.mult;
            Vector3 dir = (Vector3)cubeBoss.rb.position - cubeBoss.aggroedPlayer.transform.position;
            float side = Vector3.Dot(cubeBoss.transform.right, dir);
            if (side > 0)
            {
                Roll(-1);
            }
            else
            {
                Roll(1);
            }
        }

        rollTimer -= Time.deltaTime;
    }

    public void Roll(float side)
    {
        cubeBoss.rb.AddTorque(15000000 * -side * (1 + cubeBoss.mult * 0.15f));
        cubeBoss.rb.AddForceX(side * 1000000 * (1 + cubeBoss.mult * 0.15f));
    }
}
