using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeJumpState : BaseState
{
    public CubeBoss cubeBoss;

    public float jumpDelay = 1.5f;
    private float jumpDelayTimer;

    public float inAirTime = 3f;
    private float inAirTimer;

    private bool launched = false;
    public float launchForce = 200000f;

    public CubeJumpState(StateMachine stateMachine, CubeBoss cubeBoss) : base(stateMachine)
    {
        this.cubeBoss = cubeBoss;
    }

    public override void OnEnter()
    {
        jumpDelayTimer = jumpDelay / cubeBoss.mult;
        inAirTimer = inAirTime / cubeBoss.mult;
        cubeBoss.rb.freezeRotation = true;
        launched = false;
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
            if (cubeBoss.rb.gravityScale < 1)
            {
                cubeBoss.rb.gravityScale = 1;
            }

            if (!launched)
            {
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
                            item.CreateShockWave(-1);
                        }
                        else
                        {
                            item.CreateShockWave(1);
                        }
                    }
                }
                
                cubeBoss.ChangeRandomAttackState();
            }
            return;
        }

        if (jumpDelayTimer <= 0)
        {
            if (cubeBoss.rb.gravityScale > 0)
            {
                cubeBoss.rb.gravityScale = 0;
            }
            inAirTimer -= Time.deltaTime;
            Vector3 dir = Vector3.Lerp(cubeBoss.rb.transform.position, cubeBoss.aggroedPlayer.transform.position + new Vector3(0, 15, 0), Time.deltaTime * 4 * (inAirTime / inAirTimer));
            cubeBoss.rb.MovePosition(dir);
        }
    }
}
