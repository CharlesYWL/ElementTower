using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int healthCounts;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullhearts;
    public Sprite emptyhearts;

    // Start is called before the first frame update
    void Start()
    {
        healthCounts = 10;
        numOfHearts = 10;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(healthCounts > numOfHearts)
            {
                numOfHearts = healthCounts;
            }

            if (i < healthCounts)
            {
                hearts[i].sprite = fullhearts;
            } else
            {
                hearts[i].sprite = emptyhearts;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            } else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
