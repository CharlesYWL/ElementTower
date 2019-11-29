using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows AI to operate idle state.
/// </summary>
public class AiStateIdle : AiState
{
    /// <summary>
    /// Raises the state enter event.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="newState">New state.</param>
	public override void OnStateEnter(AiState previousState, AiState newState)
    {
		// Stop to moving and turning
		if (aiBehavior.navAgent != null)
		{
			aiBehavior.navAgent.move = false;
			aiBehavior.navAgent.turn = false;
		}
		// If unit has animator
		if (anim != null && anim.runtimeAnimatorController != null)
		{
			// Search for clip
			foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
			{
				if (clip.name == "Idle")
				{
					// Play animation
					anim.SetTrigger("idle");
					break;
				}
			}
		}
    }
}
