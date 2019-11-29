using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeEffect : MonoBehaviour
{
	// Name of effect
	public string effectName;
	// Effect modifier (-1f = -100%, 0f = 0%, 1f = 100%)
	public float modifier = 1f;
	// Effect duration
	public float duration = 3f;
	// Area radius
	public float radius = 1f;
	// Explosion FX prefab
	public GameObject explosionFx;
	// Explosion FX duration
	public float explosionFxDuration = 1f;
	// Effect FX prefab
	public GameObject effectFx;
	// Sound effect
	public AudioClip sfx;
	// Allowed objects tags for collision detection
	public List<string> tags = new List<string>();

	// Scene is closed now. Forbidden to create new objects on destroy
	private bool isQuitting;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EventManager.StartListening("SceneQuit", SceneQuit);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("SceneQuit", SceneQuit);
	}

	/// <summary>
	/// Raises the application quit event.
	/// </summary>
	void OnApplicationQuit()
	{
		isQuitting = true;
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		// If scene is in progress
		if (isQuitting == false)
		{
			// Find all colliders in specified radius
			Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
			foreach (Collider2D col in cols)
			{
				if (IsTagAllowed(col.tag) == true)
				{
					EffectControl effectControl = col.gameObject.GetComponent<EffectControl>();
					if (effectControl != null)
					{
						effectControl.AddEffect(effectName, modifier, duration, effectFx);
					}
				}
			}
			if (explosionFx != null)
			{
				// Create explosion visual effect
				Destroy(Instantiate<GameObject>(explosionFx, transform.position, transform.rotation), explosionFxDuration);
			}
			if (sfx != null && AudioManager.instance != null)
			{
				// Play sfx
				AudioManager.instance.PlaySound(sfx);
			}
		}
	}

	/// <summary>
	/// Raises on scene quit.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void SceneQuit(GameObject obj, string param)
	{
		isQuitting = true;
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
}
