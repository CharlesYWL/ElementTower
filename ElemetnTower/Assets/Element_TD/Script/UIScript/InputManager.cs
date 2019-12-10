using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public Button shopbutton;
    public Button refreshbutton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shopbutton.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            BuildManager.instance.RefreshShop();
        }
    }
}
