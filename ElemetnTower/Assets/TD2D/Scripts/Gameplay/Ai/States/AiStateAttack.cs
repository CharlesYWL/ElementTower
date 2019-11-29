using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows AI to attack targets.
/// </summary>
public class AiStateAttack : AiState
{
	[Space(10)]
    // Attack target closest to the capture point
    public bool useTargetPriority = false;
    // Go to this state if passive event occures
	public AiState passiveAiState;

    // Target for attack
    private GameObject target;
    // List with potential targets finded during this frame
    private List<GameObject> targetsList = new List<GameObject>();
    // My melee attack type if it is
    private Attack meleeAttack;
    // My ranged attack type if it is
    private Attack rangedAttack;
    // Type of last attack is made
    private Attack myLastAttack;
    // Allows to await new target for one frame before exit from this state
    private bool targetless;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    public override void Awake()
    {
		base.Awake();
        meleeAttack = GetComponentInChildren<AttackMelee>() as Attack;
        rangedAttack = GetComponentInChildren<AttackRanged>() as Attack;
    }

	/// <summary>
	/// Raises the state enter event.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	/// <param name="newState">New state.</param>
	public override void OnStateEnter(AiState previousState, AiState newState)
	{
		// Stop to moving
		if (aiBehavior.navAgent != null)
		{
			aiBehavior.navAgent.move = false;
		}
	}

    /// <summary>
    /// Raises the state exit event.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="newState">New state.</param>
	public override void OnStateExit(AiState previousState, AiState newState)
    {
        LoseTarget();
    }

    /// <summary>
    /// FixedUpdate for this instance.
    /// </summary>
    void FixedUpdate()
    {
        // If have no target - try to get new target
        if ((target == null) && (targetsList.Count > 0))
        {
            target = GetTopmostTarget();
        }
        // There are no targets around
        if (target == null)
        {
            if (targetless == false)
            {
                targetless = true;
            }
            else
            {
                // If have no target more than one frame - exit from this state
                aiBehavior.ChangeState(passiveAiState);
            }
        }
    }

    /// <summary>
    /// Gets top priority target from list.
    /// </summary>
    /// <returns>The topmost target.</returns>
    private GameObject GetTopmostTarget()
    {
        GameObject res = null;
        if (useTargetPriority == true) // Get target with minimum distance to capture point
        {
            float minPathDistance = float.MaxValue;
            foreach (GameObject ai in targetsList)
            {
                if (ai != null)
                {
                    AiStatePatrol aiStatePatrol = ai.GetComponent<AiStatePatrol>();
                    float distance = aiStatePatrol.GetRemainingPath();
                    if (distance < minPathDistance)
                    {
                        minPathDistance = distance;
                        res = ai;
                    }
                }
            }
        }
        else // Get first target from list
        {
            res = targetsList[0];
        }
        // Clear list of potential targets
        targetsList.Clear();
        return res;
    }

    /// <summary>
    /// Loses the current target.
    /// </summary>
    private void LoseTarget()
    {
        target = null;
        targetless = false;
        myLastAttack = null;
    }

	/// <summary>
	/// Raises the trigger event.
	/// </summary>
	/// <param name="trigger">Trigger.</param>
	/// <param name="my">My.</param>
	/// <param name="other">Other.</param>
	public override bool OnTrigger(AiState.Trigger trigger, Collider2D my, Collider2D other)
	{
		if (base.OnTrigger(trigger, my, other) == false)
		{
			switch (trigger)
			{
			case AiState.Trigger.TriggerStay:
				TriggerStay(my, other);
				break;
			case AiState.Trigger.TriggerExit:
				TriggerExit(my, other);
				break;
			}
		}
		return false;
	}

    /// <summary>
    /// Triggers the stay.
    /// </summary>
    /// <param name="my">My.</param>
    /// <param name="other">Other.</param>
	private void TriggerStay(Collider2D my, Collider2D other)
    {
        if (target == null) // Add new target to potential targets list
        {
            targetsList.Add(other.gameObject);
        }
        else // Attack current target
        {
            // If this is my current target
            if (target == other.gameObject)
            {
                if (my.name == "MeleeAttack") // If target is in melee attack range
                {
                    // If I have melee attack type
                    if (meleeAttack != null)
                    {
						if (aiBehavior.navAgent != null)
						{
							// Look at target
							aiBehavior.navAgent.LookAt(target.transform);
						}
                        // Remember my last attack type
                        myLastAttack = meleeAttack as Attack;
                        // Try to make melee attack
                        meleeAttack.TryAttack(other.transform);
                    }
                }
                else if (my.name == "RangedAttack") // If target is in ranged attack range
                {
                    // If I have ranged attack type
                    if (rangedAttack != null)
                    {
                        // If target not in melee attack range
                        if ((meleeAttack == null)
                            || ((meleeAttack != null) && (myLastAttack != meleeAttack)))
                        {
							if (aiBehavior.navAgent != null)
							{
								// Look at target
								aiBehavior.navAgent.LookAt(target.transform);
							}
                            // Remember my last attack type
                            myLastAttack = rangedAttack as Attack;
                            // Try to make ranged attack
                            rangedAttack.TryAttack(other.transform);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Triggers the exit.
    /// </summary>
    /// <param name="my">My.</param>
    /// <param name="other">Other.</param>
	private void TriggerExit(Collider2D my, Collider2D other)
    {
        if (other.gameObject == target)
        {
            // Lose my target if it quit attack range
            LoseTarget();
        }
    }
}
