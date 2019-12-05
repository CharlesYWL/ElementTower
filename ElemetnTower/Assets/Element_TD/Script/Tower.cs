﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Transform Target=null;

    [Header("Property")]
    public float Range = 15f;
    public float FireRate = 1f;
    private float FireCountDown = 0f;
    
    [Header("Setup Fields")]
    public string EnemyTag = "Enemy";
    public GameObject ProjectilePrefab;
    public Transform ProjectilePoint;
    public Transform ProjectCrystal;


    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        float ShortestPath = Mathf.Infinity;
        GameObject NearestEnemy = null;
        foreach(GameObject Enemy in Enemies)
        {
            float DistanceToEnemy = Vector3.Distance(transform.position, Enemy.transform.position);
            if(DistanceToEnemy < ShortestPath)
            {
                ShortestPath = DistanceToEnemy;
                NearestEnemy = Enemy;
            }
        }

        //Find the closest target
        if(NearestEnemy != null && ShortestPath <= Range)
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
            UpdateTarget();
            return;
        }
        //Fire Projectile Method
        if(FireCountDown <= 0f)
        {
            FireProjectile();
            FireCountDown = 1f / FireRate;
        }
        FireCountDown -= Time.deltaTime;

        RotateCrystal(ProjectCrystal);
    }

    // this function used to rate Crystal
    private void RotateCrystal(Transform crystal)
    {
        
    }

    public void FireProjectile()
    {
        this.ProjectilePoint.LookAt(Target);
        GameObject ProjectileShoot = (GameObject)Instantiate(ProjectilePrefab, ProjectilePoint.position, ProjectilePoint.rotation);

        ProjectileMover pm = ProjectileShoot.GetComponent<ProjectileMover>();
        if (pm != null)
        {
            //Auto follow target
            pm.FollowTarget = this.Target;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
