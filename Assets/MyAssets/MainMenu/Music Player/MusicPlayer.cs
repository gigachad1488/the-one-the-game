using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;

    public List<PlayListSO> playLists;

    private PlayListSO currentPlayList;
    private int currentSoundId;

    public bool repeat = false;
    public bool shuffle = false;
    private bool stoped = false;

    private Coroutine soundTick;

    private bool hidden = false;

    [Space(5)]
    [Header("UI")]
    [SerializeField]
    private Canvas musicPlayerCanvas;
    private CanvasGroup musicPlayerCanvasGroup;
    [SerializeField]
    private Button hidePanelButton;
    [SerializeField]
    private Button outsideHideButton;
    [SerializeField]
    private Button showPanelButton;
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI durationText;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button prevButton;
    [SerializeField]
    private Button playButton;
    private Image playButtonImage;
    [SerializeField]
    private Sprite playSprite;
    [SerializeField]
    private Sprite stopSprite;
    [SerializeField]
    private Button shuffleButton;
    private Image shuffleButtonImage;
    [SerializeField]
    private Button repeatButton;
    private Image repeatButtonImage;
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private Image volumeIcon;
    [SerializeField]
    private Color enabledColor = Color.white;
    [SerializeField]
    private Color disabledColor = new Color(255, 255, 255, 0.3f);

    private void Awake()
    {
        musicPlayerCanvasGroup = musicPlayerCanvas.GetComponent<CanvasGroup>();

        shuffleButtonImage = shuffleButton.GetComponent<Image>();
        repeatButtonImage = repeatButton.GetComponent<Image>();
        playButtonImage = playButton.GetComponent<Image>();

        musicPlayerCanvas.gameObject.SetActive(true);
    }

    private void Start()
    {
        currentSoundId = -1;

        if (playLists.Count > 0) 
        {
            currentPlayList = playLists[0]; 
        }
       
        nextButton.onClick.AddListener(NextSound);
        prevButton.onClick.AddListener(PrevSound);
        playButton.onClick.AddListener(OnPlayButtonPress);

        shuffleButton.onClick.AddListener(ShuffleSwitch);
        repeatButton.onClick.AddListener(RepeatSwitch);

        float volume = PlayerPrefs.GetFloat("musicPlayerVolume", 0.6f);
        OnVolumeSliderValueChange(volume);
        volumeSlider.value = volume;
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderValueChange);       

        hidePanelButton.onClick.AddListener(delegate { PanelVisibility(false); });
        outsideHideButton.onClick.AddListener(delegate { PanelVisibility(false); });
        showPanelButton.onClick.AddListener(PanelVisibility);

        OnPlayButtonPress(false);

        ShuffleSwitch(false);
        RepeatSwitch(false);

        PanelVisibility(false);

        StartCoroutine(StartSound(UnityEngine.Random.Range(0, currentPlayList.sounds.Count)));
    }

    private bool stopBuffer;

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            stopBuffer = stoped;
            stoped = pause;
        }
        else
        {
            stoped = stopBuffer;
        }

    }

    public void PanelVisibility()
    {
        PanelVisibility(!musicPlayerCanvasGroup.interactable);
    }

    public void PanelVisibility(bool value)
    {
        if (value)
        {
            musicPlayerCanvasGroup.alpha = 1;
            musicPlayerCanvasGroup.interactable = true;
            musicPlayerCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            musicPlayerCanvasGroup.alpha = 0;
            musicPlayerCanvasGroup.interactable = false;
            musicPlayerCanvasGroup.blocksRaycasts = false;
        }
    }

    public void NextSound()
    {       
        if (shuffle) 
        {
            int randomId;
            while (true)
            {
                randomId = UnityEngine.Random.Range(0, currentPlayList.sounds.Count);
                if (randomId != currentSoundId)
                {
                    break;
                }
            }

            StartCoroutine(StartSound(randomId));
            return;
        }

        if (currentSoundId + 1 >= currentPlayList.sounds.Count)
        {
            StartCoroutine(StartSound(0));
            return;
        }

        StartCoroutine(StartSound(currentSoundId + 1));
    }

    public void PrevSound()
    {
        if (shuffle)
        {
            int randomId;
            while (true)
            {
                randomId = UnityEngine.Random.Range(0, currentPlayList.sounds.Count);
                if (randomId != currentSoundId)
                {
                    break;
                }
            }

            StartCoroutine(StartSound(randomId));
            return;
        }

        if (currentSoundId - 1 < 0)
        {
            StartCoroutine(StartSound(currentPlayList.sounds.Count - 1));
        }

        StartCoroutine(StartSound(currentSoundId - 1));
    }

    public void AfterSound()
    {
        if (repeat)
        {
            StartCoroutine(StartSound(currentSoundId));
            return;
        }

        if (shuffle)
        {
            int randomId;
            while (true)
            {
                randomId = UnityEngine.Random.Range(0, currentPlayList.sounds.Count);
                if (randomId != currentSoundId)
                {
                    break;
                }
            }

            StartCoroutine(StartSound(randomId));
            return;
        }

        if (currentSoundId + 1 >= currentPlayList.sounds.Count)
        {
            StartCoroutine(StartSound(0));
        }

        StartCoroutine(StartSound(currentSoundId + 1));
    }

    public IEnumerator StartSound(int i)
    {
        if (soundTick != null)
        {
            StopCoroutine(soundTick);
        }

        if (currentSoundId != i)
        {         
            musicSource.clip = currentPlayList.sounds[i].clip;
            currentSoundId = i;
            double t = (double)musicSource.clip.samples / musicSource.clip.frequency;
            soundSlider.maxValue = (float)t;
            soundSlider.value = 0;
            durationText.text = TimeSpan.FromSeconds((float)musicSource.clip.length).ToString(@"mm\:ss");
            nameText.text = currentPlayList.sounds[i].name;
        }
        timeText.text = "00:00";
        musicSource.time = 0;

        if (!stoped)
        {
            musicSource.Play();
        }
        
        soundTick = StartCoroutine(SoundTick());
        yield return null;
    }

    public void OnPlayButtonPress()
    {
        stoped = !stoped;

        if (stoped)
        {
            PauseSound();
            playButtonImage.sprite = playSprite;
        }
        else
        {
            ResumeSound();
            playButtonImage.sprite = stopSprite;
        }
    }

    public void OnPlayButtonPress(bool value)
    {
        stoped = value;

        if (stoped)
        {
            PauseSound();
            playButtonImage.sprite = playSprite;
        }
        else
        {
            ResumeSound();
            playButtonImage.sprite = stopSprite;
        }
    }

    public void PauseSound()
    {
        musicSource.Pause();
        stoped = true;
    }

    public void ResumeSound()
    {
        musicSource.Play();
        stoped = false;
    }

    private IEnumerator SoundTick()
    {
        //double t = (double)musicSource.clip.samples / musicSource.clip.frequency;

        TimeSpan time;

        while (true) 
        {
            if (!stoped)
            {
                if (!musicSource.isPlaying)
                {
                    break;
                }

                time = TimeSpan.FromSeconds(musicSource.time);
                soundSlider.value = (float)time.TotalSeconds;
                timeText.text = time.ToString(@"mm\:ss");
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }

        AfterSound();

        yield return null;
    }

    public void OnSliderDrag()
    {
        if (soundSlider.value >= musicSource.clip.length)
        {
            musicSource.time = musicSource.clip.length;
        }
        else
        {
            musicSource.time = soundSlider.value;
        }

        timeText.text = TimeSpan.FromSeconds(musicSource.time).ToString(@"mm\:ss");
    }

    public void OnVolumeSliderValueChange(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("musicPlayerVolume", value);

        if (value <= 0)
        {
            volumeIcon.color = disabledColor;
        }
        else
        {
            volumeIcon.color = enabledColor;
        }
    }

    public void ShuffleSwitch()
    {
        shuffle = !shuffle;

        if (shuffle) 
        {
            shuffleButtonImage.color = enabledColor;
        }
        else
        {
            shuffleButtonImage.color = disabledColor;
        }
    }

    public void ShuffleSwitch(bool value)
    {
        shuffle = value;

        if (shuffle)
        {
            shuffleButtonImage.color = enabledColor;
        }
        else
        {
            shuffleButtonImage.color = disabledColor;
        }
    }

    public void RepeatSwitch()
    {
        repeat = !repeat;

        if (repeat)
        {
            repeatButtonImage.color = enabledColor;
        }
        else
        {
            repeatButtonImage.color = disabledColor;
        }
    }

    public void RepeatSwitch(bool value)
    {
        repeat = value;

        if (repeat)
        {
            repeatButtonImage.color = enabledColor;
        }
        else
        {
            repeatButtonImage.color = disabledColor;
        }
    }
}
