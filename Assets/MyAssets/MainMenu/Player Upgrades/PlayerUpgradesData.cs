using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerUpgradesData
{
    public readonly int baseHp = 200;
    public readonly float baseMoveSpeed = 30f;
    public readonly float baseFlyForce = 15f;
    public readonly float baseFlyTime = 1f;
    public readonly float baseDashCd = 1.5f;
    public readonly float baseDashForce = 30f;

    public readonly int baseHpScale = 20;
    public readonly float baseMoveSpeedScale = 2f;
    public readonly float baseFlyForceScale = 1f;
    public readonly float baseFlyTimeScale = 0.2f;
    public readonly float baseDashCdScale = 0.05f;
    public readonly float baseDashForceScale = 2f;

    public int hpLevel = 1;
    public int moveSpeedLevel = 1;
    public int flyForceLevel = 1;
    public int flyTimeLevel = 1;
    public int dashCdLevel = 1;
    public int dashForceLevel = 1;
}
