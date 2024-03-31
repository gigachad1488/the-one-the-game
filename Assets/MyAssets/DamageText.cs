using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;
    public float damage = 0f;
    public float mult = 1f;
    public float duration = 1f;

    private void Start()
    {
        Tween.PositionY(transform, transform.position.y + 2, duration).OnComplete(this, x => Destroy(gameObject));
        text.text = damage.ToString();

        if (mult >= 2)
        {
            text.color = Color.red;
        }
    }
}
