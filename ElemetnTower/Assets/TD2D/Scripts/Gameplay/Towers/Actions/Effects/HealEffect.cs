using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heal effect.
/// </summary>
public class HealEffect : TowerSkillEffect
{
	// Visual effect prefab
	public GameObject healFxPrefab;
	// Visual effect duration
	public float fxDuration = 1f;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		DefendersSpawner defendersSpawner = tower.GetComponent<DefendersSpawner>();
		if (defendersSpawner != null)
		{
			// Get all active defenders
			foreach (GameObject defender in defendersSpawner.defPoint.GetDefenderList())
			{
				DamageTaker damageTaker = defender.GetComponent<DamageTaker>();
				// Heal it
				damageTaker.TakeDamage(-damageTaker.hitpoints);
				if (healFxPrefab != null)
				{
					// Create heal visual effect
					Destroy(Instantiate(healFxPrefab, defender.transform), fxDuration);
				}
			}
		}
		else
		{
			Debug.Log("This tower can not use heal skills");
		}
		Destroy(gameObject);
	}
}
