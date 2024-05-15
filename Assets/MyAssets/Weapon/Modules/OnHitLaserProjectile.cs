using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitLaserProjectile : MonoBehaviour
{
    public Vector3 destination;

    public TrailRenderer trailRenderer;

    private void Start()
    {
        Tween.Position(transform, destination, new TweenSettings(0.5f, useFixedUpdate: true));
    }
}
