using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack with ranged weapon
/// </summary>
public class AttackRanged : Attack
{
	// Prefab for arrows
	public GameObject arrowPrefab;
	// From this position arrows will fired
	public Transform firePoint;

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
		Debug.Assert(arrowPrefab && firePoint, "Wrong initial parameters");
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

	private IEnumerator FireCoroutine(Transform target, GameObject bulletPrefab)
	{
		if (target != null && bulletPrefab != null)
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
				// Create arrow
				GameObject arrow = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
				IBullet bullet = arrow.GetComponent<IBullet>();
				bullet.SetDamage(damage);
				bullet.Fire(target);
				// Play sound effect
				if (sfx != null && AudioManager.instance != null)
				{
					AudioManager.instance.PlayAttack(sfx);
				}
			}
		}
	}

	/// <summary>
	/// Make ranged attack
	/// </summary>
	/// <param name="target">Target.</param>
	public override void Fire(Transform target)
	{
		StartCoroutine(FireCoroutine(target, arrowPrefab));
	}

	/// <summary>
	/// Make ranged attack with special bullet
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="bulletPrefab">Bullet prefab.</param>
	public void Fire(Transform target, GameObject bulletPrefab)
	{
		StartCoroutine(FireCoroutine(target, bulletPrefab));
	}

	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
