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
    [Header("CardPool")]
    public static BuildManager instance;
    public GameObject CyrstalTower;
    public GameObject DesertTower;
    public GameObject FireTower;
    public GameObject GlacierTower;
    public GameObject LightTower;
    public GameObject MountainTower;
    public GameObject OceanTower;
    public GameObject PoisonTower; // Cannot be used
    public GameObject ShadoeTower;
    public GameObject ThunderTower;
    public GameObject WindTower;

    [Header("UI")]
    public GameObject CardsHoler;
    public GameObject ShopHoler;
    private Cards c;
    private Shop s;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("more than one BuildManager in scene!");
            return;
        }
        instance = this;
        c = CardsHoler.GetComponent<Cards>();
        s = ShopHoler.GetComponent<Shop>();
    }
    void Start()
    {
        //This is for demo only! Delete after we have purchase system
        InvokeRepeating("RandomGenerateCard", 3.0f, 5f);
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
        GameObject target=null;
        int i = Random.Range(1, 11);
        switch (i)
        {
            case 1:
                target = this.CyrstalTower;
                break;
            case 2:
                target = this.DesertTower;
                break;
            case 3:
                target = this.FireTower;
                break;
            case 4:
                target = this.GlacierTower;
                break;
            case 5:
                target = this.LightTower;
                break;
            case 6:
                target = this.MountainTower;
                break;
            case 7:
                target = this.OceanTower;
                break;
            case 8:
                target = this.PoisonTower;
                break;
            case 9:
                target = this.ShadoeTower;
                break;
            case 10:
                target = this.ThunderTower;
                break;
            case 11:
                target = this.WindTower;
                break;
            default:
                break;
        }
        c.addPrefeb(target);
    }
}
