using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[Serializable]
public class WeaponBaseData
{
    //public Guid guid;

    public string weaponModelAddressablesPath;
    public string projectileAddressablesPath;

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

    [SerializeReference]
    public ModuleDataType actionData;
    [SerializeReference]
    public ModuleDataType shootData;
    [SerializeReference]
    public ModuleDataType projectileData;
}
