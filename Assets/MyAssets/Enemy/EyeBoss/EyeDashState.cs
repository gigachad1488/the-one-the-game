using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDashState : BaseState
{
    public EyeBoss eyeBoss;

    private float baseDashDistance = 80f;
    private float dashDistance;

    private float baseDashTime = 1.2f;
    private float dashTime;
    private float dashTimeTimer;

    private float baseDashCd = 1.3f;
    private float dashCd;
    private float dashCdTimer;

    private bool dashStarted = false;
    private bool beginRotate = true;

    private Vector2 dashDestination;
    private Vector2 startPosition;
    private float rotationAngle;

    private float rotateTime;

    private float currentAngle;

    private int dashCount;

    private ParticleSystem.MainModule[] ramParticlesMains;

    public EyeDashState(StateMachine stateMachine, EyeBoss eyeBoss) : base(stateMachine)
    {
        this.eyeBoss = eyeBoss;

        ramParticlesMains = new ParticleSystem.MainModule[2];

        for (int i = 0; i < ramParticlesMains.Length; i++) 
        {
            ramParticlesMains[i] = eyeBoss.ramParticles[i].main;
        }
    }

    public override void OnEnter()
    {
        dashDistance = baseDashDistance * eyeBoss.mult * 0.1f;
        dashTime = baseDashTime / eyeBoss.mult * 0.9f;
        dashCd = baseDashCd / eyeBoss.mult * 0.9f;

        dashCount = Random.Range(3, 6);

        rotateTime = 0;

        dashTimeTimer = 0f;
        dashCdTimer = dashCd;
        dashStarted = false;
        beginRotate = true;

        currentAngle = eyeBoss.rb.rotation;

        ramParticlesMains[0].simulationSpeed = eyeBoss.mult;
        ramParticlesMains[1].simulationSpeed = eyeBoss.mult;

        eyeBoss.ramParticles[0].Play();
        eyeBoss.ramParticles[1].Play();
    }

    public override void OnExit()
    {
        eyeBoss.ramParticles[0].Stop();
        eyeBoss.ramParticles[1].Stop();
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

                rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                dashTimeTimer = 0;

                Tween.Custom(0, 1, dashTime, x =>
                {
                    float nextAngle = Mathf.LerpAngle(eyeBoss.rb.rotation, rotationAngle, Time.deltaTime * 15);
                    eyeBoss.rb.MoveRotation(nextAngle);

                    eyeBoss.rb.MovePosition(Vector3.Lerp(startPosition, dashDestination, x));
                }, Ease.InSine).OnComplete(eyeBoss, x =>
                {
                    dashStarted = false;
                    dashCdTimer = dashCd;
                    currentAngle = eyeBoss.rb.rotation;
                    rotateTime = 0;

                    dashCount--;
                });
            }

            //float nextAngle = Mathf.LerpAngle(eyeBoss.rb.rotation, rotationAngle, Time.deltaTime * 8);

            //eyeBoss.rb.MoveRotation(nextAngle);

            //float ratio = dashTimeTimer / dashTime;

            //Vector3 nextPosition = Vector3.Lerp(startPosition, dashDestination, ratio);
            //eyeBoss.rb.MovePosition(nextPosition);

            //dashTimeTimer += Time.deltaTime;

            /*
            if (ratio >= 0.99f)
            {
                dashStarted = false;
                dashCdTimer = dashCd;
                currentAngle = eyeBoss.rb.rotation;
                rotateTime = 0;

                dashCount--;
            }
            */
        }
        else
        {
            dashCdTimer -= Time.deltaTime;
            //rotateTime += Time.deltaTime;

            //float ratio = rotateTime / dashCd;

            float angle = Mathf.Atan2(eyeBoss.aggroedPlayer.transform.position.y - eyeBoss.transform.position.y, eyeBoss.aggroedPlayer.transform.position.x - eyeBoss.transform.position.x) * Mathf.Rad2Deg;
            float nextAngle = Mathf.LerpAngle(eyeBoss.rb.rotation, angle, Time.deltaTime * 5);

            eyeBoss.rb.MoveRotation(nextAngle);
        }

        if (dashCount <= 0)
        {
            eyeBoss.ChangeToFlight();
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
