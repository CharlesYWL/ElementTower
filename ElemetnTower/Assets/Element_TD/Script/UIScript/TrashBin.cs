using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrashBin : MonoBehaviour,IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Sprite Close;
    public Sprite Open;
    private Image img;
    private BuildManager bm;

    // Start is called before the first frame update
    void Start()
    {
        bm = BuildManager.instance;
        img = gameObject.GetComponent<Image>();
        img.sprite = Close;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name+" Drop here");
        Draggable dg = eventData.pointerDrag.GetComponent<Draggable>();
        if (!bm.isChildOfShop(eventData.pointerDrag))
            dg.TowerSuccessCreate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.sprite = Open;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.sprite = Close;
    }
}
