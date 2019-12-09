using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSript : MonoBehaviour
{
    [SerializeField]
    public GameObject gb;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("started method");
        GenerateCard();
    }

    private void GenerateCard()
    {
        for(int i=0; i<5; i++)
        {
            var temp = Instantiate(gb);
            temp.SetActive(true);
            Debug.Log("loop is " + i);
        }
    }
}
