using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelectedWeapons", menuName = "Weapons/SelectedWeapons")]
public class SelectedWeaponsSO : ScriptableObject
{
    [SerializeReference]
    public List<Weapon> selectedWeapons = new List<Weapon>();
}
