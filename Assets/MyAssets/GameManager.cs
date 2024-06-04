using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] bosses;

    public Player player;

    public MultiSceneData multiScenedata;

    public Transform bossSpawnPoint;

    public WeaponBuilder weaponBuilder;
    public Canvas endCanvas;
    public Button endMenuButton;
    public Transform weaponsLayout;
    public InventorySlot slotPrefab;
    public WeaponItem weaponItemPrefab;

    public static GameManager instance;

    private List<Weapon> weapons = new List<Weapon>();

    private void Awake()
    {
        multiScenedata = GameObject.FindGameObjectWithTag("MultiScene").GetComponent<MultiSceneData>();

        endMenuButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("MainMenu"));
        endCanvas.gameObject.SetActive(false);

        player.playerData = multiScenedata.playerData;

        if (instance == null)
        {
            instance = this;
        }
    }

    private IEnumerator Start()
    {       
        Array enums = Enum.GetValues(typeof(WeaponType));

        int dropCount = UnityEngine.Random.Range(multiScenedata.minDrop, multiScenedata.maxDrop + 1);

        for (int i = 0; i < dropCount; i++) 
        {
            yield return weaponBuilder.BuildWeapon((WeaponType)enums.GetValue(UnityEngine.Random.Range(0, enums.Length)), (weapon) =>
            {
                InventorySlot slot = Instantiate(slotPrefab, weaponsLayout.transform);
                slot.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                WeaponItem item = Instantiate(weaponItemPrefab, slot.transform);
                item.weapon = weapon;
                item.GetComponent<Image>().raycastTarget = false;
                weapons.Add(weapon);
            }, multiScenedata.level);
        }       

        SpawnBoss();

        yield return null;
    }

    public void SpawnBoss()
    {
        IBoss boss = Instantiate(bosses[multiScenedata.bossId], bossSpawnPoint.position, Quaternion.identity).GetComponent<IBoss>();
        boss.difficultyMult = multiScenedata.mult;
        boss.aggroedPlayer = player;
    }

    public void ShowEndMenu()
    {
        JsonDataService service = new JsonDataService();
        
        foreach (var weapon in weapons) 
        {
            service.SaveData("w" + weapon.guid.ToString(), weapon.GetData());
        }

        endCanvas.gameObject.SetActive(true);
    }
}
