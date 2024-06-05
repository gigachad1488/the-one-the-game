    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradePanel : MonoBehaviour
{
    public Button upgradeButton;
    private Image buttonImage;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI valueText;

    private Color baseColor;

    public int cost = 1;

    private void OnValidate()
    {
        if (upgradeButton != null)
        {
            buttonImage = upgradeButton.GetComponent<Image>();
            baseColor = buttonImage.color;
        }
    }

    public void SetValues(int cost, float value)
    {
        this.cost = cost;
        upgradeCostText.text = cost.ToString();
        valueText.text = value.ToString();
    }

    public bool CanBuy(int money)
    {
        StopAllCoroutines();

        if (money >= cost)
        {
            StartCoroutine(ColorChange(Color.green));
            return true;
        }
        else
        {
            StartCoroutine(ColorChange(Color.red));
            return false;
        }
    }

    private IEnumerator ColorChange(Color color)
    {
        buttonImage.color = color;      

        yield return new WaitForSeconds(0.2f);

        buttonImage.color = baseColor;
    }
}
