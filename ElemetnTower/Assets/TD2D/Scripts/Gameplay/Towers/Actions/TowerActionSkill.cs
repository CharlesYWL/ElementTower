using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tower active skill.
/// </summary>
public class TowerActionSkill : TowerActionCooldown
{
	// Prefab for skill effect
	public TowerSkillEffect effectPrefab;
	// Radius
	[HideInInspector]
	public CircleCollider2D radiusCollider;

	// Tower
	private Tower tower;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		tower = GetComponentInParent<Tower>();
		Debug.Assert(effectPrefab && tower, "Wrong initial settings");
	}

	/// <summary>
	/// Clicked this instance.
	/// </summary>
	protected override void Clicked()
	{
		base.Clicked();
		// Apply skill effect
		TowerSkillEffect towerSkillEffect = Instantiate(effectPrefab);
		towerSkillEffect.tower = tower;
		AttackRanged attackRanged = tower.GetComponentInChildren<AttackRanged>();
		if (attackRanged != null)
		{
			towerSkillEffect.radiusCollider = attackRanged.GetComponent<CircleCollider2D>();
		}
		else if (tower.range != null)
		{
			towerSkillEffect.radiusCollider = tower.range.GetComponent<CircleCollider2D>();
		}
	}
}
