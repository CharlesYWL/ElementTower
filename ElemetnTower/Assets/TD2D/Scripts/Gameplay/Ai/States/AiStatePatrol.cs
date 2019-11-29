using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows AI to move with specified path.
/// </summary>
public class AiStatePatrol : AiState
{
	[Space(10)]
	[HideInInspector]
    // Specified path
    public Pathway path;
    // Need to loop path after last point is reached?
    public bool loop = false;
	[HideInInspector]
	// Current destination
	public Waypoint destination;

    /// <summary>
    /// Awake this instance.
    /// </summary>
	public override void Awake()
    {
		base.Awake();
		Debug.Assert (aiBehavior.navAgent, "Wrong initial parameters");
    }

    /// <summary>
    /// Raises the state enter event.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="newState">New state.</param>
	public override void OnStateEnter(AiState previousState, AiState newState)
    {
        if (path == null)
        {
            // If I have no path - try to find it
            path = FindObjectOfType<Pathway>();
            Debug.Assert(path, "Have no path");
        }
        if (destination == null)
        {
            // Get next waypoint from my path
            destination = path.GetNearestWaypoint(transform.position);
        }
        // Set destination for navigation agent
		aiBehavior.navAgent.destination = destination.transform.position;
		// Start moving
		aiBehavior.navAgent.move = true;
		aiBehavior.navAgent.turn = true;
		// If unit has animator
		if (anim != null && anim.runtimeAnimatorController != null)
        {
			// Search for clip
			foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
			{
				if (clip.name == "Move")
				{
					// Play animation
					anim.SetTrigger("move");
					break;
				}
			}
        }
    }

    /// <summary>
    /// Raises the state exit event.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="newState">New state.</param>
	public override void OnStateExit(AiState previousState, AiState newState)
    {
		// Stop moving
		aiBehavior.navAgent.move = false;
		aiBehavior.navAgent.turn = false;
    }

    /// <summary>
    /// Fixed update for this instance.
    /// </summary>
    void FixedUpdate()
    {
        if (destination != null)
        {
            // If destination reached
            if ((Vector2)destination.transform.position == (Vector2)transform.position)
            {
                // Get next waypoint from my path
                destination = path.GetNextWaypoint (destination, loop);
                if (destination != null)
                {
                    // Set destination for navigation agent
					aiBehavior.navAgent.destination = destination.transform.position;
                }
            }
        }
    }

    /// <summary>
    /// Gets the remaining distance on pathway.
    /// </summary>
    /// <returns>The remaining path.</returns>
    public float GetRemainingPath()
    {
        Vector2 distance = destination.transform.position - transform.position;
        return (distance.magnitude + path.GetPathDistance(destination));
    }

	/// <summary>
	/// Updates the destination.
	/// </summary>
	/// <param name="getNearestWaypoint">If set to <c>true</c> get nearest waypoint automaticaly.</param>
	public void UpdateDestination(bool getNearestWaypoint)
	{
		if (getNearestWaypoint == true)
		{
			// Get next waypoint from my path
			destination = path.GetNearestWaypoint(transform.position);
		}
		if (enabled == true)
		{
			// Set destination for navigation agent
			aiBehavior.navAgent.destination = destination.transform.position;
		}
	}
}
