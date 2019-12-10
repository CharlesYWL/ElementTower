using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Element;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

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
    public GameObject TowerRangeMark;
    public GameObject heartGroup;
    public GameObject UpgradeEffect;
    public GameObject WastedTitle;
    public GameObject WinTitle;
    //Money
    [Header("MONEY")]
    [SerializeField]
    public float Money;
    [SerializeField]
    public float Addmoney;
    [SerializeField]
    public Text Moneytext;
    [SerializeField]
    public int RefreshMoney;

    private Cards c;
    private Shop s;
    private GameObject hintWeHave;
    private GameObject RangeWeHave;
    private bool SellUIActive = false;
    private bool isOpen = false;
    private bool firstclick = true;
    private System.Random random;
    private float RescaleMark = 0.37f;
    public GameObject SelectedTower;
    public enum ElementType { FireTower , GlacierTower , WindTower , OceanTower , DesertTower , ThunderTower, MountainTower, LightTower, ShadoeTower, CyrstalTower, PoisonTower }

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
        random = new System.Random();
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
        List<float>prob = GetWaveProb();
        int i = GetRandomResult(prob);
        GameObject target=null;
        switch (i)
        {
            case 0:
                target = this.FireTower;
                break;
            case 1:
                target = this.GlacierTower;
                break;
            case 2:
                target = this.WindTower;
                break;
            case 3:
                target = this.OceanTower;
                break;
            case 4:
                target = this.DesertTower;
                break;
            case 5:
                target = this.ThunderTower;
                break;
            case 6:
                target = this.MountainTower;
                break;
            case 7:
                target = this.LightTower;
                break;
            case 8:
                target = this.ShadoeTower;
                break;
            case 9:
                target = this.CyrstalTower;
                break;
            case 10:
                target = this.PoisonTower;
                break;
            default:
                break;
        }
        s.addPrefeb(target);
    }

    private int GetRandomResult(List<float> prob)
    {
        float sum = 0;
        List<float> AcumulateProb = new List<float>();
        for (int j = 0; j < prob.Count; j++)
        {
            sum += prob[j];
            AcumulateProb.Add(sum);
        }
        float num = (float)random.NextDouble()*100;

        int i;
        for ( i = 0; i < AcumulateProb.Count; i++)
        {
            if (num <= AcumulateProb[i]) break ;
        }
        return i;
    }

    private List<float> GetWaveProb()
    {

        //TODO: Get wave from other
        int i = WaveController.waveNumber;
        //Get wave from other place;
        switch (i)
        {
            case 0:
                return new List<float>() { 33.3f, 33.3f, 33.3f, 0, 0, 0, 0, 0, 0, 0, 0 };
            case 1:
                return new List<float>() {33.3f,33.3f,33.3f,0,0,0,0,0,0,0,0};
            case 2:
                return new List<float>() {23.3f,23.3f,23.3f,15,15,0,0,0,0,0,0};
            case 3:
                return new List<float>() { 20, 20, 20, 17.5f, 17.5f, 2.5f, 2.5f, 0, 0, 0, 0 };
            case 4:
                return new List<float>() { 16.66F, 16.66F, 16.66F, 17.5f, 17.5f, 7.5f, 7.5f, 0, 0, 0, 0 };
            case 5:
                return new List<float>() { 13.33f, 13.33f, 13.33f, 17.5f, 17.5f, 11.5f, 11.5f, 1f, 1f, 0, 0 };
            case 6:
                return new List<float>() { 11, 11, 11, 15, 15, 15, 15, 3.5f, 3.5f, 0, 0 };
            case 7:
                return new List<float>() { 10, 10, 10, 15, 15, 15, 15, 5, 5, 0, 0 };
            case 8:
                return new List<float>() { 8, 8, 8, 15, 15, 15, 15, 7.5f, 7.5f, 0.5f, 0.5f };
            case 9:
                return new List<float>() { 7.3f, 7.3f, 7.3f, 12.5f, 12.5f, 15, 15, 10, 10, 1.5f, 1.5f };
            case 10:
                return new List<float>() { 6.3f, 6.3f, 6.3f, 12.5f, 12.5f, 12.5f, 12.5f, 12.5f, 12.5f, 3, 3};
            default:
                return new List<float>() { 6.3f, 6.3f, 6.3f, 12.5f, 12.5f, 12.5f, 12.5f, 12.5f, 12.5f, 3, 3 };
        }
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
            //toggleShop();
            return;
        }
        if (isOpen)
        {
            s.closeShop();
            toggleShop();
        }
        else
        {
            s.openShop();
            toggleShop();
        }
    }

    public void toggleShop()
    {
        if (isOpen)
        {
            isOpen = false;
        }else
        {
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
    public float GetMoney()
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
            DestroySelectUI();
            BuildSelectUI(tower);
        }
        else //Now we select it self, shoul toggle UI off
        {
            if (SellUIActive) {
                DestroySelectUI();
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
        TowerInfo tf = SelectedTower.GetComponent<TowerInfo>();
        if (tf.UpgradeTower && c.UpgradeTower(SelectedTower))
        {
            // Create NewTower and destory old tower
            Instantiate(tf.UpgradeTower, SelectedTower.transform.position, SelectedTower.transform.rotation);
            GameObject efc = Instantiate(this.UpgradeEffect, SelectedTower.transform.position, SelectedTower.transform.rotation);
            efc.transform.localScale = new Vector3(4, 4, 4);
            efc.transform.Rotate(-90, 0, 0);
            Destroy(this.SelectedTower);
            // Set Selected UI back
            Destroy(hintWeHave);
            Destroy(RangeWeHave);
            this.SelectedTower = null;
            SellUIPrefeb.SetActive(false);
        }

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
        Destroy(RangeWeHave);
        this.SelectedTower = null;
        SellUIPrefeb.SetActive(false);
    }

    public void BuildSelectUI(GameObject tower)
    {
        this.SelectedTower = tower;
        hintWeHave = Instantiate(Hint, tower.transform.position, tower.transform.rotation);
        hintWeHave.transform.localScale = new Vector3(6, 6, 6);
        SellUIPrefeb.transform.position = tower.transform.position;
        SellUIPrefeb.SetActive(true);
        SellUIActive = true;
        BuildRangeMark(tower, tower.transform);
    }
    public void DestroySelectUI()
    {
        this.SelectedTower = null;
        if (hintWeHave)
        {
            Destroy(hintWeHave);
        }
        if (RangeWeHave)
        {
            DestoryRangeMark();
        }
        SellUIPrefeb.SetActive(false);
        SellUIActive = false;
    }
    public void BuildRangeMark(GameObject tower,Transform tr) 
    {
        RangeWeHave = Instantiate(TowerRangeMark, tr.position + new Vector3(0, 0.5f, 0), tr.rotation);
        Tower t = tower.GetComponent<Tower>();
        RangeWeHave.transform.localScale = new Vector3(RescaleMark * t.Range, RescaleMark * t.Range, RescaleMark * t.Range);
        RangeWeHave.transform.Rotate(90, 0, 0);
    }
    public void DestoryRangeMark()
    {
        if (RangeWeHave)
        {
            Destroy(RangeWeHave);
        }
    }
    public bool IsTowerSelected() { return (SelectedTower != null); }

    public void LoseGame() 
    { //We lose, Add some text to show, and go EndScene
        WastedTitle.SetActive(true);   
    }
    public void WinGame()
    { //We lose, Add some text to show, and go EndScene
        WinTitle.SetActive(true);
    }

    public void PlayerGetDamage(int damage)
    {
        HeartGroup hg = heartGroup.GetComponent<HeartGroup>();
        hg.PlayerGetDamage(damage);
    }

    public void GoToEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
