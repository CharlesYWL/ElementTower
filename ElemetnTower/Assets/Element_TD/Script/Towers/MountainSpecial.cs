using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fire tower will help poison spread out its poison effect
/// </summary>
public class MountainSpecial : MonoBehaviour
{
    public GameObject AOEPoison;
    private float PoisonDamage = 3f;
    private Tower tower;
    private TowerLabel towerLabel;
    float count = 0f;
    float CD = 5f;
    private GameObject ComboEffectWeHave = null;
    // Start is called before the first frame update
    void Start()
    {
        tower = gameObject.GetComponent<Tower>();
        towerLabel = gameObject.GetComponent<TowerLabel>();
        switch (towerLabel.Level)
        {
            case 1:
                PoisonDamage = 10f;
                break;
            case 2:
                PoisonDamage = 20f;
                break;
            case 3:
                PoisonDamage = 40f;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        count -= Time.deltaTime;
        var dt = GameObject.FindGameObjectWithTag("Fire");
        if (dt)
        {
            // Apply Poison AOE to in range enemy
            GameObject[] es = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in es)
            {
                if (Vector3.Distance(this.transform.position, e.transform.position) <= tower.Range)
                {
                    Enemy enemy = e.GetComponent<Enemy>();
                    enemy.Health -= PoisonDamage;
                }
            }
            if (count <= 0 && tower.Target != null && !ComboEffectWeHave)
            {
                Quaternion rotation = Quaternion.FromToRotation(this.transform.position, tower.Target.transform.position);
                ComboEffectWeHave = Instantiate(AOEPoison, transform.position, rotation);
                ComboEffectWeHave.transform.localScale = new Vector3(4, 4, 4);
                Destroy(AOEPoison);
                count = CD;
            }
        }
    }
}
