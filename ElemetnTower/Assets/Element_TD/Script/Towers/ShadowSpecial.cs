using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSpecial : MonoBehaviour
{
    public GameObject AOEEffect;
    public float CD = 5f;
    public int Lv1Damage = 100;
    public int Lv2Damage = 200;
    public int Lv3Damage = 300;
    private Tower tower;
    private TowerLabel tl;
    private GameObject EffectWeHave=null;
    private float Damage;
    private float countTime;
    public float DamageDelay = 2f;
    private float DamageCountDown;
    private bool StrikeActive = false;
    // Start is called before the first frame update
    void Start()
    {
        tower = gameObject.GetComponent<Tower>();
        tl = gameObject.GetComponent<TowerLabel>();
        DamageCountDown = DamageDelay;

        switch (tl.Level)
        {
            case 1:
                Damage = Lv1Damage;
                break;
            case 2:
                Damage = Lv2Damage;
                break;
            case 3:
                Damage = Lv3Damage;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;
        if (StrikeActive)
        {
            DamageCountDown -= Time.deltaTime;
        }
        GameObject x = GameObject.FindGameObjectWithTag("Thunder");
        if (x)
        {
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log("time: " + countTime + "We have " + Enemys.Length + "Enemy");
            if (countTime >= CD && (Enemys.Length>=1))
            {
                countTime = 0;
                ThunderStrike();
            }
        }
        if (DamageCountDown <=0)
         {
            Debug.Log("Maing Damage");
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in Enemys)
            {
                if (Vector3.Distance(transform.position, e.transform.position) <= tower.Range)
                {
                    Enemy em = e.GetComponent<Enemy>();
                       em.TakeDamage(Damage);
                   }
                }
            //After taking dmage ,reset all
            StrikeActive = false;
            DamageCountDown = DamageDelay;
        }
    }

    private void ThunderStrike()
    {
        StrikeActive = true;
        EffectWeHave = Instantiate(AOEEffect, transform.position, transform.rotation);
        EffectWeHave.transform.localScale = new Vector3(3, 3, 3);
    }
}
