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

    private float baseLevelCost = 20f;
    private float levelCostMult = 2f;

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
                playerData.hpLevel++;
                hpPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.hpLevel)), playerData.baseHp + (playerData.hpLevel * playerData.baseHpScale));
                MainMenuManager.instance.money -= hpPanel.cost;
                SaveData();
            }
        });

        moveSpeedPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.moveSpeedLevel)), playerData.baseMoveSpeed + (playerData.moveSpeedLevel * playerData.baseMoveSpeedScale));
        moveSpeedPanel.upgradeButton.onClick.AddListener(() =>
        {
            if (moveSpeedPanel.CanBuy(MainMenuManager.instance.money))
            {
                playerData.moveSpeedLevel++;
                moveSpeedPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.moveSpeedLevel)), playerData.baseMoveSpeed + (playerData.moveSpeedLevel * playerData.baseMoveSpeedScale));
                MainMenuManager.instance.money -= moveSpeedPanel.cost;
                SaveData();
            }
        });

        flyTimePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.flyTimeLevel)), playerData.baseFlyTime + (playerData.flyTimeLevel * playerData.baseFlyTimeScale));
        flyTimePanel.upgradeButton.onClick.AddListener(() =>
        {
            if (flyTimePanel.CanBuy(MainMenuManager.instance.money))
            {
                playerData.flyTimeLevel++;
                flyTimePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.flyTimeLevel)), playerData.baseFlyTime + (playerData.flyTimeLevel * playerData.baseFlyTimeScale));
                MainMenuManager.instance.money -= flyTimePanel.cost;
                SaveData();
            }
        });

        flyForcePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.flyForceLevel)), playerData.baseFlyForce + (playerData.flyForceLevel * playerData.baseFlyForceScale));
        flyForcePanel.upgradeButton.onClick.AddListener(() =>
        {
            if (flyForcePanel.CanBuy(MainMenuManager.instance.money))
            {
                playerData.flyForceLevel++;
                flyForcePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.flyForceLevel)), playerData.baseFlyForce + (playerData.flyForceLevel * playerData.baseFlyForceScale));
                MainMenuManager.instance.money -= flyForcePanel.cost;
                SaveData();
            }
        });

        dashCdPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.dashCdLevel)), playerData.baseDashCd - (playerData.dashCdLevel * playerData.baseDashCdScale));
        dashCdPanel.upgradeButton.onClick.AddListener(() =>
        {
            if (dashCdPanel.CanBuy(MainMenuManager.instance.money))
            {
                playerData.dashCdLevel++;
                dashCdPanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.dashCdLevel)), playerData.baseDashCd - (playerData.dashCdLevel * playerData.baseDashCdScale));
                MainMenuManager.instance.money -= dashCdPanel.cost;
                SaveData();
            }
        });

        dashForcePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.dashForceLevel)), playerData.baseDashForce + (playerData.dashForceLevel * playerData.baseDashForceScale));
        dashForcePanel.upgradeButton.onClick.AddListener(() =>
        {
            if (dashForcePanel.CanBuy(MainMenuManager.instance.money))
            {
                playerData.dashForceLevel++;
                dashForcePanel.SetValues(Mathf.RoundToInt(baseLevelCost * (levelCostMult * playerData.dashForceLevel)), playerData.baseDashForce + (playerData.dashForceLevel * playerData.baseDashForceScale));
                MainMenuManager.instance.money -= dashForcePanel.cost;
                SaveData();
            }
        });

        data.playerData = this.playerData;
    }

    private void SaveData()
    {
        dataService.SaveData("gg/pum", playerData);
        data.playerData = this.playerData;
    }
}
