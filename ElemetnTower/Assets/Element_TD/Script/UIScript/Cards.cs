using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is to main cards in hand
/// </summary>
public class Cards : MonoBehaviour
{
    BuildManager bm;
    // Start is called before the first frame update
    private int MAXCAP = 8;
    void Start()
    {
        bm = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is for test use
    public void addPrefeb(GameObject prefeb)
    {
        Debug.Log("Add prefeb");
        GameObject go;
        go = Instantiate(prefeb, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
    }
    public int getCardsNumber()
    {
        return transform.childCount;
    }

    //Return is Slot Aviailable
    public bool SlotAvialiable()
    {
        if (transform.childCount >= MAXCAP)
        {
            //Do Something to warn Players
        }
        return (transform.childCount<MAXCAP);
    }
}
