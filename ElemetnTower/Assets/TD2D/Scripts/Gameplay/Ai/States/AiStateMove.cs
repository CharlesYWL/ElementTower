using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows AI to operate move towards destination.
/// </summary>
public class AiStateMove : AiState
{
	[Space(10)]

    // Go to this state if passive event occures
	public AiState passiveAiState;
	// End point for moving
	[HideInInspector]
	public Transform destination;

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
        // Set destination for navigation agent
		aiBehavior.navAgent.destination = destination.position;
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
        // If destination reached
        if ((Vector2)transform.position == (Vector2)destination.position)
        {
            // Look at required direction
			aiBehavior.navAgent.LookAt(destination.right);
            // Go to passive state
            aiBehavior.ChangeState(passiveAiState);
        }
    }
}
