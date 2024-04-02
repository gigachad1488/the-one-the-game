using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRollState : BaseState
{
    public CubeBoss cubeBoss;

    public float rollCd = 4f;

    private float rolls = 0;

    private float rollTimer;
    public CubeRollState(StateMachine stateMachine, CubeBoss cubeBoss) : base(stateMachine)
    {
        this.cubeBoss = cubeBoss;
    }

    public override void OnEnter()
    {
        rollTimer = 0;

        rollTimer = rollCd / cubeBoss.mult * 0.2f;

        rolls = 0;

        foreach (var item in cubeBoss.borderColliders)
        {
            item.canCollisionAttack = true;
        }
    }

    public override void OnExit()
    {
        foreach (var item in cubeBoss.borderColliders)
        {
            item.canCollisionAttack = false;
        }
    }
    
    public override void OnUpdate()
    {
        if (rollTimer <= 0 && Mathf.Abs(cubeBoss.rb.angularVelocity) <= 3)
        {
            if (Random.Range(0, 20 * rolls) > 50 * rolls * 0.2f) 
            {
                cubeBoss.ChangeRandomAttackState();
                return;
            }

            rolls++;

            rollTimer = rollCd / cubeBoss.mult;

            Vector3 dir = (Vector3)cubeBoss.rb.position - cubeBoss.aggroedPlayer.transform.position;
            //float side = Vector3.Dot(cubeBoss.rb.position, dir);
            if (dir.x >= 0)
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
