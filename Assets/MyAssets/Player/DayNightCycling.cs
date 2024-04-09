using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycling : MonoBehaviour
{
    public float cycleTime = 20f;

    public Transform sun;
    public Transform moon;
    public Material skyboxMaterial;
    public Light2D globalLight;

    public Transform startPos;
    public Transform endPos;

    private Sequence sequence;

    public float startPoint = 0;

    private void Start()
    {
        sun.localPosition = startPos.localPosition;
        moon.localPosition = startPos.localPosition;

        sequence = Sequence.Create(-1);
        sequence.Group(Tween.Custom(1, 0, cycleTime * 0.1f, x => skyboxMaterial.SetFloat("_DayNightSlider", x)))
        .Group(Tween.Custom(0.2f, 0.6f, cycleTime * 0.1f, x => globalLight.intensity = x, Ease.Linear))
        .Group(Tween.LocalPosition(sun, startPos.localPosition, endPos.localPosition, cycleTime).OnComplete(this, x =>
        {
            sun.gameObject.SetActive(false);
            moon.gameObject.SetActive(true);
        })
        .Chain(Tween.Custom(0, 1, cycleTime * 0.1f, x => skyboxMaterial.SetFloat("_DayNightSlider", x)))
        .Group(Tween.Custom(0.6f, 0.2f, cycleTime * 0.1f, x => globalLight.intensity = x, Ease.Linear))
        .Group(Tween.LocalPosition(moon, startPos.localPosition, endPos.localPosition, cycleTime).OnComplete(this, x =>
        {
            sun.gameObject.SetActive(true);
            moon.gameObject.SetActive(false);
        })));

        sequence.progress = startPoint;
    }
}
