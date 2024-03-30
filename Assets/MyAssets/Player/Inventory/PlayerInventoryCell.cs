using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryCell : MonoBehaviour
{
    [SerializeField]
    private Image weaponSprite;
    [SerializeField]
    private Image cellBorder;

    [SerializeField]
    private Color selectedColor = Color.gray;
    private Color defaultColor;

    public int cellId;

    private void Awake()
    {
        defaultColor = cellBorder.color;
    }

    public void SetWeapon(Weapon weapon)
    {
        weaponSprite.sprite = weapon.weaponModelPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public int SelectCell()
    {
        cellBorder.color = selectedColor;
        return cellId;
    }

    public void UnselectCell()
    {
        cellBorder.color = defaultColor;
    }
}
