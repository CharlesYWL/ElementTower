using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTitle : MonoBehaviour
{
    public float LastTime = 3f;
    private float count = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if (count >= LastTime)
        {
            //Add sounds something
            BuildManager bm = BuildManager.instance;
            bm.GoToEndScene();
        }
    }
}
