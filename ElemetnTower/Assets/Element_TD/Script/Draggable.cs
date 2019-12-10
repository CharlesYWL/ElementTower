using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler, IPointerClickHandler
{
    [Header("Property")]
    public GameObject Tower;

    [Header("Dont Thouch Below")]
    private Vector2 offSet;
    public Transform parentToReturnTo = null;
    private bool TowerCreate = false;
    BuildManager bm;

    GameObject placeHolder=null;

    void Start()
    {
        //init all thing
        parentToReturnTo = transform.parent;
        bm = BuildManager.instance;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //PlaceHoler thing
        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        //Drag thing
        parentToReturnTo = transform.parent;
        offSet = eventData.position - new Vector2(transform.position.x, transform.position.y);
        transform.SetParent(transform.parent.parent);

        //This make the card no long block cursor message
        GetComponent<CanvasGroup>().blocksRaycasts = false;


    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move
        transform.position = eventData.position-offSet;
        //PlaceHoler thing
        int newSiblingIndex = parentToReturnTo.childCount;
        for (int i = 0; i < parentToReturnTo.childCount; i++)
        {
            if (transform.position.x < parentToReturnTo.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--; 
                break;
            }
        }
        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentToReturnTo);
        transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());


        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (TowerCreate)
        {
            Destroy(this.gameObject);
        }
        Destroy(placeHolder);
    }

    // this is used for genetae appropriate tower
    public GameObject GetTower()
    {
        return Tower;
    }

    // this is to set TowerCreate true and to destory itself
    public void TowerSuccessCreate()
    {
        TowerCreate = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Update parent everytime of event
        parentToReturnTo = transform.parent;
        if (bm.isChildOfShop(this.gameObject))
        {
            float money = bm.Money;
            float towermoney = this.gameObject.GetComponent<CardInfoShow>().CardMoney;
            if (money < towermoney)
            {
                return;
            }
            bm.Money -= towermoney;
        }
        this.gameObject.GetComponent<CardInfoShow>().MoneyText.enabled = false;
        bm.CardClicked(this.gameObject);
    }

}
