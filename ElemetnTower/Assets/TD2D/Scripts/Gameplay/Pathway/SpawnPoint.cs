using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Enemy spawner.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
	/// <summary>
	/// Enemy wave structure.
	/// </summary>
	[System.Serializable]
	public class Wave
	{
		// Delay before wave run
		public float delayBeforeWave;
		// List of enemies in this wave
		public List<GameObject> enemies = new List<GameObject>();
	}

	// Enemies will have different speed in specified interval
	public float speedRandomizer = 0.2f;
	// Delay between enemies spawn in wave
	public float unitSpawnDelay = 1.5f;
	// Waves list for this spawner
	public List<Wave> waves;
	// Endless enemies wave mode for this spawn poin
	public bool endlessWave = false;
	// This list is used for random enemy spawn
	[HideInInspector]
	public List<GameObject> randomEnemiesList = new List<GameObject>();

	// Enemies will move along this pathway
	private Pathway path;
	// Delay counter
	private float counter;
	// Buffer with active spawned enemies
	private List<GameObject> activeEnemies = new List<GameObject>();
	// All enemies were spawned
	private bool finished = false;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		path = GetComponentInParent<Pathway>();
		Debug.Assert(path != null, "Wrong initial parameters");
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EventManager.StartListening("UnitDie", UnitDie);
		EventManager.StartListening("WaveStart", WaveStart);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("UnitDie", UnitDie);
		EventManager.StopListening("WaveStart", WaveStart);
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// If all spawned enemies are dead
		if ((finished == true) && (activeEnemies.Count <= 0))
		{
			EventManager.TriggerEvent("AllEnemiesAreDead", null, null);
			gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Runs the wave.
	/// </summary>
	/// <returns>The wave.</returns>
	private IEnumerator RunWave(int waveIdx)
	{
		if (waves.Count > waveIdx)
		{
			yield return new WaitForSeconds(waves[waveIdx].delayBeforeWave);

			while (endlessWave == true)
			{
				GameObject prefab = randomEnemiesList[Random.Range (0, randomEnemiesList.Count)];
				// Create enemy
				GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
				newEnemy.name = prefab.name;
				// Set pathway
				newEnemy.GetComponent<AiStatePatrol>().path = path;
				NavAgent agent = newEnemy.GetComponent<NavAgent>();
				// Set speed offset
				agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
				// Add enemy to list
				activeEnemies.Add(newEnemy);
				// Wait for delay before next enemy run
				yield return new WaitForSeconds(unitSpawnDelay);
			}

			foreach (GameObject enemy in waves[waveIdx].enemies)
			{
				GameObject prefab = null;
				prefab = enemy;
				// If enemy prefab not specified - spawn random enemy
				if (prefab == null && randomEnemiesList.Count > 0)
				{
					prefab = randomEnemiesList[Random.Range (0, randomEnemiesList.Count)];
				}
				if (prefab == null)
				{
					Debug.LogError("Have no enemy prefab. Please specify enemies in Level Manager or in Spawn Point");
				}
				// Create enemy
				GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
				newEnemy.name = prefab.name;
				// Set pathway
				newEnemy.GetComponent<AiStatePatrol>().path = path;
				NavAgent agent = newEnemy.GetComponent<NavAgent>();
				// Set speed offset
				agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
				// Add enemy to list
				activeEnemies.Add(newEnemy);
				// Wait for delay before next enemy run
				yield return new WaitForSeconds(unitSpawnDelay);
			}
			if (waveIdx + 1 == waves.Count)
			{
				finished = true;
			}
		}
	}

	/// <summary>
	/// On unit die.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UnitDie(GameObject obj, string param)
	{
		// If this is active enemy
		if (activeEnemies.Contains(obj) == true)
		{
			// Remove it from buffer
			activeEnemies.Remove(obj);
		}
	}

	// Wave start event received
	private void WaveStart(GameObject obj, string param)
	{
		int waveIdx;
		int.TryParse(param, out waveIdx);
		StartCoroutine("RunWave", waveIdx);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
