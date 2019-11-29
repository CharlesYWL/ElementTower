using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy waves timings.
/// </summary>
[ExecuteInEditMode]
public class WavesInfo : MonoBehaviour
{
	// TO between waves
	public List<float> wavesTimeouts = new List<float>();

	// Execute only in edit mode
	#if UNITY_EDITOR

	// TO between waves by default
	private float defaultWaveTimeout = 30f;
	// List with active spawners in level
	private SpawnPoint[] spawners;

	/// <summary>
	/// On editor update.
	/// </summary>
	public void Update()
	{
		spawners = FindObjectsOfType<SpawnPoint>();
		int wavesCount = 0;
		// Get the max number of waves from spawners
		foreach (SpawnPoint spawner in spawners)
		{
			if (spawner.waves.Count > wavesCount)
			{
				wavesCount = spawner.waves.Count;
			}
		}
		// Display actual list with waves timeouts
		if (wavesTimeouts.Count < wavesCount)
		{
			int i;
			for (i = wavesTimeouts.Count; i < wavesCount; ++i)
			{
				wavesTimeouts.Add (defaultWaveTimeout);
			}
		}
		else if (wavesTimeouts.Count > wavesCount)
		{
			wavesTimeouts.RemoveRange (wavesCount, wavesTimeouts.Count - wavesCount);
		}
	}

	#endif
}
