using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameMusic : MonoBehaviour
{
    private AudioSource source;
    private void OnValidate()
    {
        source = GetComponent<AudioSource>();
    }

    public void Stop()
    {
        Tween.AudioVolume(source, 0, 1).OnComplete(this, x => Destroy(gameObject));
    }
}
