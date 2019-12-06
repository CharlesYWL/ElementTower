using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPoint :  BuildInterface
{
    public Color hoverColor;
    public GameObject hint;
    private GameObject hintWehave;
    private MeshRenderer rend;
    private GameObject tower;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    private void OnMouseDown()
    {
        if (tower != null) {
            Debug.Log("Cant build there");
            //TODO: Display on UI
            return;
        }
        //Build a tower and Destory itself
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        tower = Instantiate(towerToBuild, transform.position, transform.rotation);
        Destroy(hintWehave);
        Destroy(gameObject);
    }
    private void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
        //TODO: get hint for users
        hintWehave = Instantiate(hint, transform.position, transform.rotation);
        hintWehave.transform.localScale = new Vector3(2,2,2);
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        //TODO: now its just to show we enter the box
        Destroy(hintWehave);
    }

     
}
