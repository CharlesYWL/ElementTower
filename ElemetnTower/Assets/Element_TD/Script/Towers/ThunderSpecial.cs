using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Thunder improves water's tower attack range
/// </summary>
public class ThunderSpecial : MonoBehaviour
{
    // Start is called before the first frame update
    private bool ComboFlag = false;
    private float buffAS = 3f;
    private Tower t;
    private TowerLabel t1;
    void Start()
    {
        t = gameObject.GetComponent<Tower>();
        t1 = gameObject.GetComponent<TowerLabel>();
        switch(t1.Level)
        {
            case 1:
                buffAS = 10f;
                break;
            case 2:
                buffAS = 20f;
                break;
            case 3:
                buffAS = 30f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var dt = GameObject.FindGameObjectWithTag("Ocean");
        if(!ComboFlag && dt)
        {
            t.Range += buffAS;
            ComboFlag = true;
        } else if(ComboFlag && !dt)
        {
            t.Range -= buffAS;
            ComboFlag = false;
        }
    }
}
