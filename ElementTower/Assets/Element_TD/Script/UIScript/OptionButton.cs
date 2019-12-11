using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public GameObject OptionsPage;
    private bool isOptionOpen = false;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOption();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleOption();
    }

    public void ToggleOption()
    {
        if (isOptionOpen)
        { //now close Panel and continue game
            isOptionOpen = false;
            ContinueGame();
        }
        else
        { // now open panel and pause game
            isOptionOpen = true;
            PauseGame();
        }
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
        OptionsPage.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }
    private void ContinueGame()
    {
        Time.timeScale = 1;
        OptionsPage.SetActive(false);
        //enable the scripts again
    }
}
