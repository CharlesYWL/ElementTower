using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpecial : MonoBehaviour
{
    Tower towers;
    float Buff = 10f;
    float newRange = 0f;
    // Start is called before the first frame update
    void Start()
    {
        towers = towers.GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Crystal"))
        {
            towers.Range += Buff;
        }
        else
        {
            //towers.Range;
            
        }
    }
}
