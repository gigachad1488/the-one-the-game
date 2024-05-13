using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image image;
    [HideInInspector]
    public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == 0)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == 0)
            transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == 0)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }
}
