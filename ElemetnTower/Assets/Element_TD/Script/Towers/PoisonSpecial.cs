using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fire tower will help poison spread out its poison effect
/// </summary>
public class PoisonSpecial : MonoBehaviour
{
    GameObject AOEPoison;
    private bool Comboflag = false;
    private float buffAS = 3f;
    float currentHealth = 0f;
    private Enemy targets;
    private TowerLabel towerLabel;
    float count = 0f;
    float CD = 5f;
    // Start is called before the first frame update
    void Start()
    {
        targets = gameObject.GetComponent<Enemy>();
        towerLabel = gameObject.GetComponent<TowerLabel>();
        currentHealth = targets.Health;
        switch (towerLabel.Level)
        {
            case 1:
                buffAS = 5f;
                break;
            case 2:
                buffAS = 10f;
                break;
            case 3:
                buffAS = 15f;
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
        if (!Comboflag && dt)
        {
            if(count < CD)
            {
                //Instantiate(AOEPoison, transform.position, Quaternion.identity);
                currentHealth -= buffAS * Time.deltaTime;
                Comboflag = true;
            }
            else
            {
                //Destroy(AOEPoison);
                targets.Health = currentHealth;
            }
            
        }
        else if (Comboflag && !dt)
        {
            targets.Health = currentHealth;

            Comboflag = false;
        }
    }
}
