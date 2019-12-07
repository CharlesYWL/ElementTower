using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildPoint :  BuildInterface,IDropHandler
{
    public GameObject hint;
    private GameObject hintWehave;
    private MeshRenderer rend;
    private GameObject tower;
    private BuildManager bm;
    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        bm = BuildManager.instance;
    }

    private void OnMouseEnter()
    {

        //get hint for users
        if (hintWehave)
        {
            return;
        }
        hintWehave = Instantiate(hint, transform.position, transform.rotation);
        hintWehave.transform.Rotate(-90, 0, 0);
        hintWehave.transform.localScale = new Vector3(8, 8, 8);
    }

    private void OnMouseExit()
    {
        //TODO: now its just to show we enter the box
        Destroy(hintWehave);
    }

    // This function deal with card dropped here
    // BUG: Cannot be triggered
    public void OnDrop(PointerEventData eventData)
    {
        //Build new tower
        if (tower != null)
        {
            Debug.Log("Cant build there");
            //TODO: Display on UI
            return;
        }
        //Build a tower and Destory itself
        Debug.Log("eventData is: " + eventData.pointerDrag.name);
        Debug.Log("isChildOfShop? " + (bm.isChildOfShop(eventData.pointerDrag.gameObject)));
        if (!(bm.isChildOfShop(eventData.pointerDrag.gameObject)))
        {
            //Debug.Log("We drag a shopcard on buildpoints");
            eventData.pointerDrag.GetComponent<Draggable>().TowerSuccessCreate();
            GameObject towerToBuild = eventData.pointerDrag.GetComponent<Draggable>().GetTower();
            tower = Instantiate(towerToBuild, transform.position, transform.rotation);
            Destroy(hintWehave);
            Destroy(gameObject);
        }

    }

}
