using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBossLaserState : BaseState
{
    private EyeBoss boss;

    private float baseLaserPrepCd = 6f;
    private float laserPrepCd;
    private float laserPrepCdTimer;

    private float baseLaserCycleDuration = 12f;
    private float laserCycleDuration;
    private float laserCycleDurationTimer;

    private bool laserStarted = false;

    private float finishRotation;
    private float initRotation;

    private float baseRotation = 360f;
    private float rotation;

    private float yOffset;
    private float xOffset;

    private ParticleSystem.MainModule beamParticlesMainModule;

    private GameObject beamGameObject;
    private ContactDamage beam;

    public EyeBossLaserState(StateMachine stateMachine, EyeBoss eyeBoss) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
        this.boss = eyeBoss;

        beamParticlesMainModule = boss.beamParticles.main;
    }

    public override void OnEnter()
    {
        yOffset = Random.Range(8f, 25f);

        xOffset = Random.Range(-35f, 35f);

        laserPrepCd = baseLaserPrepCd / boss.mult * 0.8f;
        laserCycleDuration = baseLaserCycleDuration / boss.mult * 0.4f;

        rotation = baseRotation * boss.mult;

        laserPrepCdTimer = 0;
        laserCycleDurationTimer = 0;

        laserStarted = false;

        beamGameObject = Instantiate(boss.beamPrefab, boss.shootPoint);
        beamGameObject.transform.up = boss.shootPoint.right;

        beam = beamGameObject.GetComponentInChildren<ContactDamage>();
        beam.boss = boss.gameObject;
        beam.mult = 1.5f;

        beamGameObject.SetActive(false);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {     
        if (laserPrepCdTimer >= laserPrepCd)
        {
            if (!laserStarted)
            {
                laserStarted = true;

                initRotation = boss.rb.rotation;

                float distance = (boss.aggroedPlayer.transform.position - boss.transform.position).magnitude;

                /*

                LineSegmentsIntersection(boss.aggroedPlayer.transform.position - new Vector3(distance * 2, 0, 0), boss.aggroedPlayer.transform.position + new Vector3(distance * 2, 0, 0), boss.transform.position, boss.shootPoint.right.normalized * Vector3.Distance(boss.shootPoint.position, boss.aggroedPlayer.transform.position) * 2, out Vector2 intersection);


                Vector3 checkPos = boss.shootPoint.right + boss.shootPoint.right.normalized * Vector3.Distance(boss.transform.position, boss.aggroedPlayer.transform.position) * 2;

                Debug.DrawLine(boss.aggroedPlayer.transform.position, boss.aggroedPlayer.transform.position + new Vector3(distance * 2, 0, 0), Color.blue, 5);
                Debug.DrawLine(boss.shootPoint.position, intersection, Color.red, 5);
                Debug.DrawLine(boss.shootPoint.position, checkPos, Color.green, 5);

                */

                //var dot = Vector3.Dot(beam.transform.position, boss.aggroedPlayer.transform.position);

                var wtf = boss.shootPoint.InverseTransformPoint(boss.aggroedPlayer.transform.position);

                float omg = (boss.shootPoint.position - boss.aggroedPlayer.transform.position).sqrMagnitude;

                //Debug.Log("OMG = " + wtf);

                if (wtf.x > 0 || wtf.y <= 0)
                {
                    //Debug.Log("ROTATING LEFT");
                    finishRotation = -rotation;
                }
                else
                {
                    //Debug.Log("ROTATING RIGHT");
                    finishRotation = rotation;
                }

                if (wtf.x > 0 && wtf.y > 0)
                {
                    //Debug.Log("ROTATING LEFT");
                    finishRotation = -rotation;
                }

                if (wtf.x > 0 && wtf.y < 0)
                {
                    //Debug.Log("ROTATING RIGHT");
                    finishRotation = rotation;
                }

                if (wtf.x < 0 && wtf.y < 0)
                {
                    //Debug.Log("ROTATING RIGHT");
                    finishRotation = rotation;
                }

                if (wtf.x <= 0 && wtf.y >= 0)
                {
                    //Debug.Log("ROTATING RIGHT");
                    finishRotation = -rotation;
                }

                Tween.EulerAngles(boss.transform, new Vector3(0, 0, boss.rb.rotation), new Vector3(0, 0, boss.rb.rotation + finishRotation), laserCycleDuration).OnComplete(boss, x => 
                {
                    Destroy(beamGameObject);
                    boss.ChangeToFlight();
                });
                
                beamGameObject.SetActive(true);
            }

            boss.beamParticles.Emit(1);

            //boss.rb.MoveRotation(Mathf.Lerp(initRotation, finishRotation, laserCycleDurationTimer / laserCycleDuration));

            //laserCycleDurationTimer += Time.deltaTime;

            if (laserCycleDurationTimer >= laserCycleDuration)
            {
                
            }
        }
        else
        {
            float ratio = laserPrepCdTimer / laserPrepCd;

            boss.rb.MovePosition(Vector2.MoveTowards(boss.rb.position, new Vector2(boss.aggroedPlayer.transform.position.x + xOffset, boss.aggroedPlayer.transform.position.y + yOffset), 60f * Time.deltaTime * (0.8f - ratio)));

            boss.beamParticles.Emit(4);

            beamParticlesMainModule.simulationSpeed = 0.5f + ratio;

            float angle = Mathf.Atan2(boss.aggroedPlayer.transform.position.y - boss.transform.position.y, boss.aggroedPlayer.transform.position.x - boss.transform.position.x) * Mathf.Rad2Deg;
            float nextAngle = Mathf.LerpAngle(boss.rb.rotation, angle, Time.deltaTime * 2);

            boss.rb.MoveRotation(nextAngle);

            laserPrepCdTimer += Time.deltaTime;
        }
    }

    public static bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if (d == 0.0f)
        {
            return false;
        }

        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        intersection.x = p1.x + u * (p2.x - p1.x);
        intersection.y = p1.y + u * (p2.y - p1.y);

        return true;
    }
}
