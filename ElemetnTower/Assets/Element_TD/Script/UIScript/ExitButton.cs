using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    private bool isOptionOpen = false;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }

}
