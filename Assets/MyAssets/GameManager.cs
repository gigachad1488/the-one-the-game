using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.HDROutputUtils;

public class GameManager : MonoBehaviour
{
    public string[] bossesLabel;
    public GameObject[] bossesPrefabs;

    public Player player;

    public MultiSceneData multiScenedata;

    public Transform bossSpawnPoint;

    public WeaponBuilder weaponBuilder;
    public Canvas endCanvas;
    public Button endMenuButton;
    public TextMeshProUGUI rewardText;
    public Transform weaponsLayout;
    public InventorySlot slotPrefab;
    public WeaponItem weaponItemPrefab;

    public PreGameMusic preGameMusic;

    public static GameManager instance;

    private List<Weapon> weapons = new List<Weapon>();

    private AsyncOperationHandle<GameObject> bossHandle;

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

        yield return bossHandle;
    }

    public void SpawnBoss()
    {
        IBoss boss = Instantiate(bossesPrefabs[multiScenedata.bossId], bossSpawnPoint.position, Quaternion.identity).GetComponent<IBoss>();
        boss.difficultyMult = multiScenedata.mult;
        boss.aggroedPlayer = player;

        preGameMusic.Stop();

        //StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        var operation = Addressables.InstantiateAsync(bossesLabel[multiScenedata.bossId], bossSpawnPoint.position, Quaternion.identity);

        yield return operation;

        IBoss boss = operation.Result.GetComponent<IBoss>();
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

        int reward = Mathf.RoundToInt(multiScenedata.moneyReward * UnityEngine.Random.Range(0.8f, 1.2f));
        rewardText.text = reward.ToString();
        PlayerPrefs.SetInt("money", StaticData.money + reward);
        endCanvas.gameObject.SetActive(true);
    }
}
