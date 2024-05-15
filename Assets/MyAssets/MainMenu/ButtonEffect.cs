using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(Image), typeof(Button), typeof(AudioSource))]

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverColor;
    private Color defaultColor;

    private Image image;

    private AudioSource source;

    public AudioClip enterClip;
    public AudioClip clickClip;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.volume = 0.5f;

        image = GetComponent<Image>();
        defaultColor = image.color;

        GetComponent<Button>().onClick.AddListener(delegate { source.PlayOneShot(clickClip); });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        source.PlayOneShot(enterClip);

        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
    }

}
