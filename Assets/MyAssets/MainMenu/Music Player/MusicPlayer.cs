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

    [Space(5)]
    [Header("UI")]
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

    private void Start()
    {
        currentSoundId = -1;

        if (playLists.Count > 0) 
        {
            currentPlayList = playLists[0]; 
        }

        NextSound();

        nextButton.onClick.AddListener(NextSound);
        prevButton.onClick.AddListener(PrevSound);
    }

    public void NextSound()
    {       
        if (shuffle) 
        {
            int randomId = UnityEngine.Random.Range(0, currentPlayList.sounds.Count);
            StartCoroutine(StartSound(randomId));
            return;
        }

        if (currentSoundId + 1 >= currentPlayList.sounds.Count)
        {
            StartCoroutine(StartSound(0));
        }

        StartCoroutine(StartSound(currentSoundId + 1));
    }

    public void PrevSound()
    {
        if (shuffle)
        {
            int randomId = UnityEngine.Random.Range(0, currentPlayList.sounds.Count);
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
            int randomId = UnityEngine.Random.Range(0, currentPlayList.sounds.Count);
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

        musicSource.time = 0;
        musicSource.Play();     
        
        soundTick = StartCoroutine(SoundTick());
        yield return null;
    }

    public void StopSound()
    {
        musicSource.Stop();
        stoped = true;
    }

    private IEnumerator SoundTick()
    {
        double t = (double)musicSource.clip.samples / musicSource.clip.frequency;

        Debug.Log("T = " + t + " LENGG = " + musicSource.clip.length);
        TimeSpan time;

        while (musicSource.isPlaying) 
        {
            if (!stoped)
            {
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

        Debug.Log("HES HERE");
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

        Debug.Log("SLIDA VALU = " + soundSlider.value);
    }
}
