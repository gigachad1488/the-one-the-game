using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFlightState : BaseState
{
    public EyeBoss boss;

    private float baseMoveSpeed = 20f;
    private float moveSpeed;

    private float baseShootRate = 1f;
    public float shootRate;
    private float shootTimer;

    private float basePredictionPower = 0.6f;
    public float predictionPower;

    private float baseProjectileSpeed = 30f;
    private float projectileSpeed;

    private float xOffsetCd = 1f;
    private float xOffsetCdTimer;
    private float xOffset = 15f;
    private float yOffset = 17f;

    private float moveSpeedMult = 1f;

    private float currentYOffset;
    private float currentXOffset;

    public EyeFlightState(StateMachine stateMachine, EyeBoss eyeBoss) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
        this.boss = eyeBoss;
    }

    public override void OnEnter()
    {
        shootRate = baseShootRate / (boss.mult * 0.8f);
        predictionPower = basePredictionPower * (boss.mult * 0.8f);
        projectileSpeed = baseProjectileSpeed * boss.mult;
        moveSpeed = baseMoveSpeed * boss.mult;
        shootTimer = shootRate;

        xOffsetCdTimer = 0;

        currentYOffset = yOffset;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        shootTimer -= Time.deltaTime;
        xOffsetCdTimer -= Time.deltaTime;

        float distanceToPlayer = (boss.aggroedPlayer.transform.position - boss.transform.position).magnitude;
        Vector3 predictPosition = (boss.aggroedPlayer.transform.position + ((Vector3)boss.aggroedPlayer.playerMovement.rb.velocity * (distanceToPlayer / projectileSpeed) * predictionPower));

        float angle = Mathf.Atan2(predictPosition.y - boss.transform.position.y, predictPosition.x - boss.transform.position.x) * Mathf.Rad2Deg;
        float nextAngle = Mathf.LerpAngle(boss.rb.rotation, angle, Time.deltaTime * 30);

        boss.rb.MoveRotation(nextAngle);

        if (shootTimer <= 0)
        {
            shootTimer = shootRate;

            EyeBossLaser laser = Instantiate(boss.laserProjectilePrefab, boss.shootPoint.position, boss.transform.rotation);
            laser.Init(boss, projectileSpeed);
        }

        if (xOffsetCdTimer <= 0f)
        {
            xOffsetCdTimer = xOffsetCd;
            currentXOffset = Random.Range(-xOffset, xOffset + 0.1f);
            moveSpeedMult = 0f;
            Tween.Custom(0.2f, 1, xOffsetCd * 0.3f, x => moveSpeedMult = x, Ease.InSine);
        }

        boss.rb.MovePosition(Vector2.MoveTowards(boss.rb.position, new Vector2(predictPosition.x + currentXOffset, predictPosition.y + currentYOffset), moveSpeed * Time.deltaTime * moveSpeedMult));
    }
}
