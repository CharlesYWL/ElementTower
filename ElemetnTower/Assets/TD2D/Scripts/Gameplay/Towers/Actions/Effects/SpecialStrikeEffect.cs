using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special strike effect. Attack one random enemy in radius.
/// </summary>
public class SpecialStrikeEffect : TowerSkillEffect
{
	// Prefab for bullet
	public GameObject bulletPrefab;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Debug.Assert(bulletPrefab, "Wrong initial settings");
		AttackRanged attack = tower.GetComponentInChildren<AttackRanged>();
		if (attack != null)
		{
			float radius = radiusCollider.radius * Mathf.Max(radiusCollider.transform.localScale.x, radiusCollider.transform.localScale.y);
			// Get random enemy
			Collider2D enemy = Physics2D.OverlapCircle(radiusCollider.transform.position, radius, 1 << LayerMask.NameToLayer("Enemy"));
			if (enemy != null)
			{
				GameObject defaultBulletPrefab = attack.arrowPrefab;
				attack.arrowPrefab = bulletPrefab;
				// Attack it
				attack.Fire(enemy.gameObject.transform);
				attack.arrowPrefab = defaultBulletPrefab;
			}
		}
		else
		{
			Debug.Log("This tower can not use attack skills");
		}
		Destroy(gameObject);
	}
}
