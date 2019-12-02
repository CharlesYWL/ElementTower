using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

    private Transform target;
    private int wavepointIndex = 0;
    public float Speed = 10f;

    void Start()
    {
        target = WayPoint.points[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= WayPoint.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = WayPoint.points[wavepointIndex];
    }

    void EndPath()
    {
        Destroy(gameObject);
    }

}