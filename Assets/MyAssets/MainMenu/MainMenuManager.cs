using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Space(5)]
    [Header("Main")]
    [SerializeField]
    private Canvas mainCanvas;
    private CanvasGroup mainCanvasGroup;
    [SerializeField]
    private Button quitButton;

    [Space(5)]
    [Header("Inventory")]
    public InventoryManager inventoryManager;
    [SerializeField]
    private Button inventoryButton;
    [SerializeField]
    private Canvas inventoryCanvas;
    private CanvasGroup inventoryCanvasGroup;
    [SerializeField]
    private Button inventoryBackButton;
    [SerializeField]
    private ScrollRect slotsScrollRect;

    [Space(5)]
    [Header("Player")]
    [SerializeField]
    private PlayerUpgradeManager playerUpgradeManager;
    [SerializeField]
    private Button playerButton;   
    [SerializeField]
    private Canvas playerCanvas;
    private CanvasGroup playerCanvasGroup;
    [SerializeField]
    private Button playerBackButton;

    [Space(5)]
    [Header("Settings")]
    [SerializeField]
    private SettingsManager settingsManager;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Canvas settingsCanvas;
    private CanvasGroup settingsCanvasGroup;
    [SerializeField]
    private Button settingsBackButton;

    [Space(5)]
    [Header("Levels")]
    [SerializeField]
    private Button levelsButton;
    [SerializeField]
    private Canvas levelsCanvas;
    private CanvasGroup levelsCanvasGroup;
    [SerializeField]
    private Button levelsBackButton;

    public MultiSceneData data;

    public static MainMenuManager instance;

    public TextMeshProUGUI moneyText;

    public int money
    {
        get
        {
            return StaticData.money;
        }
        set
        {
            StaticData.money = value;
            moneyText.text = StaticData.money.ToString();
        }
    }

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }

        money = PlayerPrefs.GetInt("money", 0);

        data = GameObject.FindGameObjectWithTag("MultiScene").GetComponent<MultiSceneData>();
        
        inventoryManager.multiSceneData = data;

        playerUpgradeManager.Init(data);

        inventoryCanvasGroup = inventoryCanvas.GetComponent<CanvasGroup>();
        mainCanvasGroup = mainCanvas.GetComponent<CanvasGroup>();
        settingsCanvasGroup = settingsCanvas.GetComponent<CanvasGroup>();
        playerCanvasGroup = playerCanvas.GetComponent<CanvasGroup>();
        levelsCanvasGroup = levelsCanvas.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Time.timeScale = 1;

        mainCanvas.gameObject.SetActive(true);
        inventoryCanvas.gameObject.SetActive(true);
        playerCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(true);
        levelsCanvas.gameObject.SetActive(true);

        inventoryButton.onClick.AddListener(ShowInventory);
        inventoryBackButton.onClick.AddListener(ShowMain);

        playerButton.onClick.AddListener(ShowShop);  
        playerBackButton.onClick.AddListener(ShowMain);

        settingsButton.onClick.AddListener(ShowSettings);
        settingsBackButton.onClick.AddListener(delegate { ShowMain(); settingsManager.ChangeTab(0); });

        levelsButton.onClick.AddListener(ShowLevels);
        levelsBackButton.onClick.AddListener(ShowMain);

        quitButton.onClick.AddListener(() => Application.Quit());

        ShowMain();
    }

    public void ShowMain()
    {
        mainCanvasGroup.alpha = 1;
        mainCanvasGroup.interactable = true;
        mainCanvasGroup.blocksRaycasts = true;

        inventoryCanvasGroup.alpha = 0;
        inventoryCanvasGroup.interactable = false;
        inventoryCanvasGroup.blocksRaycasts = false;

        playerCanvasGroup.alpha = 0;
        playerCanvasGroup.interactable = false;
        playerCanvasGroup.blocksRaycasts = false;

        settingsCanvasGroup.alpha = 0;
        settingsCanvasGroup.interactable = false;
        settingsCanvasGroup.blocksRaycasts = false;

        levelsCanvasGroup.alpha = 0;
        levelsCanvasGroup.interactable = false;
        levelsCanvasGroup.blocksRaycasts = false;
    }

    public void ShowInventory()
    {
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        inventoryCanvasGroup.alpha = 1;
        inventoryCanvasGroup.interactable = true;
        inventoryCanvasGroup.blocksRaycasts = true;

        slotsScrollRect.horizontalNormalizedPosition = 0;
        slotsScrollRect.verticalNormalizedPosition = 0;
    }

    public void ShowShop()
    {
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        playerCanvasGroup.alpha = 1;
        playerCanvasGroup.interactable = true;
        playerCanvasGroup.blocksRaycasts = true;
    }

    public void ShowSettings()
    {
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        settingsCanvasGroup.alpha = 1;
        settingsCanvasGroup.interactable = true;
        settingsCanvasGroup.blocksRaycasts = true;
    }

    public void ShowLevels()
    {
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        levelsCanvasGroup.alpha = 1;
        levelsCanvasGroup.interactable = true;
        levelsCanvasGroup.blocksRaycasts = true;
    }
}
