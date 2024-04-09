using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject droped = eventData.pointerDrag;
            DraggableItem draggableItem = droped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
        }             
    }
}
