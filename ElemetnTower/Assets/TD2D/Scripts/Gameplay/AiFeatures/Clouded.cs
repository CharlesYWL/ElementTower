using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit get a cloud that makes him invisible
/// </summary>
public class Clouded : MonoBehaviour
{
	[HideInInspector]
	// Cloud duration
	public float duration;

	// Collider component of this game object
	private Collider2D col;
	// Counter for clouded duration
	private float counter;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		col = GetComponentInParent<Collider2D>();
		Debug.Assert(col, "Wrong initial settings");
		counter = duration;
		// Make unit invisible (dont collide with other units)
		col.enabled = false;
	}
	
	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate()
	{
		if (counter > 0f)
		{
			counter -= Time.fixedDeltaTime;
		}
		else
		{
			// Make unit visible
			col.enabled = true;
			Destroy(gameObject);
		}
	}
}
