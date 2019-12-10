using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float Health = 100f;
    public float startSpeed = 10f;    
    public float RotationSpeed = 5f;
    
    //[HideInInspector]
    public float Speed;
    public float SlowFactor = 2f;

    private void Start()
    {
        Speed = startSpeed;
    }
    //Damage Method
    public void TakeDamage(float DamageAmount)
    {
        Health -= DamageAmount;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        BuildManager.instance.AddMoney();
    }

    public void Slow(float percentage)
    {
        Speed = startSpeed *(1f - percentage);
        Debug.Log(Speed);
    }
    public void Poison(float damage)
    {
        
    }
}