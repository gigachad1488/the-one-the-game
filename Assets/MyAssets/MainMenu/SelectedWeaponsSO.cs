using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedWeapons : MonoBehaviour 
{
    public List<Weapon> selectedWeapons = new List<Weapon>();
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
