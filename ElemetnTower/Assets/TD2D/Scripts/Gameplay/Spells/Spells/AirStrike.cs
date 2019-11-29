using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spell like meteorite, starburst and so on
/// </summary>
public class AirStrike : MonoBehaviour
{
	// Delays for FX
	public float[] delaysBeforeDamage = {0.5f};
	// Damage for each hit
	public int damage = 5;
	// Damage radius
	public float radius = 1f;
	// FX prefab
	public GameObject effectPrefab;
	// After this timeout FX will be destroyed
	public float effectDuration = 2f;
	// Sound effect
	public AudioClip sfx;

	// Machine state
	private enum MyState
	{
		WaitForClick,
		WaitForFX
	}
	// Current state for this instance
	private MyState myState = MyState.WaitForClick;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EventManager.StartListening("UserClick", UserClick);
		EventManager.StartListening("UserUiClick", UserUiClick);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("UserClick", UserClick);
		EventManager.StopListening("UserUiClick", UserUiClick);
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Debug.Assert(effectPrefab, "Wrong initial settings");
	}

	/// <summary>
	/// User click handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UserClick(GameObject obj, string param)
	{
		if (myState == MyState.WaitForClick)
		{
			// If clicked not on tower
			if (obj == null || obj.CompareTag("Tower") == false)
			{
				// Create FX
				transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
				Destroy(effect, effectDuration);
				EventManager.TriggerEvent("ActionStart", gameObject, null);
				// Start damege coroutine
				StartCoroutine(DamageCoroutine());
				myState = MyState.WaitForFX;
			}
			else // Clicked on tower
			{
				Destroy(gameObject);
			}
		}
	}

	/// <summary>
	/// User UI click handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UserUiClick(GameObject obj, string param)
	{
		// If clicked on UI instead game map
		if (myState == MyState.WaitForClick)
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Make damage specified by delays and amount.
	/// </summary>
	/// <returns>The coroutine.</returns>
	private IEnumerator DamageCoroutine()
	{
		foreach (float delay in delaysBeforeDamage)
		{
			yield return new WaitForSeconds(delay);

			// Search for targets
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
			foreach (Collider2D col in hits)
			{
				if (col.CompareTag("Enemy") == true || col.CompareTag("FlyingEnemy") == true)
				{
					DamageTaker damageTaker = col.GetComponent<DamageTaker>();
					if (damageTaker != null)
					{
						damageTaker.TakeDamage(damage);
					}
				}
			}
			if (sfx != null && AudioManager.instance != null)
			{
				// Play sound effect
				AudioManager.instance.PlaySound(sfx);
			}
		}

		Destroy(gameObject);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		if (myState == MyState.WaitForClick)
		{
			EventManager.TriggerEvent("ActionCancel", gameObject, null);
		}
		StopAllCoroutines();
	}
}
