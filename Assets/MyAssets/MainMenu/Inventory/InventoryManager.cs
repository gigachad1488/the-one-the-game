using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.VisualScripting;
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




        if (false)
        {
            for (int i = 0; i < 1; i++)
            {
                int r = UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeaponType)).Length);
                WeaponType type;
                if (r == 0)
                {
                    type = WeaponType.Ranged;
                }
                else if (r == 1)
                {
                    type = WeaponType.MeleeDelault;
                }
                else if (r == 2)
                {
                    type = WeaponType.MeleeSwing;
                }
                else
                {
                    type = WeaponType.MeleeSpear;
                }



                StartCoroutine(builder.BuildWeapon(type, (weapon) =>
                {
                    string key = "weapon" + Guid.NewGuid().ToString();

                    InventorySlot slot = Instantiate(slotPrefab, slotsGrid.transform);
                    WeaponItem weaponItem = Instantiate(weaponItemPrefab, slot.transform);
                    weaponItem.weapon = weapon;
                    weapon.transform.SetParent(weaponItem.transform);

                    var data = weapon.GetData();
                    JsonDataService service = new JsonDataService();
                    service.SaveData("aboba", data);

                }, 5));
            }
        }
        else
        {
            JsonDataService dataService = new JsonDataService();

            StartCoroutine(builder.BuildWeaponFromJson(dataService.LoadData<WeaponBaseData>("aboba"), (weapon) =>
            {
                InventorySlot slot = Instantiate(slotPrefab, slotsGrid.transform);
                WeaponItem weaponItem = Instantiate(weaponItemPrefab, slot.transform);
                weaponItem.weapon = weapon;
                weapon.transform.SetParent(weaponItem.transform);
            }));
        }
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
