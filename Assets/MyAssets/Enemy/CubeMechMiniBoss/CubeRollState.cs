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

    private Thruster thruster = null;
    private float thrusterDisableCd = 0.5f;
    private float thrusterEnableTimer = 0.3f;
    public CubeRollState(StateMachine stateMachine, CubeBoss cubeBoss) : base(stateMachine)
    {
        this.cubeBoss = cubeBoss;
    }

    public override void OnEnter()
    {
        rollTimer = rollCd / cubeBoss.mult * 0.2f;
        rolls = 0;
        thrusterEnableTimer = 0;
        thrusterDisableCd = 0;

        thruster = null;

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

        if (thruster != null)
        {
            thruster.DisableParticles();
            thruster = null;
        }
    }

    public override void OnUpdate()
    {
        if (Mathf.Abs(cubeBoss.rb.angularVelocity) <= 0.05f)
        {
            thrusterDisableCd -= Time.deltaTime;

            if (thruster != null && thrusterDisableCd <= 0)
            {
                thruster.DisableParticles();
                thruster = null;
            }

            if (rollTimer <= 0)
            {

                if (Random.Range(0, 20 * rolls) > 50 * rolls * 0.2f)
                {
                    cubeBoss.ChangeRandomAttackState();
                    return;
                }

                rolls++;
                rollTimer = rollCd / cubeBoss.mult;

                Vector3 dir = (Vector3)cubeBoss.rb.position - cubeBoss.aggroedPlayer.transform.position;
                float side;
                //float side = Vector3.Dot(cubeBoss.rb.position, dir);
                if (dir.x >= 0)
                {
                    side = -1;
                }
                else
                {
                    side = 1;
                }

                foreach (var item in cubeBoss.thrusters)
                {
                    if (((cubeBoss.transform.position.x - item.transform.position.x) * side) >= 0 && (cubeBoss.transform.position.y - item.transform.position.y) >= 0)
                    {
                        if (thruster != null)
                        {
                            thruster.DisableParticles();
                        }

                        thruster = item;
                        thrusterDisableCd = 0.1f;
                        thrusterEnableTimer = 0.5f / cubeBoss.mult;
                        break;
                    }
                }


                Roll(side);
            }
        }
        else
        {
            if (thruster != null)
            {
                float power = Mathf.Clamp(100 * thrusterEnableTimer, 0.5f, 50f);
                thruster.EnableParticles(power);
            }
        }

        thrusterEnableTimer -= Time.deltaTime;
        rollTimer -= Time.deltaTime;
    }

    public void Roll(float side)
    {
        cubeBoss.rb.AddTorque(15000000 * -side * (1 + cubeBoss.mult * 0.15f));
        cubeBoss.rb.AddForceX(side * 1000000 * (1 + cubeBoss.mult * 0.15f));
    }
}
