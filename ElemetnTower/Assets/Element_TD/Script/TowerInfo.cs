using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TowerInfo : MonoBehaviour, IPointerClickHandler
{
    [Header("Setting")]
    public Text text;
    public float fadeTime = 1f;
    [Header("Prefebs")]
    public GameObject hint;
    public GameObject CardPrefeb;
    public GameObject UpgradeTower;
    //public GameObject TextHolder;
    private bool displayInfo = false;

    private GameObject hintWehave=null;
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

        text.transform.LookAt(cam.transform);
        text.transform.Rotate(0,90,0);*/

    }

    private void OnMouseEnter()
    {
        if (text)
        {
            displayInfo = true;
        }
        //get hint for users
        if (hintWehave)
        {
            return;
        }
        hintWehave = Instantiate(hint, transform.position, transform.rotation);
        hintWehave.transform.Rotate(-90, 0, 0);
        hintWehave.transform.localScale = new Vector3(8, 8, 8);
    }
    private void OnMouseExit()
    {
        if (text)
        {
            displayInfo = false;
        }
        Destroy(hintWehave);
    }

    // TODO: It will pop up recycle sign to click.
    private void OnMouseDown()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {   //Show Tower Infomation
            bm.TowerClicked(gameObject);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Test only
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
