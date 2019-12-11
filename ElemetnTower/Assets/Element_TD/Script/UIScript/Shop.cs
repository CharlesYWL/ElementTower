using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class is to maintain shop
/// </summary>
public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    List<GameObject> CardArray;
    //private GameObject[] CardArray;
    private int MAXCAP = 5;
    private int childCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //CardArray = new GameObject[MAXCAP];
        CardArray = new List<GameObject>();
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is for test use
    public void addPrefeb(GameObject prefeb)
    {
        GameObject go;
        go = Instantiate(prefeb, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        childCount++;
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
        CardArray.Clear();

    }

    public void openShop()
    {
        foreach (GameObject child in CardArray)
        {
            child.SetActive(true);
        }
        CardArray.Clear();
    }

    public void closeShop()
    {
        int childs = this.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            CardArray.Add(this.transform.GetChild(i).gameObject);
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public bool isFullCap() 
    {
        return (this.childCount >= MAXCAP);
    }


}
