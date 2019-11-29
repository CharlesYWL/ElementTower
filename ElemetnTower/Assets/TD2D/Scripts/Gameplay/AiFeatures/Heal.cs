using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heal random target in specified range (collider)
/// </summary>
public class Heal : AiFeature
{
	// Amount of healed hp
	public int healAmount = 1;
	// Delay between healing
	public float cooldown = 3f;
	// Visual effect for healing
	public GameObject healVisualPrefab;
	// Duration for heal visual effect
	public float healVisualDuration = 1f;
	// Allowed objects tags for collision detection
	public List<string> tags = new List<string>();

	// Counter for cooldown
	private float cooldownCounter;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		cooldownCounter = cooldown;
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate()
	{
		if (cooldownCounter < cooldown)
		{
			cooldownCounter += Time.fixedDeltaTime;
		}
	}

	/// <summary>
	/// Determines whether this instance is tag allowed the specified tag.
	/// </summary>
	/// <returns><c>true</c> if this instance is tag allowed the specified tag; otherwise, <c>false</c>.</returns>
	/// <param name="tag">Tag.</param>
	private bool IsTagAllowed(string tag)
	{
		bool res = false;
		if (tags.Count > 0)
		{
			foreach (string str in tags)
			{
				if (str == tag)
				{
					res = true;
					break;
				}
			}
		}
		else
		{
			res = true;
		}
		return res;
	}

	/// <summary>
	/// Heal specified target if cooldown expired.
	/// </summary>
	/// <param name="target">Target.</param>
	private void TryToHeal(DamageTaker target)
	{
		// If cooldown expired
		if (cooldownCounter >= cooldown)
		{
			cooldownCounter = 0f;
			target.TakeDamage(-healAmount);
			if (healVisualPrefab != null)
			{
				// Create visual healing effect on target
				GameObject effect = Instantiate(healVisualPrefab, target.transform);
				// And destroy it after specified timeout
				Destroy(effect, healVisualDuration);
			}
		}
	}

	/// <summary>
	/// Raises the trigger stay2d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerStay2D(Collider2D other)
	{
		if (IsTagAllowed(other.tag) == true)
		{
			// If it has Damege Taker component
			DamageTaker target = other.gameObject.GetComponent<DamageTaker>();
			if (target != null)
			{
				// If target injured
				if (target.currentHitpoints < target.hitpoints)
				{
					TryToHeal(target);
				}
			}
		}
	}
}
