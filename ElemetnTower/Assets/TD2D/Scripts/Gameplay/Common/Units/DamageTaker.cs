using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This target can receive damage.
/// </summary>
public class DamageTaker : MonoBehaviour
{
    // Start hitpoints
    public int hitpoints = 1;
    // Remaining hitpoints
	[HideInInspector]
    public int currentHitpoints;
    // Damage visual effect duration
    public float damageDisplayTime = 0.2f;
    // Helth bar object
    public Transform healthBar;
	// SendMessage will trigger on damage taken
	public bool isTrigger;
	// Die sound effect
	public AudioClip dieSfx;

    // Image of this object
    private SpriteRenderer sprite;
    // Visualisation of hit or heal is in progress
	private bool coroutineInProgress;
	// Original width of health bar (full hp)
    private float originHealthBarWidth;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        currentHitpoints = hitpoints;
        sprite = GetComponentInChildren<SpriteRenderer>();
        Debug.Assert(sprite && healthBar, "Wrong initial parameters");
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
    void Start()
    {
        originHealthBarWidth = healthBar.localScale.x;
    }

    /// <summary>
    /// Take damage.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void TakeDamage(int damage)
    {
		if (damage > 0)
		{
			if (this.enabled == true)
			{
				if (currentHitpoints > damage)
				{
					// Still alive
					currentHitpoints -= damage;
					UpdateHealthBar();
					// If no coroutine now
					if (coroutineInProgress == false)
					{
						// Damage visualisation
						StartCoroutine(DisplayDamage());
					}
					if (isTrigger == true)
					{
						// Notify other components of this game object
						SendMessage("OnDamage");
					}
				}
				else
				{
					// Die
					currentHitpoints = 0;
					UpdateHealthBar();
					Die();
				}
			}
		}
		else // damage < 0
		{
			// Healed
			currentHitpoints = Mathf.Min(currentHitpoints - damage, hitpoints);
			UpdateHealthBar();
		}
    }

	/// <summary>
	/// Updates the health bar width.
	/// </summary>
    private void UpdateHealthBar()
    {
        float healthBarWidth = originHealthBarWidth * currentHitpoints / hitpoints;
        healthBar.localScale = new Vector2(healthBarWidth, healthBar.localScale.y);
    }

    /// <summary>
    /// Die this instance.
    /// </summary>
    public void Die()
    {
		EventManager.TriggerEvent("UnitKilled", gameObject, null);
		StartCoroutine(DieCoroutine());
    }

	private IEnumerator DieCoroutine()
	{
		if (dieSfx != null && AudioManager.instance != null)
		{
			AudioManager.instance.PlayDie(dieSfx);
		}
		foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
		{
			col.enabled = false;
		}
		GetComponent<AiBehavior>().enabled = false;
		GetComponent<NavAgent>().enabled = false;
		GetComponent<EffectControl>().enabled = false;
		Animator anim = GetComponent<Animator>();
		// If unit has animator
		if (anim != null && anim.runtimeAnimatorController != null)
		{
			// Search for clip
			foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
			{
				if (clip.name == "Die")
				{
					// Play animation
					anim.SetTrigger("die");
					yield return new WaitForSeconds(clip.length);
					break;
				}
			}
		}
		Destroy(gameObject);
	}

    /// <summary>
    /// Damage visualisation.
    /// </summary>
    /// <returns>The damage.</returns>
    IEnumerator DisplayDamage()
    {
        coroutineInProgress = true;
        Color originColor = sprite.color;
        float counter;
        // Set color to black and return to origin color over time
		for (counter = 0f; counter < damageDisplayTime; counter += Time.fixedDeltaTime)
        {
            sprite.color = Color.Lerp(originColor, Color.black, Mathf.PingPong(counter, damageDisplayTime / 2f));
			yield return new WaitForFixedUpdate();
        }
        sprite.color = originColor;
        coroutineInProgress = false;
    }

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		EventManager.TriggerEvent("UnitDie", gameObject, null);
		StopAllCoroutines();
	}
}
