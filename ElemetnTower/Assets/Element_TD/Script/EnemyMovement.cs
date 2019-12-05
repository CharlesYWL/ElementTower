using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

    private Transform target;
    private float MaxTurnSpeed = 10f;
    private int wavepointIndex = 0;
    public float Speed = 10f;

    void Start()
    {
        target = WayPoints.points[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World); //Initialze the first wavepoint

        //Obtain next wavepoint
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
        Quaternion wanted_rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted_rotation, MaxTurnSpeed * Time.deltaTime);
        
        

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

    void EndPath()
    {
        Destroy(gameObject);
    }

}