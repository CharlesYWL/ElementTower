using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create exhaust on damage and cover all nearest units in clouds.
/// </summary>
public class CloudOnDamage : MonoBehaviour
{
	// Clouds duration
	public float duration = 3f;
	// Cooldown between exhaust
	public float cooldown = 5f;
	// Exhaust radius
	public float radius = 1f;
	// Prefab for clouds
	public Clouded cloudPrefab;
	// Exhaust visual effect
	public GameObject exhaustFX;
	// Allowed objects tags for collision detection
	public List<string> tags = new List<string>();

	// Machine state
	private enum MyState
	{
		WaitForDamage,
		Clouded,
		Cooldown
	}
	// Current state of this instance
	private MyState myState = MyState.WaitForDamage;
	// Counter for duration and cooldown
	private float counter;

	// Use this for initialization
	void Start()
	{
		Debug.Assert(cloudPrefab && exhaustFX, "Wrong initial settings");
		counter = 0f;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		switch (myState)
		{
		case MyState.Cooldown:	// Wait for cooldown end
			if (counter > 0f)
			{
				counter -= Time.deltaTime;
			}
			else
			{
				counter = 0f;
				myState = MyState.WaitForDamage;
			}
			break;
		case MyState.Clouded:	// Make cloud to get invisible all nearest units
			if (counter > 0f)
			{
				counter -= Time.deltaTime;
			}
			else
			{
				counter = cooldown;
				myState = MyState.Cooldown;
			}
			break;
		}
	}

	/// <summary>
	/// Raises the damage event (from DamageTaker script if it set as trigger)
	/// </summary>
	public void OnDamage()
	{
		// If it is allowed state now
		if (myState == MyState.WaitForDamage)
		{
			myState = MyState.Clouded;
			counter = duration;
			CloudNow();
			// Create visual effect
			GameObject obj = Instantiate(exhaustFX, transform);
			Destroy(obj, duration);
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
	/// Cover nearest units in clouds
	/// </summary>
	private void CloudNow()
	{
		// Searching for units
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
		foreach (Collider2D col in cols)
		{
			if (IsTagAllowed(col.tag) == true)
			{
				// Add cloud to unit
				Clouded clouded = Instantiate(cloudPrefab, col.gameObject.transform);
				clouded.duration = duration;
			}
		}
	}
}
