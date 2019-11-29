using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dynamic filter for AI collision mask
/// </summary>
public class AiTriggerCollider : MonoBehaviour
{
    // Allowed objects tags for collision detection
    public List<string> tags = new List<string>();

    // My collider
    private Collider2D col;
    // AI behaviour component in parent object
    private AiBehavior aiBehavior;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        col = GetComponent<Collider2D>();
        aiBehavior = GetComponentInParent<AiBehavior>();
        Debug.Assert(col && aiBehavior, "Wrong initial parameters");
		col.enabled = false;
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		col.enabled = true;
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
    /// Raises the trigger enter2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            // Notify AI behavior about this event
			aiBehavior.OnTrigger(AiState.Trigger.TriggerEnter, col, other);
        }
    }

    /// <summary>
    /// Raises the trigger stay2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            // Notify AI behavior about this event
			aiBehavior.OnTrigger(AiState.Trigger.TriggerStay, col, other);
        }
    }

    /// <summary>
    /// Raises the trigger exit2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            // Notify AI behavior about this event
			aiBehavior.OnTrigger(AiState.Trigger.TriggerExit, col, other);
        }
    }
}
