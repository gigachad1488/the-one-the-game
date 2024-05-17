using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDashState : BaseState
{
    public EyeBoss eyeBoss;

    private float baseDashDistance = 45f;
    private float dashDistance;

    private float baseDashTime = 1.2f;
    private float dashTime;
    private float dashTimeTimer;

    private float baseDashCd = 2f;
    private float dashCd;
    private float dashCdTimer;

    private bool dashStarted = false;
    private bool beginRotate = true;

    private Vector2 dashDestination;
    private Vector2 startPosition;

    private float rotateTime;

    private float currentAngle;

    public EyeDashState(StateMachine stateMachine, EyeBoss eyeBoss) : base(stateMachine)
    {
        this.eyeBoss = eyeBoss;
    }

    public override void OnEnter()
    {
        dashDistance = baseDashDistance * eyeBoss.mult * 0.5f;
        dashTime = baseDashTime / eyeBoss.mult * 0.5f;
        dashCd = baseDashCd / eyeBoss.mult * 0.5f;

        rotateTime = 0;

        dashTimeTimer = 0f;
        dashCdTimer = dashCd;
        dashStarted = false;
        beginRotate = true;

        currentAngle = eyeBoss.rb.rotation;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (dashCdTimer <= 0f) 
        {
            if (!dashStarted) 
            {
                dashStarted = true;

                Vector3 direction = (eyeBoss.aggroedPlayer.gameObject.transform.position - eyeBoss.transform.position).normalized;
                Vector3 fixedDirection = direction * dashDistance;

                dashDestination = (eyeBoss.transform.position + (Vector3)eyeBoss.aggroedPlayer.playerMovement.rb.velocity.normalized * 4f) + fixedDirection;
                startPosition = eyeBoss.rb.position;              

                dashTimeTimer = 0;
            }
            
            float ratio = dashTimeTimer / dashTime;

            Vector3 nextPosition = Vector3.Lerp(startPosition, dashDestination, ratio);
            eyeBoss.rb.MovePosition(nextPosition);

            dashTimeTimer += Time.deltaTime;

            if (ratio >= 0.99f)
            {
                dashStarted = false;
                dashCdTimer = dashCd;
                currentAngle = eyeBoss.rb.rotation;
                rotateTime = 0;
            }
        }
        else
        {
            dashCdTimer -= Time.deltaTime;
            rotateTime += Time.deltaTime;

            float ratio = rotateTime / dashCd;

            float angle = Mathf.Atan2(eyeBoss.aggroedPlayer.transform.position.y - eyeBoss.transform.position.y, eyeBoss.aggroedPlayer.transform.position.x - eyeBoss.transform.position.x) * Mathf.Rad2Deg;
            float nextAngle = Mathf.Lerp(currentAngle, angle, ratio);

            eyeBoss.rb.MoveRotation(nextAngle);
        }
    }

    private void RotateToPlayer(float time)
    {
        float angle = Mathf.Atan2(eyeBoss.aggroedPlayer.transform.position.y - eyeBoss.transform.position.y, eyeBoss.aggroedPlayer.transform.position.x - eyeBoss.transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, time);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(dashDestination, 50);
    }
}
