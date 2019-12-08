using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    [SerializeField]
    public Text textobject;

    public static TopPanel instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        textobject.text = BuildManager.instance.GetMoney().ToString();
    }

    public static TopPanel getTopPanelInfo
    {
        get { return instance; }
    }
}