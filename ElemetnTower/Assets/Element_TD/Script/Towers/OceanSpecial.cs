using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanSpecial : MonoBehaviour
{
    // Start is called before the first frame update
    private bool Comboflag = false;
    private float buffAS = 3f;
    private Tower t;
    private TowerLabel tl;
    void Start()
    {
        t = gameObject.GetComponent<Tower>();
        tl = gameObject.GetComponent<TowerLabel>();
        switch (tl.Level)
        {
            case 1:
                buffAS = 30f;
                break;
            case 2:
                buffAS = 40f;
                break;
            case 3:
                buffAS = 50f;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        var dt = GameObject.FindGameObjectWithTag("Desert");
        if (!Comboflag && dt)
        {
            t.FireRate += buffAS;
            Comboflag = true;
        } else if (Comboflag && !dt) 
        {
            t.FireRate -= buffAS;
            Comboflag = false;
        }

    }
}
