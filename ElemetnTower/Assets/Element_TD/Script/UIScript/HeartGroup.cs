using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartGroup : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Life0;
    public GameObject Life1;
    public GameObject Life2;
    public GameObject Life3;
    public GameObject Life4;
    public GameObject Life5;
    public GameObject Life6;
    public GameObject Life7;
    public GameObject Life8;
    public GameObject Life9;

    private BuildManager bm;
    private int Life = 10;
    void Start()
    {
        bm = BuildManager.instance;
        Life = 10;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseLife() 
    {
        if (Life > 1)
        {
            ReduceLife(Life);
            Life--;
        }
        else
        {// we lose
            bm.LoseGame();
        }

    }

    public void PlayerGetDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            LoseLife();
        }
    }

    private void ReduceLife(int i)
    {
        switch (i)
        {
            case 1:
                Life0.SetActive(false);
                break;
            case 2:
                Life1.SetActive(false);
                break;
            case 3:
                Life2.SetActive(false);
                break;
            case 4:
                Life3.SetActive(false);
                break;
            case 5:
                Life4.SetActive(false);
                break;
            case 6:
                Life5.SetActive(false);
                break;
            case 7:
                Life6.SetActive(false);
                break;
            case 8:
                Life7.SetActive(false);
                break;
            case 9:
                Life8.SetActive(false);
                break;
            case 10:
                Life9.SetActive(false);
                break;
            default:
                break;
        }
    }
}
