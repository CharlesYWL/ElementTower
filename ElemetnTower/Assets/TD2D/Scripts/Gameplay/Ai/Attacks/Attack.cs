using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic class for attacks types.
/// </summary>
public class Attack : MonoBehaviour
{
	// Damage amount
	public int damage = 1;
	// Cooldown between attacks
	public float cooldown = 1f;
	// Delay for fire animation
	public float fireDelay = 0f;
	// Sound effect
	public AudioClip sfx;

	public virtual void TryAttack(Transform target)
	{

	}

	public virtual void Fire(Transform target)
	{

	}
}
