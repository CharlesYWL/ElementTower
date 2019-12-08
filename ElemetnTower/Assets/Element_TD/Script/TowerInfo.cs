using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TowerInfo : MonoBehaviour, IPointerClickHandler
{
    public Text text;
    //public GameObject TextHolder;
    private bool displayInfo = false;
    public float fadeTime = 1f;
    private BuildManager bm;
    // Start is called before the first frame update
    void Start()
    {
        bm = BuildManager.instance;
        text.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        TurnRotate();
        FadeText();
    }

    private void TurnRotate()
    {
/*        Camera cam = bm.GetCamera();
        
        TextHolder.transform.LookAt(cam.transform);
        //TextHolder.transform.Rotate(0,90,0);*/

    }

    private void OnMouseEnter()
    {
        if (text)
        {
            displayInfo = true;
        }
    }
    private void OnMouseExit()
    {
        if (text)
        {
            displayInfo = false;
        }
    }

    // TODO: It will pop up recycle sign to click.
    private void OnMouseDown()
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

    private void FadeText()
    {
        if (displayInfo)
        {
            text.color = Color.Lerp(Color.white, Color.clear, fadeTime * Time.deltaTime);
        }
        else
        { text.color = Color.Lerp(Color.clear, Color.white, fadeTime * Time.deltaTime); }
    }
}
