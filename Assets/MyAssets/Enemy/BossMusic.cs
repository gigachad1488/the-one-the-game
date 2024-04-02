using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
    public AudioSource introSource;
    public AudioSource mainSource;

    public float delay = 0f;

    private IEnumerator Start()
    {
        introSource.loop = false;
        mainSource.loop = true;

        double duration = (double)introSource.clip.samples / introSource.clip.frequency;

        yield return new WaitForSeconds(delay);

        mainSource.PlayScheduled(duration + AudioSettings.dspTime);
    }
}
