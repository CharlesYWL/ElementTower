using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moving and turning operation.
/// </summary>
public class NavAgent : MonoBehaviour
{
    // Speed im m/s
    public float speed = 1f;
    // Can moving
	[HideInInspector]
    public bool move = true;
    // Can turning
	[HideInInspector]
    public bool turn = true;
    // Destination position
    [HideInInspector]
    public Vector2 destination;
    // Velocity vector
    [HideInInspector]
    public Vector2 velocity;

	private Animator anim;
    // Position on last frame
    private Vector2 prevPosition;

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        prevPosition = transform.position;
		anim = GetComponentInParent<Animator>();
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void FixedUpdate()
    {
        // If moving is allowed
        if (move == true)
        {
            // Move towards destination point
			transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
        }
        // Calculate velocity
        velocity = (Vector2)transform.position - prevPosition;
		velocity /= Time.fixedDeltaTime;
        // If turning is allowed
        if (turn == true)
        {
            SetSpriteDirection(destination - (Vector2)transform.position);
        }
        // Save last position
        prevPosition = transform.position;
    }

    /// <summary>
    /// Sets sprite direction on x axis.
    /// </summary>
    /// <param name="direction">Direction.</param>
    private void SetSpriteDirection(Vector2 direction)
    {
		// Flip gameobject dependings on direction
        if (direction.x > 0f && transform.localScale.x < 0f) // To the right
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0f && transform.localScale.x > 0f) // To the left
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

		// Set direction for 8d animation (if used coresponding animator)
		if (anim != null)
		{
			foreach (AnimatorControllerParameter param in anim.parameters)
			{
				if (param.name == "directionY")
				{
					float directionY = direction.y / (Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
					anim.SetFloat("directionY", directionY);
					break;
				}
			}
		}
    }

    /// <summary>
    /// Looks at direction.
    /// </summary>
    /// <param name="direction">Direction.</param>
    public void LookAt(Vector2 direction)
    {
        SetSpriteDirection(direction);
    }

    /// <summary>
    /// Looks at target.
    /// </summary>
    /// <param name="target">Target.</param>
    public void LookAt(Transform target)
    {
        SetSpriteDirection(target.position - transform.position);
    }
}
