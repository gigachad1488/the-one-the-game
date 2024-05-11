using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSceneData : MonoBehaviour
{
    public float mult = 1;
    
    public int bossId = 0;

    public List<Weapon> selectedWeapons = new List<Weapon>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
