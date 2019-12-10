using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fire tower will help poison spread out its poison effect
/// </summary>
public class GlacierSpecial : MonoBehaviour
{
    public GameObject AOEGlacier;
    private float SlowRatio = 3f;
    private TowerLabel towerLabel;
    private Tower tower;
    private float count = 0f;
    private float CD = 5f;
    private GameObject ComboEffectWeHave = null;
    // Start is called before the first frame update
    void Start()
    {
        tower = gameObject.GetComponent<Tower>();
        towerLabel = gameObject.GetComponent<TowerLabel>();
        switch (towerLabel.Level)
        {
            case 1:
                SlowRatio = 0.3f;
                break;
            case 2:
                SlowRatio = 0.5f;
                break;
            case 3:
                SlowRatio = 0.7f;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        count -= Time.deltaTime;
        var dt = GameObject.FindGameObjectWithTag("Ocean");
        if (dt)
        {
            // Apply slow to in range enemy
            GameObject[] es = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in es)
            {
                if (Vector3.Distance(this.transform.position, e.transform.position) <= tower.Range)
                {
                    Enemy enemy = e.GetComponent<Enemy>();
                    enemy.Slow(SlowRatio);
                }
            }
            if (count <= 0 && tower.Target != null && !ComboEffectWeHave)
            {
                Quaternion rotation = Quaternion.FromToRotation(this.transform.position, tower.Target.transform.position);
                ComboEffectWeHave = Instantiate(AOEGlacier, transform.position, rotation);
                ComboEffectWeHave.transform.localScale=new Vector3(2, 2, 2);
                Destroy(AOEGlacier);
                count = CD;
            }
        }
    }
}
