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
    public bool UpgradeTower(GameObject tower)
    {
        int find = 0;
        List<int> index = new List<int>();

        for (int i = 0; i < getCardsNumber(); i++)
        {
            if (TowerLabel.Compare(tower, transform.GetChild(i).gameObject))
            {// if they have same label
                find++;
                index.Add(i);
            }
        }
        if (find < 2)
        {
            return false;
        }
        else
        {
            Destroy(transform.GetChild(index[1]).gameObject);
            Destroy(transform.GetChild(index[0]).gameObject);
            return true;
        }

    }
}
