using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image borderImage;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI dropText;

    public float mult = 1;
    public float minDropCount = 1;
    public float maxDropCount = 2;

    public MultiSceneData data;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartGame);

        borderImage.fillAmount = 0;

        dropText.text = string.Join('-', minDropCount, maxDropCount);
    }

    public void StartGame()
    {
        MainMenuManager.instance.inventoryManager.SaveSelectedWeapons();

        data.mult = mult;
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tween.StopAll(borderImage);
        Tween.UIFillAmount(borderImage, 1, 0.4f, Ease.OutBounce);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tween.StopAll(borderImage);
        Tween.UIFillAmount(borderImage, 0, 0.2f);
    }
}
