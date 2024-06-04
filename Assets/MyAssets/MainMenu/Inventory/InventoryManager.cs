using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Weapon> weapons;

    public InventorySlot[] selectedWeaponsSlots = new InventorySlot[3];

    public MultiSceneData multiSceneData;

    [SerializeField]
    private InventorySlot slotPrefab;

    [SerializeField]
    private WeaponItem weaponItemPrefab;

    [SerializeField]
    private Transform slotsGrid;

    public WeaponBuilder builder;

    public bool build = true;
    public int count = 1;

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
        if (build)
        {
            for (int i = 0; i < count; i++)
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
                    InventorySlot slot = Instantiate(slotPrefab, slotsGrid.transform);
                    WeaponItem weaponItem = Instantiate(weaponItemPrefab, slot.transform);
                    weaponItem.weapon = weapon;
                    weapon.transform.SetParent(weaponItem.transform);

                    var data = weapon.GetData();
                    JsonDataService service = new JsonDataService();
                    service.SaveData("w" + weapon.guid.ToString(), data);
                }, 1));
            }
        }
        else
        {
            JsonDataService dataService = new JsonDataService();

            string[] weapons = Directory.GetFiles(Application.persistentDataPath).Where(x => Path.GetFileName(x).StartsWith('w')).ToArray();

            Guid[] selectedGuids = new Guid[3];

            for (int i = 0; i < selectedGuids.Length; i++)
            {
                string id = "selectedWeapon" + (i + 1);
                if (Guid.TryParse(PlayerPrefs.GetString(id, string.Empty), out Guid result))
                {
                    selectedGuids[i] = result;
                }
            }

            if (weapons.Length <= 0)
            {
                StartCoroutine(builder.BuildStartWeapon((weapon) =>
                {
                    InventorySlot slot = Instantiate(slotPrefab, slotsGrid.transform);

                    for (int i = 0; i < selectedGuids.Length; i++)
                    {
                        if (weapon.guid.Equals(selectedGuids[i]))
                        {
                            slot = selectedWeaponsSlots[i];
                            break;
                        }
                    }

                    WeaponItem weaponItem = Instantiate(weaponItemPrefab, slot.transform);
                    weaponItem.weapon = weapon;
                    weapon.transform.SetParent(weaponItem.transform);

                    var data = weapon.GetData();
                    dataService.SaveData("w" + weapon.guid.ToString(), data);
                }));
            }
            else
            {
                foreach (string wpn in weapons)
                {
                    string fileName = Path.GetFileName(wpn);
                    StartCoroutine(builder.BuildWeaponFromJson(dataService.LoadData<WeaponBaseData>(fileName), fileName, (weapon) =>
                    {
                        InventorySlot slot = Instantiate(slotPrefab, slotsGrid.transform);

                        for (int i = 0; i < selectedGuids.Length; i++)
                        {
                            if (weapon.guid.Equals(selectedGuids[i]))
                            {
                                slot = selectedWeaponsSlots[i];
                                break;
                            }
                        }

                        WeaponItem weaponItem = Instantiate(weaponItemPrefab, slot.transform);
                        weaponItem.weapon = weapon;
                        weapon.transform.SetParent(weaponItem.transform);
                    }));
                }
            }
        }
    }

    public void SaveSelectedWeapons()
    {
        multiSceneData.selectedWeapons.Clear();

        int slot = 1;

        foreach (var item in selectedWeaponsSlots)
        {
            if (item.transform.childCount != 0)
            {
                WeaponItem weaponItem = item.transform.GetChild(0).GetComponent<WeaponItem>();
                multiSceneData.selectedWeapons.Add(weaponItem.weapon);
                weaponItem.weapon.transform.SetParent(multiSceneData.transform);

                string id = "selectedWeapon" + slot;
                PlayerPrefs.SetString("selectedWeapon" + slot, weaponItem.weapon.guid.ToString());
            }
            else
            {
                PlayerPrefs.SetString("selectedWeapon" + slot, string.Empty);
            }

            slot++;
        }
    }
}

[Serializable]
public class ModuleDataType
{
    [SerializeReference]
    public ModuleData data = new ModuleData();
}

