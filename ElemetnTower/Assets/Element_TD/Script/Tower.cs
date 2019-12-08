using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : BuildInterface
{
    private Transform Target = null;

    [Header("Property")]
    public float Range = 15f;
    public float FireRate = 1f;
    public LineRenderer LineRenderer;
    private float FireCountDown = 0f;
    private float damageOverTime = 4f;
      
    [Header("Setup Fields")]
    public string EnemyTag = "Enemy";
    public bool isProjectile = false;
    public bool isRaser = false;
    public GameObject ProjectilePrefab;
    public Transform ProjectilePoint;
    public GameObject hint;
    private GameObject hintWehave;

    Animator Attack;

    void Start()
    {
        Attack = GetComponent<Animator>();
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

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
            
            if (isRaser == true)
            {
                if (LineRenderer.enabled)
                {
                    LineRenderer.enabled = false;
                }
            }
            UpdateTarget();
            Attack.ResetTrigger("Attack");
            
            return;
        }
        else
        {
            Attack.SetTrigger("Attack");
            float DistanceToEnemy = Vector3.Distance(transform.position, Target.position);
            if (DistanceToEnemy > Range)
            {
                UpdateTarget();
            }

        }

        //Razer Method
        if(isRaser == true)
        {
            FireRazer();
        }
        //Projectile Method
        if (FireCountDown <= 0f && isProjectile == true)
        {
            FireProjectile();
            FireCountDown = 1f / FireRate;
            
        }
        FireCountDown -= Time.deltaTime;

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

    public void FireProjectile()
    {
        this.ProjectilePoint.LookAt(Target);
        GameObject ProjectileShoot = (GameObject)Instantiate(ProjectilePrefab, ProjectilePoint.position, ProjectilePoint.rotation);
        ProjectileShoot.transform.localScale = new Vector3(4, 4, 4);
        ProjectileMover pm = ProjectileShoot.GetComponent<ProjectileMover>();
        if (pm != null)
        {
            //Auto follow target
            pm.FollowTarget = this.Target;
        }
    }

    //This will draw the range line around  tower
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }


    // OnMouseEnter and OnMouseExit relate to choose stuff
    private void OnMouseEnter()
    {

        //get hint for users
        if (hintWehave)
        {
            return;
        }
        hintWehave = Instantiate(hint, transform.position, transform.rotation);
        hintWehave.transform.Rotate(-90, 0, 0);
        hintWehave.transform.localScale = new Vector3(8, 8, 8);
    }
    private void OnMouseExit()
    {
        Destroy(hintWehave);
    }

    // TODO: It will pop up recycle sign to click.
    private void OnMouseDown()
    {
        
    }

}
