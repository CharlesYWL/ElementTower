using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Element;
using UnityEngine.UI;

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
    public GameObject Hint;
    public GameObject SellUIPrefeb;
    public GameObject BuildPointPrefeb;

    //Money
    [Header("MONEY")]
    [SerializeField]
    public int Money;
    [SerializeField]
    public int Addmoney;
    [SerializeField]
    public Text Moneytext;
    [SerializeField]
    public int RefreshMoney;

    private Cards c;
    private Shop s;
    private GameObject hintWeHave;
    private bool SellUIActive = false;
    private bool isOpen = false;
    private bool firstclick = true;
    enum ElementType { FireTower , GlacierTower , WindTower , OceanTower , DesertTower , ThunderTower, MountainTower, LightTower, ShadoeTower, CyrstalTower, PoisonTower }

    private float timeCount = 0f;

    public GameObject SelectedTower;

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
        if (Money < RefreshMoney)
        {
            return;
        }
        Money -= RefreshMoney;
        s.clearShop();
        // We add 5 random prefebs to shop
        while (!s.isFullCap())
        {
            RandomGenerateCard();
        }
    }

    public void OpenCloseShop()
    {
        if (firstclick)
        {
            firstclick = false;
            
            RefreshShop();
            isOpen = true;
            return;
        }
        if (isOpen)
        {
            s.closeShop();
            isOpen = false;
        }
        else
        {
            s.openShop();
            isOpen = true;
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
        return dg.parentToReturnTo==this.ShopHoler.transform;
    }

    public Camera GetCamera() {
        return cam;    
    }

    //Money
    public int GetMoney()
    {
        return Money;
    }

    public void AddMoney()
    {
        this.Money += this.Addmoney;
    }
    public void TowerClicked(GameObject tower)
    {
        if (this.SelectedTower != tower) // we select differnt tower
        {
            this.SelectedTower = tower;
            if (hintWeHave)
            {
                Destroy(hintWeHave);
            }
            hintWeHave = Instantiate(Hint, tower.transform.position, tower.transform.rotation);
            hintWeHave.transform.localScale = new Vector3(6, 6, 6);
            SellUIPrefeb.transform.position = tower.transform.position;
            SellUIPrefeb.SetActive(true);
            SellUIActive = true;
        }
        else //Now we select it self, shoul toggle UI off
        {
            if (hintWeHave)
            {
                Destroy(hintWeHave);
            }
            if (SellUIActive) {
                this.SelectedTower = null;
                SellUIPrefeb.SetActive(false);
            }
            else
            {
                this.SelectedTower = tower;
                SellUIPrefeb.SetActive(true);
            }
        }

    }

    public void UpgradeClicked()
    {
        Debug.Log("We Click Upgrade");
    }
    public void RecycleClicked()
    {
        if (!c.SlotAvialiable())
        {
            Debug.Log("CardNoSlots");
            //Dothing warn player
            return;
        }
        //Now Add card back to hand
        TowerInfo tf = SelectedTower.GetComponent<TowerInfo>();
        c.addPrefeb(tf.CardPrefeb);
        // Create Buildpoint back and destory tower
        Instantiate(BuildPointPrefeb, SelectedTower.transform.position, SelectedTower.transform.rotation);
        Destroy(this.SelectedTower);
        // Set Selected UI back
        Destroy(hintWeHave);
        this.SelectedTower = null;
        SellUIPrefeb.SetActive(false);


    }

}
