using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;
    private Enemy enemy;
    private float timeCount = 0;
    public int WayChoise = 0;
    public Transform[] targets;
    public int DamageToPlayer = 1;
    BuildManager bm;
    void Start()
    {
        bm = BuildManager.instance;
        enemy = GetComponent<Enemy>();

        target = targets[0];
    }
    void Update()
    {
        timeCount += Time.deltaTime;

        Quaternion Rotation = Quaternion.LookRotation(targets[wavepointIndex].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, Time.deltaTime * enemy.RotationSpeed);
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.Speed * Time.deltaTime, Space.World); //Initialze the first wavepoint

        //Obtain next wavepoint
        if (Vector3.Distance(transform.position, targets[wavepointIndex].position) <= 0.4f)
        {
            GetNextWaypoint();
        }
        //Reset speed to initial after slow down enemy
        if (enemy.Speed <= enemy.startSpeed)
        {
            enemy.Speed += Time.deltaTime;
        }


    }

    public void SetTargets(Transform[] t)
    {
        this.targets = t;
    }
    void GetNextWaypoint()
    {
        if (wavepointIndex >= targets.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = targets[wavepointIndex];
    }

    void EndPath()
    {
        bm.PlayerGetDamage(DamageToPlayer);
        Destroy(gameObject);
        //health -1
        //if health -3 life heart -1
        //when life heart == 0, end game

    //    health.healthCounts = health.healthCounts - 1; 
    }

}
