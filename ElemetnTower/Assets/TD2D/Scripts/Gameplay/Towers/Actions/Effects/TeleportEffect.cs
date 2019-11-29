using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleport target back on pathway.
/// </summary>
public class TeleportEffect : TowerSkillEffect
{
	// Offset on pathway
	public float teleportOffset = 1f;
	// Visual effect
	public GameObject fxPrefab;
	// Visual effect duration
	public float fxDuration = 3f;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		float radius = radiusCollider.radius * Mathf.Max(radiusCollider.transform.localScale.x, radiusCollider.transform.localScale.y);
		// Get random enemy
		Collider2D enemy = Physics2D.OverlapCircle(radiusCollider.transform.position, radius, 1 << LayerMask.NameToLayer("Enemy"));
		if (enemy != null)
		{
			if (fxPrefab != null)
			{
				// Visual effect in teleport position
				GameObject fx = Instantiate(fxPrefab);
				fx.transform.position = enemy.gameObject.transform.position;
				Destroy(fx, fxDuration);
			}
			AiStatePatrol aiStatePatrol = enemy.gameObject.GetComponent<AiStatePatrol>();
			// Offset target on pathway
			Vector2 teleportPosition = aiStatePatrol.path.GetOffsetPosition(ref aiStatePatrol.destination, aiStatePatrol.transform.position, teleportOffset);
			aiStatePatrol.transform.position = new Vector3(teleportPosition.x, teleportPosition.y, aiStatePatrol.transform.position.z);
			// Update patrol destination
			aiStatePatrol.UpdateDestination(false);
			if (fxPrefab != null)
			{
				// Visual effect in new position
				GameObject fx = Instantiate(fxPrefab);
				fx.transform.position = enemy.gameObject.transform.position;
				Destroy(fx, fxDuration);
			}
		}
		Destroy(gameObject);
	}
}
