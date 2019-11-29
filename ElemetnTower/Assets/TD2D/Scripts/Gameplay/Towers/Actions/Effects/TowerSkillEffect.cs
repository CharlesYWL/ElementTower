using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic class for tower skill effect.
/// </summary>
public class TowerSkillEffect : MonoBehaviour
{
	[HideInInspector]
	// Tower attack radius
	public CircleCollider2D radiusCollider;
	[HideInInspector]
	// Tower
	public Tower tower;
}
