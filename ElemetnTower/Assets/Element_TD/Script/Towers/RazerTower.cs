using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RazerTower : BuildInterface
{
    private Transform Target = null;

    [Header("Property")]
    public float Range = 15f;
    public float FireRate = 1f;
    private float damageOverTime = 40f;
      
    [Header("Setup Fields")]
    public string EnemyTag = "Enemy";

    public bool isRaser = false;
    public LineRenderer LineRenderer;

    public Transform ProjectilePoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    //Find the closest enemy
    private void UpdateTarget()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        float ShortestPath = Mathf.Infinity;
        GameObject NearestEnemy = null;
        foreach (GameObject Enemy in Enemies)
        {
            float DistanceToEnemy = Vector3.Distance(transform.position, Enemy.transform.position);
            if (DistanceToEnemy < ShortestPath)
            {
                ShortestPath = DistanceToEnemy;
                NearestEnemy = Enemy;
            }
        }

        //Find the closest target
        if (NearestEnemy != null && ShortestPath <= Range)
        {
            Target = NearestEnemy.transform;
        }
        else
        {
            Target = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            
            if (isRaser)
            {
                if (LineRenderer.enabled)
                {
                    LineRenderer.enabled = false;
                }
            }
            
            return;
        }


        //Razer Method
        if(isRaser)
        {
            FireRazer();
        }


    }

    public void FireRazer()
    {
        
        Target.GetComponent<Enemy>().TakeDamage(damageOverTime * Time.deltaTime);
        if(!LineRenderer.enabled)
        {
            LineRenderer.enabled = true;
        }
        LineRenderer.SetPosition(0, ProjectilePoint.position);
        LineRenderer.SetPosition(1, Target.position);
        

    }


    //This will draw the range line around  tower
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }


}
