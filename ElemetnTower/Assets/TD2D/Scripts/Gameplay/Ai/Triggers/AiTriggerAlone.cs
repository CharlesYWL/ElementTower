using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTriggerAlone : MonoBehaviour
{
	// Alone time before trigger
	public float aloneDuration = 1f;
	public List<Component> receivers = new List<Component>();
	// Allowed objects tags for collision detection
	public List<string> tags = new List<string>();

	// My AiBehavior
	private AiBehavior ai;
	// My collider
	private Collider2D col;
	// Alone duration countrer
	private float counter;
	// Already triggered
	private bool triggered;

	void Awake()
	{
		ai = GetComponentInParent<AiBehavior>();
		col = GetComponent<Collider2D>();
		Debug.Assert(ai && col, "Wrong initial parameters");
		col.enabled = false;
	}

	void Start()
	{
		col.enabled = true;
		counter = aloneDuration;
		triggered = false;
	}

	/// <summary>
	/// Raises the trigger stay2d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject != ai.gameObject && IsTagAllowed(other.tag) == true)
		{
			if (triggered == true)
			{
				foreach (Component receiver in receivers)
				{
					receiver.SendMessage("OnTriggerAloneEnd");
				}
			}
			triggered = false;
			counter = aloneDuration;
		}
	}

	void FixedUpdate()
	{
		if (triggered == false)
		{
			if (counter > 0f)
			{
				counter -= Time.fixedDeltaTime;
			}
			else
			{
				triggered = true;
				counter = 0f;
				foreach (Component receiver in receivers)
				{
					receiver.SendMessage("OnTriggerAloneStart");
				}
			}
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
}
