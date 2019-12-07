using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Element;

/// <summary>
/// ITS useless now, buy leave it for further necessary
/// </summary>
public class BuildManager : MonoBehaviour
{
    //Shared public 
    public static BuildManager instance;
    public GameObject CyrstalTower;
    public GameObject DesertTower;
    public GameObject FireTower;
    public GameObject GlacierTower;
    public GameObject LightTower;
    public GameObject MountainTower;
    public GameObject OceanTower;
    public GameObject PoisonTower;
    public GameObject ShadoeTower;
    public GameObject ThunderTower;
    public GameObject WindTower;




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
