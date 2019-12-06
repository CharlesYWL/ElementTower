using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private Vector2 offSet;
    Transform parentToReturnTo = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On Begin Drag");
        parentToReturnTo = transform.parent;
        offSet = eventData.position - new Vector2(transform.position.x, transform.position.y);
        transform.SetParent(transform.parent.parent);

        //This make the card no long block cursor message
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position-offSet;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("On End Drag");
        transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }
}
