using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows tower to spawn new obects with cooldown.
/// </summary>
public class DefendersSpawner : MonoBehaviour
{
    // Cooldown for between spawns
    public float cooldown = 10f;
    // Max number of spawned obects in buffer
    public int maxNum = 2;
    // Spawned object prefab
    public GameObject prefab;
    // Position for spawning
    public Transform spawnPoint;
	[HideInInspector]
	// Defend points for this tower
	public DefendPoint defPoint;

    // Counter for cooldown calculation
    private float cooldownCounter;
	// Animator of this instance
	private Animator anim;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		anim = GetComponent<Animator>();
		Debug.Assert(spawnPoint, "Wrong initial settings");
		BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
		defPoint = buildingPlace.GetComponentInChildren<DefendPoint>();
		cooldownCounter = cooldown;
		// Upgrade all existing defenders on tower build
		Dictionary<GameObject, Transform> oldDefenders = new Dictionary<GameObject, Transform>();
		foreach (KeyValuePair<GameObject, Transform> pair in defPoint.activeDefenders)
		{
			oldDefenders.Add(pair.Key, pair.Value);
		}
		defPoint.activeDefenders.Clear();
		foreach (KeyValuePair<GameObject, Transform> pair in oldDefenders)
		{
			// Spawn new defender in the same place
			Spawn(pair.Key.transform, pair.Value);
		}
		// Destroy old defenders
		foreach (KeyValuePair<GameObject, Transform> pair in oldDefenders)
		{
			Destroy(pair.Key);
		}
	}

    /// <summary>
    /// Update this instance.
    /// </summary>
    void FixedUpdate()
    {
		cooldownCounter += Time.fixedDeltaTime;
        if (cooldownCounter >= cooldown)
        {
            // Try to spawn new object on cooldown
            if (TryToSpawn() == true)
            {
                cooldownCounter = 0f;
            }
            else
            {
                cooldownCounter = cooldown;
            }
        }
    }

    /// <summary>
    /// Try to spawn new object.
    /// </summary>
    /// <returns><c>true</c>, if to spawn was tryed, <c>false</c> otherwise.</returns>
    private bool TryToSpawn()
    {
        bool res = false;
        // If spawned objects number less then max allowed number
		if ((prefab != null) && (defPoint.activeDefenders.Count < maxNum))
        {
			Transform destination = defPoint.GetFreeDefendPosition();
            // If there are free defend position
            if (destination != null)
            {
				// Spawn new defender
				Spawn(spawnPoint, destination);
                res = true;
            }
        }
        return res;
    }

	/// <summary>
	/// Spawn in the specified position and destination.
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="destination">Destination.</param>
	private void Spawn(Transform position, Transform destination)
	{
		// Create new obect
		GameObject obj = Instantiate<GameObject>(prefab, position.position, position.rotation);
		obj.name = prefab.name;
		// Set destination position
		obj.GetComponent<AiStateMove>().destination = destination;
		// Add spawned object to list
		defPoint.activeDefenders.Add(obj, destination);
		// Play animation
		if (anim != null && anim.runtimeAnimatorController != null)
		{
			foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
			{
				if (clip.name == "Spawn")
				{
					anim.SetTrigger("spawn");
					break;
				}
			}
		}
	}
}
