using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CardInfoShow : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler
{
    public Text text;
    public Text MoneyText;

    //private Text t;
    private bool displayInfo = false;
    public float fadeTime= 1f;
    public Camera cam;
    private BuildManager bm;
    private GameObject canvas;

    [Header("MONEY")]
    public int CardMoney;
    // Start is called before the first frame update
    void Start()
    {
        bm = BuildManager.instance;
        //t = Info.GetComponent<Text>();
        text.color = Color.clear;
        cam = bm.GetCamera();
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        FadeText();
        if (displayInfo)
        {
            MoveObject();
        }
    }

    // Not Usable
    public void MoveObject() {
/*        Vector2 Position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition,
            cam, out Position);*/
        //text.transform.position = canvas.transform.TransformPoint(Position);


    }


    private void FadeText()
    {
        if (displayInfo)
        { 
            text.color = Color.Lerp(Color.white, Color.clear, fadeTime * Time.deltaTime); }
        else
        { text.color = Color.Lerp(Color.clear, Color.white, fadeTime * Time.deltaTime); }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (text)
        {
            displayInfo = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (text)
        {
            displayInfo = false;
        }
    }


}
