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

        double duration = (double)introSource.clip.samples / introSource.clip.frequency;

        mainSource.PlayScheduled(duration + AudioSettings.dspTime);

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
