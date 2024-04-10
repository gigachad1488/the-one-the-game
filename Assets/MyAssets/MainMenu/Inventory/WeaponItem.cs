using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour
{
    public Weapon weapon;

    [SerializeField]
    private Image image;

    private void Start()
    {
        Sprite sprite = weapon.weaponModelPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        image.sprite = sprite;
    }
}
