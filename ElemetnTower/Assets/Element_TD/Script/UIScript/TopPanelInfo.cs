using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TopPanelInfo : MonoBehaviour
{
    [SerializeField]
    public int Dollar;
    [SerializeField]
    public int AddDollar;
    [SerializeField]
    public Text textobject;

    public static TopPanelInfo instance;

    private void Start()
    {
        textobject.text = Dollar.ToString();
        instance = this;

    }

    public void AddMoney()
    {
        this.Dollar += AddDollar;
        textobject.text = Dollar.ToString();
    }

    public static TopPanelInfo getTopPanelInfo
    {
        get { return instance; }
    }

    void update()
    {
    }
}
