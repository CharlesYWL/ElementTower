using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack with melee weapon
/// </summary>
public class AttackMelee : Attack
{
	// Animation controller for this AI
	private Animator anim;
	// Counter for cooldown calculation
	private float cooldownCounter;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		anim = GetComponentInParent<Animator>();
		cooldownCounter = cooldown;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void FixedUpdate()
	{
		if (cooldownCounter < cooldown)
		{
			cooldownCounter += Time.fixedDeltaTime;
		}
	}

	/// <summary>
	/// Attack the specified target if cooldown expired
	/// </summary>
	/// <param name="target">Target.</param>
	public override void TryAttack(Transform target)
	{
		if (cooldownCounter >= cooldown)
		{
			cooldownCounter = 0f;
			Fire(target);
		}
	}

	private IEnumerator FireCoroutine(Transform target)
	{
		if (target != null)
		{
			// If unit has animator
			if (anim != null && anim.runtimeAnimatorController != null)
			{
				// Search for clip
				foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
				{
					if (clip.name == "Attack")
					{
						// Play animation
						anim.SetTrigger("attack");
						break;
					}
				}
			}
			// Delay to synchronize with animation
			yield return new WaitForSeconds(fireDelay);
			if (target != null)
			{
				// If target can receive damage
				DamageTaker damageTaker = target.GetComponent<DamageTaker>();
				if (damageTaker != null)
				{
					damageTaker.TakeDamage(damage);
				}
				// Play sound effect
				if (sfx != null && AudioManager.instance != null)
				{
					AudioManager.instance.PlayAttack(sfx);
				}
			}
		}
	}

	/// <summary>
	/// Make melee attack
	/// </summary>
	/// <param name="target">Target.</param>
	public override void Fire(Transform target)
	{
		StartCoroutine(FireCoroutine(target));
	}

	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
