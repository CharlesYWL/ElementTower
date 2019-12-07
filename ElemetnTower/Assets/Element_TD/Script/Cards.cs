using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is to main cards in hand
/// </summary>
public class Cards : MonoBehaviour
{
    BuildManager buildManager;
    // Start is called before the first frame update
    public GameObject TestTarget;
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
        Debug.Log("Add prefeb");
        GameObject go;
        go = Instantiate(prefeb, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
    }
}
