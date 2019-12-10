using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fire tower will help poison spread out its poison effect
/// </summary>
public class GlacierSpecial : MonoBehaviour
{
    GameObject AOEGlacier;
    private bool Comboflag = false;
    private float buffAS = 3f;
    float currentSpeed = 0f;
    private Enemy targets;
    private TowerLabel towerLabel;
    float count = 0f;
    float CD = 5f;
    // Start is called before the first frame update
    void Start()
    {
        targets = gameObject.GetComponent<Enemy>();
        towerLabel = gameObject.GetComponent<TowerLabel>();
        currentSpeed = targets.startSpeed;
        switch (towerLabel.Level)
        {
            case 1:
                buffAS = 2f;
                break;
            case 2:
                buffAS = 3f;
                break;
            case 3:
                buffAS = 4f;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        var dt = GameObject.FindGameObjectWithTag("Ocean");
        if (!Comboflag && dt)
        {
            if (count < CD)
            {
                //Instantiate(AOEGlacier, transform.position, Quaternion.identity);
                currentSpeed -= buffAS * Time.deltaTime;
                Comboflag = true;
            }
            else
            {
                //Destroy(AOEGlacier);
                targets.startSpeed = currentSpeed;
            }

        }
        else if (Comboflag && !dt)
        {
            targets.Health = currentSpeed;

            Comboflag = false;
        }
    }
}
