using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Element;
public class ProjectileMover : MonoBehaviour
{
    [SerializeField]public float speed = 15f;
    public float hitOffset = 0f;
    [SerializeField]public float damage = 150;
    private float percentage = 0.3f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    [SerializeField] private ElementTypes type = ElementTypes.Wind;
    //Modified for ElementTower
    public Transform FollowTarget = null;
    Tower Towers;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, 5);
    }

    void FixedUpdate()
    {
        if (speed != 0)
        {
            //Modified for ElementTower
            //rb.velocity = transform.forward * speed;
            Vector3 hightOffset = new Vector3(0, 2f, 0);
            if (FollowTarget)
            {
                transform.LookAt(FollowTarget.position + new Vector3(0, 2, 0));
                rb.velocity = (FollowTarget.position - transform.position + hightOffset).normalized * speed;
            }
            else
            {
                rb.velocity = transform.forward * speed;
            }


        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }
        if (FollowTarget != null)
        {
            ShootDamage(FollowTarget);
        }
        Destroy(gameObject);
    }

    void ShootDamage(Transform EnemyTarget)
    {
        float newDamage = 0f;
        Enemy enemy = EnemyTarget.GetComponent<Enemy>();
        if (enemy != null)
        {
            if(type == ElementTypes.Glacier) { newDamage = DamageEngine.ElementCombatAlgorithm(damage, type); enemy.Slow(percentage); }
            if(type == ElementTypes.Ocean) { newDamage = DamageEngine.ElementCombatAlgorithm(damage, type);}
            if(type == ElementTypes.Wind) { newDamage = DamageEngine.ElementCombatAlgorithm(damage, type); }
            if(type == ElementTypes.Fire) { newDamage = DamageEngine.ElementCombatAlgorithm(damage + Random.Range(1, 5), type); } else { newDamage = DamageEngine.ElementCombatAlgorithm(damage, type); }
            if(type == ElementTypes.Desert) { newDamage = DamageEngine.ElementCombatAlgorithm(damage, type); newDamage += Random.Range(0f, 10f); }
            switch (type)
            {
                case ElementTypes.Light:
                    newDamage = DamageEngine.ElementCombatAlgorithm(damage, type);
                    break;
                case ElementTypes.Mountain:
                    newDamage = DamageEngine.ElementCombatAlgorithm(damage, type);
                    break;
                case ElementTypes.Ocean:
                    newDamage = DamageEngine.ElementCombatAlgorithm(damage, type);
                    break;
                case ElementTypes.Poison:
                    newDamage = DamageEngine.ElementCombatAlgorithm(damage, type);
                    break;
                case ElementTypes.Shadow:
                    newDamage = DamageEngine.ElementCombatAlgorithm(damage, type);
                    break;
                case ElementTypes.Thunder:
                    newDamage = DamageEngine.ElementCombatAlgorithm(damage, type);
                    break;
                
            }
            
        }
        enemy.TakeDamage(newDamage);
    }
}
