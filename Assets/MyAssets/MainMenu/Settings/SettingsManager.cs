using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Space(3)]
    [Header("Main Tab")]
    public Button mainTabButton;
    private Image mainTabButtonImage;
    public Image mainTab;

    public Slider mainVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public AudioMixer mixer;

    [Space(3)]
    [Header("Graphics Tab")]
    public Button graphicsTabButton;
    private Image graphicsTabButtonImage;
    public Image graphicsTab;

    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown screenModeDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle vSyncToggle;
    public TMP_InputField fpsLimitInputField;

    public Resolution[] resolutions;

    public Transform fpsLimitPanel;

    [Space(3)]
    [Header("Controls Tab")]
    public Button controlsTabButton;
    private Image controlsTabButtonImage;
    public Image controlsTab;
    public Button resetAllBindingsButton;

    public KeySettingsItem[] bindingItems;

    public InputActionAsset actions;

    [Space(5)]
    [Header("Tabs")]
    private Image[] tabs = new Image[3];
    private Button[] tabButtons = new Button[3];
    private Image[] tabButtonsImage = new Image[3];

    private void Awake()
    {
        mainTabButtonImage = mainTabButton.GetComponent<Image>();
        graphicsTabButtonImage = graphicsTabButton.GetComponent<Image>();
        controlsTabButtonImage = controlsTabButton.GetComponent<Image>();

        string actionsJson = PlayerPrefs.GetString("actions", "");
        if (!string.IsNullOrEmpty(actionsJson))
        {
            actions.LoadBindingOverridesFromJson(actionsJson);
        }
    }

    private void Start()
    {
        InitMain();
        InitGraphics();
        InitControls();

        ChangeTab(0);
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

    #region Main tab

    public void InitMain()
    {
        tabs[0] = mainTab;
        tabButtons[0] = mainTabButton;
        tabButtonsImage[0] = mainTabButtonImage;
        mainTabButton.onClick.AddListener(delegate { ChangeTab(0); });

        mainVolumeSlider.onValueChanged.AddListener(ChangeMainVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSfxVolume);

        mainVolumeSlider.value = PlayerPrefs.GetFloat("mainVolume", 1);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);
    }

    public void ChangeMainVolume(float value)
    {
        mixer.SetFloat("Main", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("mainVolume", value);
    }
    public void ChangeMusicVolume(float value)
    {
        mixer.SetFloat("Music", Mathf.Log10(value) * 20u);
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    public void ChangeSfxVolume(float value)
    {
        mixer.SetFloat("Sfx", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("sfxVolume", value);
    }

    #endregion

    #region Graphics tab

    public void InitGraphics()
    {
        resolutions = Screen.resolutions;

        int currentResolutionIndex = Array.FindIndex(resolutions, x => x.height == Screen.currentResolution.height && x.width == Screen.currentResolution.width);

        resolutionDropdown.ClearOptions();

        foreach (var resolution in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = resolution.ToString() });
        }
        resolutionDropdown.value = currentResolutionIndex;

        if (QualitySettings.vSyncCount > 0)
        {
            fpsLimitPanel.gameObject.SetActive(false);
        }
        else
        {
            vSyncToggle.isOn = false;
        }

        qualityDropdown.value = QualitySettings.GetQualityLevel();

        tabs[1] = graphicsTab;
        tabButtons[1] = graphicsTabButton;
        tabButtonsImage[1] = graphicsTabButtonImage;
        graphicsTabButton.onClick.AddListener(delegate { ChangeTab(1); });

        qualityDropdown.onValueChanged.AddListener(ChangeGraphicsPreset);
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        vSyncToggle.onValueChanged.AddListener(ChangeVSync);
        screenModeDropdown.onValueChanged.AddListener(ChangeScreenMode);
        fpsLimitInputField.onEndEdit.AddListener(ChangeFpsLimit);

        fpsLimitInputField.text = Application.targetFrameRate.ToString();
    }

    public void ChangeGraphicsPreset(int i)
    {
        QualitySettings.SetQualityLevel(i);
    }

    public void ChangeScreenMode(int i)
    {
        switch (i)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void ChangeVSync(bool vsync)
    {
        if (vsync)
        {
            QualitySettings.vSyncCount = 1;
            fpsLimitPanel.gameObject.SetActive(false);
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            fpsLimitPanel.gameObject.SetActive(true);
            fpsLimitInputField.text = Application.targetFrameRate.ToString();
        }
    }

    public void ChangeFpsLimit(string limit)
    {
        if (Int32.TryParse(limit, out int fps))
        {
            Application.targetFrameRate = fps;
        }
        else
        {
            Application.targetFrameRate = 0;
            fpsLimitInputField.text = "0";
        }
    }

    public void ChangeResolution(int i)
    {
        Screen.SetResolution(resolutions[i].width, resolutions[i].height, Screen.fullScreenMode);
    }

    #endregion

    #region Controls

    public void InitControls()
    {
        tabs[2] = controlsTab;
        tabButtons[2] = controlsTabButton;
        tabButtonsImage[2] = controlsTabButtonImage;
        controlsTabButton.onClick.AddListener(delegate { ChangeTab(2); });
        resetAllBindingsButton.onClick.AddListener(ResetAllBindings);
    }

    public void ResetAllBindings()
    {
        foreach (KeySettingsItem item in bindingItems)
        {
            item.ResetKey();
        }
    }

    #endregion
}
