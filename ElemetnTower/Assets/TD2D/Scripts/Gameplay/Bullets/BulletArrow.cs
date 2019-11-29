using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Arrow fly trajectory.
/// </summary>
public class BulletArrow : MonoBehaviour, IBullet
{
    // Damage amount
	[HideInInspector] int damage = 1;
    // Maximum life time
    public float lifeTime = 3f;
    // Starting speed
    public float speed = 3f;
    // Constant acceleration
    public float speedUpOverTime = 0.5f;
    // If target is close than this distance - it will be hitted
    public float hitDistance = 0.2f;
    // Ballistic trajectory offset (in distance to target)
    public float ballisticOffset = 0.5f;
    // Do not rotate bullet during fly
    public bool freezeRotation = false;
	// This bullet don't deal damage to single target. Only AOE damage if it is
	public bool aoeDamageOnly = false;

    // From this position bullet was fired
    private Vector2 originPoint;
    // Aimed target
    private Transform target;
    // Last target's position
    private Vector2 aimPoint;
    // Current position without ballistic offset
    private Vector2 myVirtualPosition;
    // Position on last frame
    private Vector2 myPreviousPosition;
    // Counter for acceleration calculation
    private float counter;
    // Image of this bullet
    private SpriteRenderer sprite;

    /// <summary>
    /// Set damage amount for this bullet.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

	/// <summary>
	/// Get damage amount for this bullet.
	/// </summary>
	/// <returns>The damage.</returns>
	public int GetDamage()
	{
		return damage;
	}

    /// <summary>
    /// Fire bullet towards specified target.
    /// </summary>
    /// <param name="target">Target.</param>
    public void Fire(Transform target)
    {
        sprite = GetComponent<SpriteRenderer>();
        // Disable sprite on first frame beqause we do not know fly direction yet
        sprite.enabled = false;
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        this.target = target;
        aimPoint = target.position;
        // Destroy bullet after lifetime
        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void FixedUpdate ()
    {
		counter += Time.fixedDeltaTime;
        // Add acceleration
		speed += Time.fixedDeltaTime * speedUpOverTime;
        if (target != null)
        {
            aimPoint = target.position;
        }
        // Calculate distance from firepoint to aim
        Vector2 originDistance = aimPoint - originPoint;
        // Calculate remaining distance
        Vector2 distanceToAim = aimPoint - (Vector2)myVirtualPosition;
        // Move towards aim
        myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * speed / originDistance.magnitude);
        // Add ballistic offset to trajectory
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);
        // Rotate bullet towards trajectory
		LookAtDirection2D((Vector2)transform.position - myPreviousPosition);
        myPreviousPosition = transform.position;
        sprite.enabled = true;
        // Close enough to hit
        if (distanceToAim.magnitude <= hitDistance)
        {
            if (target != null)
            {
				// If bullet must deal damage to single target
				if (aoeDamageOnly == false)
				{
					// If target can receive damage
					DamageTaker damageTaker = target.GetComponent<DamageTaker>();
					if (damageTaker != null)
					{
						damageTaker.TakeDamage(damage);
					}
				}
            }
            // Destroy bullet
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Looks at direction2d.
    /// </summary>
    /// <param name="direction">Direction.</param>
    private void LookAtDirection2D(Vector2 direction)
    {
        if (freezeRotation == false)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    /// <summary>
    /// Adds ballistic offset to trajectory.
    /// </summary>
    /// <returns>The ballistic offset.</returns>
    /// <param name="originDistance">Origin distance.</param>
    /// <param name="distanceToAim">Distance to aim.</param>
    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset > 0f)
        {
            // Calculate sinus offset
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            // Add offset to trajectory
            return (Vector2)myVirtualPosition + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            return myVirtualPosition;
        }
    }
}
