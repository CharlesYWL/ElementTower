using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fire tower will help poison spread out its poison effect
/// </summary>
public class PoisonSpecial : MonoBehaviour
{
    public GameObject AOEPoison;
    private bool Comboflag = false;
    private float PoisonDamage = 3f;
    float currentHealth = 0f;
    private Tower tower;
    private TowerLabel towerLabel;
    float count = 0f;
    float CD = 5f;
    // Start is called before the first frame update
    void Start()
    {
        tower = gameObject.GetComponent<Tower>();
        towerLabel = gameObject.GetComponent<TowerLabel>();
        switch (towerLabel.Level)
        {
            case 1:
                PoisonDamage = 5f;
                break;
            case 2:
                PoisonDamage = 10f;
                break;
            case 3:
                PoisonDamage = 15f;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        var dt = GameObject.FindGameObjectWithTag("Fire");
        if (dt)
        {
            
        }
    }
}
