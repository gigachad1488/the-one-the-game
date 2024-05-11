using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Space(5)]
    [Header("Main")]
    [SerializeField]
    private Canvas mainCanvas;

    [Space(5)]
    [Header("Inventory")]
    public InventoryManager inventoryManager;
    [SerializeField]
    private Button inventoryButton;
    [SerializeField]
    private Canvas inventoryCanvas;
    [SerializeField]
    private Button inventoryBackButton;
    [SerializeField]
    private ScrollRect slotsScrollRect;

    [Space(5)]
    [Header("Shop")]
    [SerializeField]
    private Button shopButton;
    [SerializeField]
    private Canvas shopCanvas;
    [SerializeField]
    private Button shopBackButton;

    [Space(5)]
    [Header("Settings")]
    [SerializeField]
    private SettingsManager settingsManager;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Canvas settingsCanvas;
    [SerializeField]
    private Button settingsBackButton;

    [Space(5)]
    [Header("Levels")]
    [SerializeField]
    private Button levelsButton;
    [SerializeField]
    private Canvas levelsCanvas;
    [SerializeField]
    private Button levelsBackButton;

    public MultiSceneData data;

    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }

        data = GameObject.FindGameObjectWithTag("MultiScene").GetComponent<MultiSceneData>();
        
        inventoryManager.multiSceneData = data;
    }

    private void Start()
    {
        Time.timeScale = 1;

        inventoryButton.onClick.AddListener(ShowInventory);
        inventoryBackButton.onClick.AddListener(ShowMain);

        shopButton.onClick.AddListener(ShowShop);  
        shopBackButton.onClick.AddListener(ShowMain);

        settingsButton.onClick.AddListener(ShowSettings);
        settingsBackButton.onClick.AddListener(delegate { ShowMain(); settingsManager.ChangeTab(0); });

        levelsButton.onClick.AddListener(ShowLevels);
        levelsBackButton.onClick.AddListener(ShowMain);

        ShowMain();
    }

    public void ShowMain()
    {
        mainCanvas.gameObject.SetActive(true);
        inventoryCanvas.gameObject.SetActive(false);
        shopCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
        levelsCanvas.gameObject.SetActive(false);
    }

    public void ShowInventory()
    {
        mainCanvas.gameObject.SetActive(false);
        inventoryCanvas.gameObject.SetActive(true);

        slotsScrollRect.horizontalNormalizedPosition = 0;
        slotsScrollRect.verticalNormalizedPosition = 0;
    }

    public void ShowShop()
    {
        mainCanvas.gameObject.SetActive(false);
        shopCanvas.gameObject.SetActive(true);
    }

    public void ShowSettings()
    {
        mainCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);     
    }

    public void ShowLevels()
    {
        mainCanvas.gameObject.SetActive(false);
        levelsCanvas.gameObject.SetActive(true);
    }
}
