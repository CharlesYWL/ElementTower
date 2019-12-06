using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //Shared public 
    public static BuildManager instance;
    public GameObject MageTowerLv1Prefeb;
    public GameObject MageTowerLv2Prefeb;
    public GameObject MageTowerLv3Prefeb;
    public GameObject MageTowerLv4Prefeb;


    private GameObject towerToBuild;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("more than one BuildManager in scene!");
            return;
        }
        instance = this;
    }


    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }

    public void SetTowerToBuild(GameObject tower)
    {
        towerToBuild = tower;
    }
}
