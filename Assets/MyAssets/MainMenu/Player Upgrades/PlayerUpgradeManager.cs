using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    private MultiSceneData data;

    [Header("StatsPanels")]
    public PlayerUpgradePanel hpPanel;
    public PlayerUpgradePanel moveSpeedPanel;
    public PlayerUpgradePanel flyTimePanel;
    public PlayerUpgradePanel flyForcePanel;
    public PlayerUpgradePanel dashCdPanel;
    public PlayerUpgradePanel dashForcePanel;

    private float baseLevelCost = 10f;
    private float levelCostMult = 1.8f;

    private JsonDataService dataService;
    private PlayerUpgradesData playerData;

    public void Init(MultiSceneData data)
    {
        dataService = new();
        playerData = new();

        this.data = data;

        if (!File.Exists(Application.persistentDataPath + "/gg/pum"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/gg");
            SaveData();
        }
        else
        {
            playerData = dataService.LoadData<PlayerUpgradesData>("gg/pum");
        }

        hpPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.hpLevel)), playerData.baseHp + (playerData.hpLevel * playerData.baseHpScale));
        hpPanel.upgradeButton.onClick.AddListener(() =>
        {
            if (hpPanel.CanBuy(MainMenuManager.instance.money))
            {
                MainMenuManager.instance.money -= hpPanel.cost;
                playerData.hpLevel++;
                hpPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.hpLevel)), playerData.baseHp + (playerData.hpLevel * playerData.baseHpScale));
                SaveData();
                UpdateAllAvaibility();
            }
        });

        moveSpeedPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.moveSpeedLevel)), playerData.baseMoveSpeed + (playerData.moveSpeedLevel * playerData.baseMoveSpeedScale));
        moveSpeedPanel.upgradeButton.onClick.AddListener(() =>
        {
            if (moveSpeedPanel.CanBuy(MainMenuManager.instance.money))
            {
                MainMenuManager.instance.money -= moveSpeedPanel.cost;
                playerData.moveSpeedLevel++;
                moveSpeedPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.moveSpeedLevel)), playerData.baseMoveSpeed + (playerData.moveSpeedLevel * playerData.baseMoveSpeedScale));
                SaveData();
                UpdateAllAvaibility();
            }
        });

        flyTimePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.flyTimeLevel)), playerData.baseFlyTime + (playerData.flyTimeLevel * playerData.baseFlyTimeScale));
        flyTimePanel.upgradeButton.onClick.AddListener(() =>
        {
            if (flyTimePanel.CanBuy(MainMenuManager.instance.money))
            {
                MainMenuManager.instance.money -= flyTimePanel.cost;
                playerData.flyTimeLevel++;
                flyTimePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.flyTimeLevel)), playerData.baseFlyTime + (playerData.flyTimeLevel * playerData.baseFlyTimeScale));
                SaveData();
                UpdateAllAvaibility();
            }
        });

        flyForcePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.flyForceLevel)), playerData.baseFlyForce + (playerData.flyForceLevel * playerData.baseFlyForceScale));
        flyForcePanel.upgradeButton.onClick.AddListener(() =>
        {
            if (flyForcePanel.CanBuy(MainMenuManager.instance.money))
            {
                MainMenuManager.instance.money -= flyForcePanel.cost;
                playerData.flyForceLevel++;
                flyForcePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.flyForceLevel)), playerData.baseFlyForce + (playerData.flyForceLevel * playerData.baseFlyForceScale));
                SaveData();
                UpdateAllAvaibility();
            }
        });

        dashCdPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.dashCdLevel)), playerData.baseDashCd - (playerData.dashCdLevel * playerData.baseDashCdScale));
        dashCdPanel.upgradeButton.onClick.AddListener(() =>
        {
            if (dashCdPanel.CanBuy(MainMenuManager.instance.money))
            {
                MainMenuManager.instance.money -= dashCdPanel.cost;
                playerData.dashCdLevel++;
                dashCdPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.dashCdLevel)), playerData.baseDashCd - (playerData.dashCdLevel * playerData.baseDashCdScale));
                SaveData();
                UpdateAllAvaibility();
            }
        });

        dashForcePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.dashForceLevel)), playerData.baseDashForce + (playerData.dashForceLevel * playerData.baseDashForceScale));
        dashForcePanel.upgradeButton.onClick.AddListener(() =>
        {
            if (dashForcePanel.CanBuy(MainMenuManager.instance.money))
            {
                MainMenuManager.instance.money -= dashForcePanel.cost;
                playerData.dashForceLevel++;
                dashForcePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.dashForceLevel)), playerData.baseDashForce + (playerData.dashForceLevel * playerData.baseDashForceScale));
                SaveData();
                UpdateAllAvaibility();
            }
        });

        data.playerData = this.playerData;

        UpdateAllAvaibility();
    }

    private void UpdateAllAvaibility()
    {
        hpPanel.UpdateAvaibility();
        moveSpeedPanel.UpdateAvaibility();
        flyTimePanel.UpdateAvaibility();
        flyForcePanel.UpdateAvaibility();
        dashCdPanel.UpdateAvaibility();
        dashForcePanel.UpdateAvaibility();
    }

    private void SaveData()
    {
        dataService.SaveData("gg/pum", playerData);
        data.playerData = this.playerData;
        PlayerPrefs.SetInt("money", StaticData.money);
    }
}
