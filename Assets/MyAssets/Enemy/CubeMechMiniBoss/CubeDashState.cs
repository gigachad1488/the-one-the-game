using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDashState : BaseState
{
    public CubeBoss cubeBoss;

    public float dashDelay = 1f;
    private float dashDelayTimer;

    public float dashDuration = 2.5f;

    public bool dashStart = false;
    public bool inDash = false;
    public bool dashOver = false;

    private float dashSide = 1;
    private float dashDestination = 0;

    private List<Thruster> sideThrusters;
    private bool thrustersAccelerated = false;

    public CubeDashState(StateMachine stateMachine, CubeBoss cubeBoss) : base(stateMachine)
    {
        this.cubeBoss = cubeBoss;
    }

    public override void OnEnter()
    {
        dashDelayTimer = dashDelay / (cubeBoss.mult * 0.5f);
        dashOver = false;
        dashStart = false;
        inDash = false;
        dashSide = 1;
        dashDestination = 0;

        cubeBoss.rb.freezeRotation = true;
        cubeBoss.rb.isKinematic = true; 
        
        sideThrusters = new List<Thruster>();
        thrustersAccelerated = false;
    }

    public override void OnExit()
    {
        foreach (var item in sideThrusters)
        {
            item.DisableParticles();
        }

        cubeBoss.rb.freezeRotation = false;
        cubeBoss.rb.isKinematic = false;
    }

    public override void OnUpdate()
    {
        dashDelayTimer -= Time.deltaTime;

        if (dashDelayTimer > 0 && sideThrusters.Count <= 0)
        {
            Vector3 dir = (Vector3)cubeBoss.rb.position - cubeBoss.aggroedPlayer.transform.position;

            if (dir.x >= 0)
            {
                dashSide = -1;
            }
            else
            {
                dashSide = 1;
            }

            foreach (var item in cubeBoss.thrusters)
            {
                if ((cubeBoss.transform.position.x - item.transform.position.x) * dashSide > 0)
                {
                    sideThrusters.Add(item);
                    item.EnableParticles(0.5f);
                }
            }
        }


        if (dashDelayTimer <= 0 && !dashStart)
        {
            dashStart = true; 
        }
        else if (dashStart && !inDash)
        {
            if (!thrustersAccelerated)
            {
                foreach (var item in sideThrusters)
                {
                    item.EnableParticles(4.5f);
                    thrustersAccelerated = true;
                }
            }
            inDash = true;
            dashDestination = cubeBoss.aggroedPlayer.transform.position.x + (30 * dashSide);
            float curPosX = cubeBoss.rb.position.x;
            Tween.Custom(curPosX, dashDestination, dashDuration / cubeBoss.mult, onValueChange: x => cubeBoss.rb.MovePosition(new Vector2(x, cubeBoss.rb.position.y)), Ease.InOutCubic).OnComplete(cubeBoss, x => x.ChangeRandomAttackState());
        }
    }
}
