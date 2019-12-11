using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    [SerializeField]
    public Text textobject;
    public Text Wave;

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
        if (WaveController.waveNumber.ToString() == "0")
        {
            Wave.text = "PREPARING";
        }
        else
        {
            Wave.text = WaveController.waveNumber.ToString();
        }
    }

    public static TopPanel getTopPanelInfo
    {
        get { return instance; }
    }
}