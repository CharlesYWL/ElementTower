using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main script to control all AI states
/// </summary>
public class AiBehavior : MonoBehaviour
{
	// Navigation agent if it is needed
	[HideInInspector]
	public NavAgent navAgent;
    // This state will be activate on start
	public AiState defaultState;

    // List with all states for this AI
	private List<AiState> aiStates = new List<AiState>();
    // The state that was before
	private AiState previousState;
    // Active state
	private AiState currentState;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		if (navAgent == null)
		{
			// Try to find navigation agent for this object
			navAgent = GetComponentInChildren<NavAgent>();
		}
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		// Enable AI on AiBehavior enabling
		if (currentState != null && currentState.enabled == false)
		{
			EnableNewState();
		}
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		// Disable AI on AiBehavior disabling
		DisableAllStates();
	}

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        // Get all AI states from this gameobject
		AiState[] states = GetComponents<AiState>();
        if (states.Length > 0) 
        {
			foreach (AiState state in states)
            {
                // Add state to list
                aiStates.Add(state);
            }
            if (defaultState != null)
            {
                // Set active and previous states as default state
				previousState = currentState = defaultState;
                if (currentState != null)
                {
                    // Go to active state
                    ChangeState(currentState);
                }
                else
                {
                    Debug.LogError("Incorrect default AI state " + defaultState);
                }
            }
            else
            {
                Debug.LogError("AI have no default state");
            }
        } 
        else 
        {
            Debug.LogError("No AI states found");
        }
    }

    /// <summary>
    /// Set AI to defalt state.
    /// </summary>
    public void GoToDefaultState()
    {
        previousState = currentState;
		currentState = defaultState;
        NotifyOnStateExit();
        DisableAllStates();
        EnableNewState();
        NotifyOnStateEnter();
    }

    /// <summary>
    /// Change Ai state.
    /// </summary>
    /// <param name="state">State.</param>
	public void ChangeState(AiState state)
    {
		if (state != null)
        {
            // Try to find such state in list
			foreach (AiState aiState in aiStates)
            {
                if (state == aiState)
                {
                    previousState = currentState;
                    currentState = aiState;
                    NotifyOnStateExit();
                    DisableAllStates();
                    EnableNewState();
                    NotifyOnStateEnter();
                    return;
                }
            }
            Debug.Log("No such state " + state);
            // If have no such state - go to default state
            GoToDefaultState();
            Debug.Log("Go to default state " + aiStates[0]);
        }
    }

    /// <summary>
    /// Turn off all AI states components.
    /// </summary>
    private void DisableAllStates()
    {
		foreach (AiState aiState in aiStates) 
        {
			aiState.enabled = false;
        }
    }

    /// <summary>
    /// Turn on active AI state component.
    /// </summary>
    private void EnableNewState()
    {
		currentState.enabled = true;
    }

    /// <summary>
    /// Send OnStateExit notification to previous state.
    /// </summary>
    private void NotifyOnStateExit()
    {
		previousState.OnStateExit(previousState, currentState);
    }

    /// <summary>
    /// Send OnStateEnter notification to new state.
    /// </summary>
    private void NotifyOnStateEnter()
    {
		currentState.OnStateEnter(previousState, currentState);
    }

   	/// <summary>
   	/// Raises the trigger event.
   	/// </summary>
   	/// <param name="trigger">Trigger.</param>
   	/// <param name="my">My.</param>
   	/// <param name="other">Other.</param>
	public void OnTrigger(AiState.Trigger trigger, Collider2D my, Collider2D other)
    {
		if (currentState == null)
		{
			Debug.Log("Current sate is null");
		}
		currentState.OnTrigger(trigger, my, other);
    }
}
