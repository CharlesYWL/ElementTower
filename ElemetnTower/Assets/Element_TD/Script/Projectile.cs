using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform Target;
    public float speed = 70f;
    public void FindEnemy(Transform EnemyTarget)
    {
        Target = EnemyTarget;
    }
    // Update is called once per frame
    void Update()
    {
        if(Target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = Target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            AttackTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public void AttackTarget()
    {
        Destroy(gameObject);
    }
}
