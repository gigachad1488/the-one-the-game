using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeJumpState : BaseState
{
    public CubeBoss cubeBoss;

    public float jumpDelay = 1f;
    private float jumpDelayTimer;

    public float inAirTime = 3f;
    private float inAirTimer;

    public float launchDelay = 0.2f;
    private float launchDelayTimer;

    private bool launched = false;
    public float launchForce = 80000f;

    private List<Thruster> downThrusters;
    private bool thrustersEnabled;

    public CubeJumpState(StateMachine stateMachine, CubeBoss cubeBoss) : base(stateMachine)
    {
        this.cubeBoss = cubeBoss;
    }

    public override void OnEnter()
    {
        jumpDelayTimer = jumpDelay / cubeBoss.mult;
        inAirTimer = inAirTime / cubeBoss.mult;
        launchDelayTimer= launchDelay / cubeBoss.mult;
        cubeBoss.rb.freezeRotation = true;
        launched = false;
        thrustersEnabled = false;

        downThrusters = new List<Thruster>();

        foreach (var item in cubeBoss.thrusters)
        {
            if (cubeBoss.transform.position.y - item.transform.position.y > 0)
            {
                downThrusters.Add(item);
            }
        }
    }

    public override void OnExit()
    {
        cubeBoss.rb.freezeRotation = false;
    }

    public override void OnUpdate()
    {
        jumpDelayTimer -= Time.deltaTime;

        if (inAirTimer <= 0)
        {
            launchDelayTimer -= Time.deltaTime;

            if (cubeBoss.rb.gravityScale < 1)
            {
                cubeBoss.rb.gravityScale = 1;
            }

            if (!launched && launchDelayTimer <= 0)
            {
                if (thrustersEnabled)
                {
                    foreach (var item in downThrusters)
                    {
                        item.DisableParticles();
                        thrustersEnabled = false;
                    }
                }

                cubeBoss.rb.AddForceY(-launchForce, ForceMode2D.Impulse);
                launched = true;
            }
            else if (launched && Mathf.Abs(cubeBoss.rb.velocityY) <= 0.2f)
            {
                foreach (var item in cubeBoss.borderColliders)
                {
                    if (item.isStaying)
                    {
                        Vector3 dir = item.gameObject.transform.position - cubeBoss.rb.gameObject.transform.position;
                        if (dir.x < 0)
                        {
                            item.CreateShockWave(-1, cubeBoss.mult * 0.8f);
                        }
                        else
                        {
                            item.CreateShockWave(1, cubeBoss.mult * 0.8f);
                        }
                    }
                }
                
                cubeBoss.ChangeRandomAttackState();
            }
            return;
        }

        if (jumpDelayTimer <= 0)
        {
            if (!thrustersEnabled) 
            {
                foreach (var item in downThrusters)
                {
                    item.EnableParticles(6f);
                    thrustersEnabled = true;
                }
            }

            if (cubeBoss.rb.gravityScale > 0)
            {
                cubeBoss.rb.gravityScale = 0;
            }
            inAirTimer -= Time.deltaTime;
            Vector3 dir = Vector3.Lerp(cubeBoss.rb.transform.position, cubeBoss.aggroedPlayer.transform.position + new Vector3(0, 15, 0), Time.deltaTime * 4 * (inAirTime / cubeBoss.mult / inAirTimer));
            cubeBoss.rb.MovePosition(dir);
        }
    }
}
