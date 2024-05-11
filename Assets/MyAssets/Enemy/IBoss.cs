using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoss
{
    public Player aggroedPlayer { get; set; }
    public float difficultyMult { get; set; }
}
