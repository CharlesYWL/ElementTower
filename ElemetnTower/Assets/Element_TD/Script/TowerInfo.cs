using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerInfo : MonoBehaviour, IPointerClickHandler
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {   //Show Tower Infomation
            Debug.Log("Left click");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Test only
            Debug.Log("Right click");
        }
    }
}
