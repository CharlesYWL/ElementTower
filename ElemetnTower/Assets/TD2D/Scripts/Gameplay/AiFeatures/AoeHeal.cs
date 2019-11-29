using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heal all units in specified radius
/// </summary>
public class AoeHeal : AiFeature
{
	// Amount of healed hp
	public int healAmount = 1;
	// Healing radius
	public CircleCollider2D radius;
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
	// Animator component
	private Animator anim;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Debug.Assert(radius, "Wrong initial settings");
		anim = GetComponentInParent<Animator>();
		cooldownCounter = cooldown;
		radius.enabled = false;
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
		else
		{
			cooldownCounter = 0f;
			// Try to heal somebody
			if (Heal() == true)
			{
				if (anim != null && anim.runtimeAnimatorController != null)
				{
					// Play animation
					foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
					{
						if (clip.name == "Special")
						{
							anim.SetTrigger("special");
							break;
						}
					}
				}
			}
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
	/// Heal all targets in radius.
	/// </summary>
	private bool Heal()
	{
		bool res = false;
		// Searching for units
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius.radius * transform.localScale.x);
		foreach (Collider2D col in cols)
		{
			if (IsTagAllowed(col.tag) == true)
			{
				// If it has Damege Taker component
				DamageTaker target = col.gameObject.GetComponent<DamageTaker>();
				if (target != null)
				{
					// If target injured
					if (target.currentHitpoints < target.hitpoints)
					{
						res = true;
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
			}
		}
		return res;
	}
}
