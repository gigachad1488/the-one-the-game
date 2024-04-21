using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Space(3)]
    [Header("Main Tab")]
    public Button mainTabButton;
    private Image mainTabButtonImage;
    public Image mainTab;
    [Space(3)]
    [Header("Graphics Tab")]
    public Button graphicsTabButton;
    private Image graphicsTabButtonImage;
    public Image graphicsTab;
    [Space(3)]
    [Header("Controls Tab")]
    public Button controlsTabButton;
    private Image controlsTabButtonImage;
    public Image controlsTab;

    private Image[] tabs = new Image[3];
    private Button[] tabButtons = new Button[3];
    private Image[] tabButtonsImage = new Image[3];

    private void Awake()
    {
        mainTabButtonImage = mainTabButton.GetComponent<Image>();
        graphicsTabButtonImage = graphicsTabButton.GetComponent<Image>();
        controlsTabButtonImage = controlsTabButton.GetComponent<Image>();
    }

    private void Start()
    {
        tabs[0] = mainTab;
        tabButtons[0] = mainTabButton;
        tabButtonsImage[0] = mainTabButtonImage;
        mainTabButton.onClick.AddListener(delegate { ChangeTab(0); });

        tabs[1] = graphicsTab;
        tabButtons[1] = graphicsTabButton;
        tabButtonsImage[1] = graphicsTabButtonImage;
        graphicsTabButton.onClick.AddListener(delegate { ChangeTab(1); });

        tabs[2] = controlsTab;
        tabButtons[2] = controlsTabButton;
        tabButtonsImage[2] = controlsTabButtonImage;
        controlsTabButton.onClick.AddListener(delegate { ChangeTab(2); });
    }

    public void ChangeTab(int id)
    {
        for (int i = 0; i < tabButtons.Length; i++) 
        {
            if (i == id)
            {
                tabs[i].gameObject.SetActive(true);
                tabButtonsImage[i].color = Color.gray;
            }
            else
            {
                tabs[i].gameObject.SetActive(false);
                tabButtonsImage[i].color = Color.white;
            }
        }
    }
}
