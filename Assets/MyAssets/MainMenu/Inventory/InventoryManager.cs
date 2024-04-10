using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Weapon> weapons;

    public InventorySlot[] selectedWeaponsSlots = new InventorySlot[3];

    public SelectedWeaponsSO selectedWeaponsSO;

    [SerializeField]
    private InventorySlot slotPrefab;

    [SerializeField]
    private WeaponItem weaponItemPrefab;

    [SerializeField]
    private Transform slotsGrid;

    private void Start()
    {
        int inSelectedCount = 0;
        foreach (Weapon weapon in weapons) 
        {          
            if (!selectedWeaponsSO.selectedWeapons.Contains(weapon))
            {
                InventorySlot slot = Instantiate(slotPrefab, slotsGrid);
                WeaponItem item = Instantiate(weaponItemPrefab, slot.transform);
                item.weapon = weapon;
            }
            else
            {
                InventorySlot slot = Instantiate(slotPrefab, slotsGrid);
                WeaponItem item = Instantiate(weaponItemPrefab, selectedWeaponsSlots[inSelectedCount].transform);
                item.weapon = weapon;
                inSelectedCount++;
            }
        }
    }

    public void SaveSelectedWeapons()
    {
        selectedWeaponsSO.selectedWeapons.Clear();
        foreach (var item in selectedWeaponsSlots)
        {
            if (item.transform.childCount != 0)
            {
                WeaponItem weaponItem = item.transform.GetChild(0).GetComponent<WeaponItem>();
                selectedWeaponsSO.selectedWeapons.Add(weaponItem.weapon);
            }
        }
    }
}
