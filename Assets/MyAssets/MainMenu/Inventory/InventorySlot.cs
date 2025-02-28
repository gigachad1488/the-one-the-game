using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == 0)
        {
            if (transform.childCount == 0)
            {
                GameObject droped = eventData.pointerDrag;
                DraggableItem item = droped.GetComponent<DraggableItem>();
                item.parentAfterDrag = transform;
            }
            else
            {
                GameObject droped = eventData.pointerDrag;
                DraggableItem droppedItem = droped.GetComponent<DraggableItem>();
                DraggableItem item = transform.GetChild(0).GetComponent<DraggableItem>();
                item.transform.SetParent(droppedItem.parentAfterDrag);
                item.parentAfterDrag = droppedItem.parentAfterDrag;
                item = droppedItem;
                item.parentAfterDrag = transform;
            }
        }
    }
}
