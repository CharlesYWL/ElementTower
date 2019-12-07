using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is to maintain shop
/// </summary>
public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    private int MAXCAP = 5;
    private int childCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is for test use
    public void addPrefeb(GameObject prefeb)
    {
        childCount++;
        Debug.Log("Add prefeb");
        GameObject go;
        go = Instantiate(prefeb, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
    }
    public int getCardsNumber()
    {
        Debug.Log(transform.childCount);
        return transform.childCount;
    }
    public void clearShop()
    {
        int childs = this.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            //GameObject.DestroyImmediate();
            Destroy(this.transform.GetChild(i).gameObject);
        }
        this.childCount = 0;
    }

    public bool isFullCap() 
    {
        Debug.Log("===>" + this.childCount);
        return (this.childCount >= MAXCAP);
    }


}
