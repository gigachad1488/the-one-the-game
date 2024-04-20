using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Weapon> weapons;

    public InventorySlot[] selectedWeaponsSlots = new InventorySlot[3];

    public SelectedWeapons selectedWeapons;

    [SerializeField]
    private InventorySlot slotPrefab;

    [SerializeField]
    private WeaponItem weaponItemPrefab;

    [SerializeField]
    private Transform slotsGrid;

    public WeaponBuilder builder;

    private void Awake()
    {
        selectedWeapons = GameObject.FindGameObjectWithTag("SelectedWeapons").GetComponent<SelectedWeapons>();
    }

    private void Start()
    {
        int inSelectedCount = 0;
        /*

        selectedWeaponsSO.selectedWeapons.RemoveAll(x => x == null);

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
        */

        StartCoroutine(builder.BuildWeapon(WeaponType.Ranged, (weapon) =>
        {
            InventorySlot slot = Instantiate(slotPrefab, slotsGrid);
            WeaponItem item = Instantiate(weaponItemPrefab, slot.transform);
            item.weapon = weapon;
            weapon.transform.SetParent(item.transform);
        }, 1));

        StartCoroutine(builder.BuildWeapon(WeaponType.MeleeDelault, (weapon) =>
        {
            InventorySlot slot = Instantiate(slotPrefab, slotsGrid);
            WeaponItem item = Instantiate(weaponItemPrefab, slot.transform);
            item.weapon = weapon;
            weapon.transform.SetParent(item.transform);
        }, 1));
    }

    public void SaveSelectedWeapons()
    {
        selectedWeapons.selectedWeapons.Clear();
        foreach (var item in selectedWeaponsSlots)
        {
            if (item.transform.childCount != 0)
            {
                WeaponItem weaponItem = item.transform.GetChild(0).GetComponent<WeaponItem>();
                selectedWeapons.selectedWeapons.Add(weaponItem.weapon);
                weaponItem.weapon.transform.SetParent(selectedWeapons.transform);
            }
        }
    }
}
