using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour
{
    BuildManager buildManager;
    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChooseMageTowerLv1()
    {
        Debug.Log("We choose MageLv1");
        buildManager.SetTowerToBuild(buildManager.MageTowerLv1Prefeb);
    }
    public void ChooseMageTowerLv2()
    {
        Debug.Log("We choose MageLv2");
        buildManager.SetTowerToBuild(buildManager.MageTowerLv2Prefeb);
    }
}
