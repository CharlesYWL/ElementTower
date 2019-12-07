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

    public GameObject cards;
    private Cards c;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("more than one BuildManager in scene!");
            return;
        }
        instance = this;
        c = cards.GetComponent<Cards>();
    }
    void Start()
    {
        InvokeRepeating("RandomGenerateCard", 2.0f, 1f);
    }

    public void Update()
    {
        
    }


    public void RandomGenerateCard()
    {

        Debug.Log("Genereate Card");
        if(c.getCardsNumber() >= 5)
        {
            return;
        }
        c.addPrefeb(this.OceanTower);
    }
}
