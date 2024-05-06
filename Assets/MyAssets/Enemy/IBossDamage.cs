using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossDamage
{
    public float difficultyMult { get; set; }
    public int baseDamage { get; set; }
}
