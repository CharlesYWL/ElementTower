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
    public GameObject TopPanel;
    public Camera cam;
    private Cards c;
    private Shop s;


    enum ElementType { FireTower , GlacierTower , WindTower , OceanTower , DesertTower , ThunderTower, MountainTower, LightTower, ShadoeTower, CyrstalTower, PoisonTower }

    private float timeCount = 0f;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("more than one BuildManager in scene!");
            return;
        }
        instance = this;
        if (CardsHoler == null || ShopHoler == null)
        {
            Debug.LogError("BuildManager Need Cards & Shops! Check Inspector");
        }
        c = CardsHoler.GetComponent<Cards>();
        s = ShopHoler.GetComponent<Shop>();
    }
    void Start()
    {
        //This is for demo only! Delete after we have purchase system
        //InvokeRepeating("RandomGenerateCard", 3.0f, 5f);
    }

    public void Update()
    {
        timeCount += Time.deltaTime;
    }


    public void RandomGenerateCard()
    {
        //TODO: neew to modify prob
        GameObject target=null;
        int i = Random.Range(1, 12);
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
        s.addPrefeb(target);
    }

    public Cards getCards() {return this.c;}

    public Shop GetShop() { return this.s; }

    public void RefreshShop()
    {
        s.clearShop();
        // We add 5 random prefebs to shop
        while (!s.isFullCap())
        {
            RandomGenerateCard();
        }
    }

    public float GetTime()
    {
        return this.timeCount;
    }

    public void CardClicked(GameObject card)
    {
        if (isChildOfShop(card) && c.SlotAvialiable())
        { // which means card is in shop
            //TODO: Check Money Here!!!
            card.transform.SetParent(CardsHoler.transform);
        }
        return;
    }
    public bool isChildOfShop(GameObject card)
    {
        Draggable dg = card.GetComponent<Draggable>();

        Debug.Log("Check Child of SHOP: "+ (dg.parentToReturnTo == this.ShopHoler.transform));
        return dg.parentToReturnTo==this.ShopHoler.transform;
    }

    public Camera GetCamera() {
        return cam;    
    }
}
