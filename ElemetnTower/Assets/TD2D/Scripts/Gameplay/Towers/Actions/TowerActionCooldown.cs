using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tower action with cooldown.
/// </summary>
public class TowerActionCooldown : TowerAction
{
	// Action cooldown
	public float cooldown = 10f;
	// Icon for cooldown state
	public GameObject cooldownIcon;
	// Cooldown counter (UI Text)
	public Text cooldownText;

	// Machine state
	private enum MyState
	{
		Active,
		Cooldown
	}
	// Current state for this instance
	private MyState myState = MyState.Active;
	// Time when cooldown was started
	private float cooldownStartTime;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Debug.Assert(cooldownIcon && cooldownText, "Wrong initial settings");
		StopCooldown();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		if (myState == MyState.Cooldown)
		{
			float cooldownCounter = Time.time - cooldownStartTime;
			if (cooldownCounter < cooldown)
			{
				UpdateCooldownText(cooldown - cooldownCounter);
			}
			else
			{
				StopCooldown();
			}
		}
	}

	/// <summary>
	/// Starts the cooldown.
	/// </summary>
	private void StartCooldown()
	{
		myState = MyState.Cooldown;
		cooldownStartTime = Time.time;
		enabledIcon.SetActive(false);
		cooldownIcon.gameObject.SetActive(true);
		cooldownText.gameObject.SetActive(true);
	}

	/// <summary>
	/// Stops the cooldown.
	/// </summary>
	private void StopCooldown()
	{
		myState = MyState.Active;
		enabledIcon.SetActive(true);
		cooldownIcon.gameObject.SetActive(false);
		cooldownText.gameObject.SetActive(false);
	}

	/// <summary>
	/// Updates the cooldown counter text.
	/// </summary>
	private void UpdateCooldownText(float cooldownCounter)
	{
		cooldownText.text = ((int)Mathf.Ceil(cooldownCounter)).ToString();
	}

	/// <summary>
	/// Clicked this instance.
	/// </summary>
	protected override void Clicked()
	{
		StartCooldown();
	}
}
