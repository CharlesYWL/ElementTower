using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
/// <summary>
/// Pathway inspector.
/// </summary>
public class PathwayInspector : MonoBehaviour
{
	// Spawn point of this pathway
	public SpawnPoint spawnPoint;
	// Prefab for waypoint
	public Waypoint waypointPrefab;
	// Waypoint folder for this pathway
	public Transform waypointsFolder;
	// Waypoint offset when placed on scene
	public Vector2 offset = new Vector2(1f, 0f);

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		Debug.Assert(spawnPoint && waypointPrefab && waypointsFolder, "Wrong stuff settings");
	}

	/// <summary>
	/// Gets the spawn point.
	/// </summary>
	/// <returns>The spawn point.</returns>
	public GameObject GetSpawnPoint()
	{
		return spawnPoint.gameObject;
	}

	/// <summary>
	/// Adds the waypoint.
	/// </summary>
	/// <returns>The waypoint.</returns>
	public GameObject AddWaypoint()
	{
		Waypoint[] array = GetComponentsInChildren<Waypoint>();
		GameObject res = Instantiate(waypointPrefab, waypointsFolder).gameObject;
		res.transform.SetAsLastSibling();
		res.name = waypointPrefab.name + " (" + (array.Length + 1) + ")";
		if (array.Length > 0)
		{
			res.transform.position = array[array.Length - 1].transform.position + (Vector3)offset;
		}
		else
		{
			res.transform.position += (Vector3)offset;
		}
		return res;
	}

	/// <summary>
	/// Gets the next waypoint.
	/// </summary>
	/// <returns>The next waypoint.</returns>
	/// <param name="currentSelected">Current selected.</param>
	public GameObject GetNextWaypoint(GameObject currentSelected)
	{
		return InspectorsUtil<Waypoint>.GetNext(waypointsFolder, currentSelected);
	}

	/// <summary>
	/// Gets the previoust waypoint.
	/// </summary>
	/// <returns>The previoust waypoint.</returns>
	/// <param name="currentSelected">Current selected.</param>
	public GameObject GetPrevioustWaypoint(GameObject currentSelected)
	{
		return InspectorsUtil<Waypoint>.GetPrevious(waypointsFolder, currentSelected);
	}
}
#endif
