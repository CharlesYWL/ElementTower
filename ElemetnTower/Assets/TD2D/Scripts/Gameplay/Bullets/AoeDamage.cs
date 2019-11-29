using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Area Of Effect damage on destroing.
/// </summary>
public class AoeDamage : MonoBehaviour
{
	// Percent of AOE damage in part of IBullet damage. 0f = 0%, 1f = 100%
	public float aoeDamageRate = 1f;
    // Area radius
    public float radius = 0.3f;
    // Explosion prefab
    public GameObject explosion;
    // Explosion visual duration
    public float explosionDuration = 1f;
	// Sound effect
	public AudioClip sfx;
	// Allowed objects tags for collision detection
	public List<string> tags = new List<string>();

	// IBullet component of this gameObject to get the damage amount
	private IBullet bullet;
    // Scene is closed now. Forbidden to create new objects on destroy
    private bool isQuitting;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		bullet = GetComponent<IBullet>();
		Debug.Assert(bullet != null, "Wrong initial settings");
	}

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
					// If target can receive damage
					DamageTaker damageTaker = col.gameObject.GetComponent<DamageTaker>();
					if (damageTaker != null)
					{
						// Target takes damage equal bullet damage * AOE Damage Rate
						damageTaker.TakeDamage((int)(Mathf.Ceil(aoeDamageRate * (float)bullet.GetDamage())));
					}
				}
            }
            if (explosion != null)
            {
                // Create explosion visual effect
                Destroy(Instantiate<GameObject>(explosion, transform.position, transform.rotation), explosionDuration);
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
