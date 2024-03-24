using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpeedLifetimeModule : ProjectileModule
{
    private float time;
    private float currentTime = 0;
    public AnimationCurve speedCurve;

    private Rigidbody2D projectileRb;

    private float coroutineTickTime = 0.1f;
    private WaitForSeconds coroutineTick;

    private Vector3 startSpeed;
    public override void AfterSet()
    {
        time = speedCurve.keys[speedCurve.length - 1].time;
        projectileRb = projectile.GetComponent<Rigidbody2D>();
        startSpeed = projectileRb.velocity;
        coroutineTick = new WaitForSeconds(0.1f);

        StartCoroutine(SpeedChange());
    }

    private IEnumerator SpeedChange()
    {
        while (currentTime <= time)
        {
            Vector3 vel = startSpeed * speedCurve.Evaluate(currentTime);
            projectileRb.velocity = vel;
            currentTime += coroutineTickTime;
            yield return coroutineTick;
        }
    }

    public override void ProjectileHit()
    {
    }
}
