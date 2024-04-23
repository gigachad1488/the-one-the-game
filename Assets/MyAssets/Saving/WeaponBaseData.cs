using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[Serializable]
public class WeaponBaseData
{
    public string weaponModelAddressablesPath;

    public int baseDamage;
    public float baseAttackSpeed;
    public float baseScale;

    public int level;

    public float damageMult;
    public int damageFlat;

    public float attackSpeedMult;   
    public float attackSpeedFlat;

    public float scaleMult;
    public float scaleFlat;

    public ModuleData actionData;
    public ModuleData shootData;
    public ProjectileData projectileData;
}
