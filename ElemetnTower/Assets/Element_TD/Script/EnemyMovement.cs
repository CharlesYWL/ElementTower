using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private float Health = 100f;
    private Transform target;
    private float MaxTurnSpeed = 10f;
    private float RotationSpeed = 5f;
    private int wavepointIndex = 0;
    public float Speed = 10f;
    public float SlowFactor = 2f;
    void Start()
    {
        target = WayPoints.points[0];
    }
    //Damage Method
    public void TakeDamage(float DamageAmount)
    {
        Health -= DamageAmount;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SlowDownSpeed()
    {
        Speed = 6f;
    }
    void Update()
    {
        Quaternion Rotation = Quaternion.LookRotation(-WayPoints.points[wavepointIndex].position + transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, Time.deltaTime * RotationSpeed);
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World); //Initialze the first wavepoint

        //Obtain next wavepoint
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
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

    void EndPath()
    {
        Destroy(gameObject);
    }

}