using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
    public Health health;

    public AudioSource introSource;
    public AudioSource mainSource;

    public float delay = 0f;

    private IEnumerator Start()
    {
        health = GetComponentInParent<Health>();

        health.OnDeath += MusicFade;

        introSource.loop = false;
        mainSource.loop = true;

        AudioSource firstSource;
        float targetVolume = 0f;

        if (introSource.clip != null)
        {
            double duration = (double)introSource.clip.samples / introSource.clip.frequency;

            mainSource.PlayScheduled(duration + AudioSettings.dspTime);

            firstSource = introSource;
            targetVolume = introSource.volume;
            introSource.volume = 0f;
        }
        else
        {
            firstSource = mainSource;
            targetVolume = mainSource.volume;
            mainSource.volume = 0f;
            mainSource.Play();
        }

        Tween.AudioVolume(firstSource, targetVolume, 1);

        yield return null;
    }

    public void MusicFade()
    {
        if (introSource.isPlaying)
        {
            introSource.transform.SetParent(null);
            Tween.AudioVolume(introSource, 0, 2).OnComplete(() => Destroy(introSource.gameObject));
        }
        if (mainSource.isPlaying)
        {
            mainSource.transform.SetParent(null);
            Tween.AudioVolume(mainSource, 0, 2).OnComplete(() => Destroy(mainSource.gameObject));
        }
    }
}
