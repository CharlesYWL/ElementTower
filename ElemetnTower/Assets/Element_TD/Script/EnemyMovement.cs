using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;
    private Enemy enemy;
    private float timeCount = 0;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = WayPoints.points[0];
    }
    void Update()
    {
        timeCount += Time.deltaTime;
        if(timeCount > 0 && timeCount <= 10)
        {
            BottomWave();
        }
        else if(timeCount > 10 && timeCount <= 20)
        {
            TopWave();
        }
        else if(timeCount >= 20)
        {
            LeftWave();
        }
        
        
    }

    
    void GetNextWaypoint()
    {
        if (wavepointIndex >= WayPoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = WayPoints.points[wavepointIndex];
    }

    void ChangeToLeftWave()
    {
        target = WayPointsLeft.Leftpoints[0];
    }

    void ChangeToTopWave()
    {
        target = WayPointsTop.TopPoints[0];
    }
    void BottomWave()
    {

        Quaternion Rotation = Quaternion.LookRotation(WayPoints.points[wavepointIndex].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, Time.deltaTime * enemy.RotationSpeed);
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.Speed * Time.deltaTime, Space.World); //Initialze the first wavepoint

        //Obtain next wavepoint
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
        //Reset speed to initial after slow down enemy
        if (enemy.Speed <= enemy.startSpeed)
        {
            enemy.Speed += Time.deltaTime;
        }
    }

    void LeftWave()
    {
        ChangeToLeftWave();
        Quaternion Rotation = Quaternion.LookRotation(WayPoints.points[wavepointIndex].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, Time.deltaTime * enemy.RotationSpeed);
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.Speed * Time.deltaTime, Space.World); //Initialze the first wavepoint

        //Obtain next wavepoint
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
        //Reset speed to initial after slow down enemy
        if (enemy.Speed <= enemy.startSpeed)
        {
            enemy.Speed += Time.deltaTime;
        }
    }
    void TopWave()
    {
        ChangeToTopWave();
        Quaternion Rotation = Quaternion.LookRotation(WayPoints.points[wavepointIndex].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, Time.deltaTime * enemy.RotationSpeed);
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.Speed * Time.deltaTime, Space.World); //Initialze the first wavepoint

        //Obtain next wavepoint
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
        //Reset speed to initial after slow down enemy
        if (enemy.Speed <= enemy.startSpeed)
        {
            enemy.Speed += Time.deltaTime;
        }
    }
    void EndPath()
    {
        Destroy(gameObject);
        //health -1
        //if health -3 life heart -1
        //when life heart == 0, end game


    }

}
