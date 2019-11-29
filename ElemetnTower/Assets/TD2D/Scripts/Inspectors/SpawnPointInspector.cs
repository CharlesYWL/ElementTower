using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
/// <summary>
/// Spawn point inspector.
/// </summary>
public class SpawnPointInspector : MonoBehaviour
{
	[HideInInspector]
	// List with enemies number for each wave
	public List<int> enemies = new List<int>();

	// Spawn point component
	private SpawnPoint spawnPoint;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		spawnPoint = GetComponent<SpawnPoint>();
		Debug.Assert(spawnPoint, "Wrong stuff settings");
		// Initiate waves list
		enemies.Clear();
		foreach (SpawnPoint.Wave wave in spawnPoint.waves)
		{
			enemies.Add(wave.enemies.Count);
		}
	}

	/// <summary>
	/// Updates the wave list.
	/// </summary>
	public void UpdateWaveList()
	{
		// Update waves
		while (spawnPoint.waves.Count > enemies.Count)
		{
			spawnPoint.waves.RemoveAt(spawnPoint.waves.Count - 1);
		}
		while (spawnPoint.waves.Count < enemies.Count)
		{
			spawnPoint.waves.Add(new SpawnPoint.Wave());
		}
		// Update enemies count
		for (int i = 0; i < enemies.Count; i++)
		{
			while (spawnPoint.waves[i].enemies.Count > enemies[i])
			{
				spawnPoint.waves[i].enemies.RemoveAt(spawnPoint.waves[i].enemies.Count - 1);
			}
			while (spawnPoint.waves[i].enemies.Count < enemies[i])
			{
				spawnPoint.waves[i].enemies.Add(null);
			}
		}
	}

	/// <summary>
	/// Adds the wave.
	/// </summary>
	public void AddWave()
	{
		enemies.Add(1);
	}

	/// <summary>
	/// Removes the wave.
	/// </summary>
	public void RemoveWave()
	{
		if (enemies.Count > 0)
		{
			enemies.RemoveAt(enemies.Count - 1);
		}
	}
}
#endif
