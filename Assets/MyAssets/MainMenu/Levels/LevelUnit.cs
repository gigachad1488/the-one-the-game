using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnit : MonoBehaviour
{
    [Space(5)]
    [Header("Main")]
    public int bossId = 0;
    public int level = 1;
    public string bossName;
    public Sprite bossSprite;  
    
    public MainMenuManager mainMenuManager;

    [Serializable]
    public struct Difficulties
    {
        public string title;
        public float mult;
        public int minDrop;
        public int maxDrop;
    }

    [Space(5)]
    [Header("Difficulties")]

    public Difficulties[] difficulties;

    public Transform difficultyLayout;
    public DifficultyUnit difficultyUnitPrefab;

    [Space(5)]
    [Header("Details")]

    public Transform detailsTransform;
    public Vector3 detailsInitPosition;
    public Transform detailsDestination;

    private bool detailsShowed = false;

    [Space(5)]
    [Header("Preview")]

    public Transform previewTransform;
    public Button previevButton;
    public Image previewBossImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI bossNameText;
    public Vector3 previewInitPosition;
    public Transform previewDestination;   

    private void Start()
    {
        levelText.text = level.ToString();
        bossNameText.text = bossName;
        previewBossImage.sprite = bossSprite;
        previewBossImage.preserveAspect = true;

        detailsInitPosition = detailsTransform.localPosition;
        previewInitPosition = previewTransform.localPosition;

        previevButton.onClick.AddListener(ToggleDetails);

        foreach (var difficulty in difficulties) 
        {
            DifficultyUnit unit = Instantiate(difficultyUnitPrefab, difficultyLayout);
            unit.difficultyText.text = difficulty.title;
            unit.mult = difficulty.mult;
            unit.minDropCount = difficulty.minDrop;
            unit.maxDropCount = difficulty.maxDrop;
            unit.level = level;
            unit.bossId = bossId;
            unit.data = mainMenuManager.data;
        }
    }

    public void ToggleDetails()
    {
        if (detailsShowed) 
        {
            HideDetails();
        }
        else
        {
            ShowDetails();
        }
    }

    public void ShowDetails()
    {
        detailsShowed = true;

        Tween.StopAll(this);
        Sequence.Create()
            .Group(Tween.LocalPositionX(detailsTransform, detailsDestination.localPosition.x, 0.6f, Ease.InOutQuad))
            .Group(Tween.LocalPositionX(previewTransform, previewDestination.localPosition.x, 0.6f, Ease.InOutQuad));
    }

    public void HideDetails()
    {
        detailsShowed = false;

        Tween.StopAll(this);
        Sequence.Create()
            .Group(Tween.LocalPositionX(detailsTransform, detailsInitPosition.x, 0.6f, Ease.InOutQuad))
            .Group(Tween.LocalPositionX(previewTransform, previewInitPosition.x, 0.6f, Ease.InOutQuad));
    }
}
