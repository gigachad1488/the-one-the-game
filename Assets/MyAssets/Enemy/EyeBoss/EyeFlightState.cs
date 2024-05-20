using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFlightState : BaseState
{
    public EyeBoss boss;

    private float baseShootRate = 1f;
    public float shootRate;
    private float shootTimer;

    private float basePredictionPower = 0.6f;
    public float predictionPower;

    private float baseProjectileSpeed = 26f;
    private float projectileSpeed;

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
        shootTimer = shootRate;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        shootTimer -= Time.deltaTime;

        float distanceToPlayer = (boss.aggroedPlayer.transform.position - boss.transform.position).magnitude;
        Vector3 predictPosition = (boss.aggroedPlayer.transform.position + ((Vector3)boss.aggroedPlayer.playerMovement.rb.velocity * (distanceToPlayer / projectileSpeed) * predictionPower));

        float angle = Mathf.Atan2(predictPosition.y - boss.transform.position.y, predictPosition.x - boss.transform.position.x) * Mathf.Rad2Deg;
        float nextAngle = Mathf.LerpAngle(boss.rb.rotation, angle, Time.deltaTime * 8);

        boss.rb.MoveRotation(nextAngle);

        if (shootTimer <= 0)
        {
            shootTimer = shootRate;

            EyeBossLaser laser = Instantiate(boss.laserProjectilePrefab, boss.shootPoint.position, boss.transform.rotation);
            laser.Init(boss, projectileSpeed);
        }
    }
}
