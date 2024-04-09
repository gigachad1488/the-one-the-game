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
    private Button playButton;
    [SerializeField]
    private Canvas mainCanvas;

    [Space(5)]
    [Header("Inventory")]
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
    private Button settingsButton;
    [SerializeField]
    private Canvas settingsCanvas;
    [SerializeField]
    private Button settingsBackButton;

    private void Start()
    {
        playButton.onClick.AddListener(StartGame);

        inventoryButton.onClick.AddListener(ShowInventory);
        inventoryBackButton.onClick.AddListener(ShowMain);

        shopButton.onClick.AddListener(ShowShop);  
        shopBackButton.onClick.AddListener(ShowMain);

        settingsButton.onClick.AddListener(ShowSettings);
        settingsBackButton.onClick.AddListener(ShowMain);

        ShowMain();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowMain()
    {
        mainCanvas.gameObject.SetActive(true);
        inventoryCanvas.gameObject.SetActive(false);
        shopCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);

    }

    public void ShowInventory()
    {
        mainCanvas.gameObject.SetActive(false);
        inventoryCanvas.gameObject.SetActive(true);
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

        slotsScrollRect.horizontalNormalizedPosition = 0;
        slotsScrollRect.verticalNormalizedPosition = 0;
    }
}
