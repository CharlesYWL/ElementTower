using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
/// <summary>
/// Map folder inspector.
/// </summary>
public class MapFolderInspector : MonoBehaviour
{
	// Map image
	public SpriteRenderer map;
	// Folder for spawn icons image
	public Transform spawnIconFolder;
	// Folder for capture icons image
	public Transform captureIconFolder;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		Debug.Assert(map && spawnIconFolder && captureIconFolder, "Wrong stuff settings");
	}

	/// <summary>
	/// Changes the map sprite.
	/// </summary>
	/// <returns>The map sprite.</returns>
	/// <param name="mapPrefab">Map prefab.</param>
	public void ChangeMapSprite(Sprite sprite)
	{
		if (map != null && sprite != null)
		{
			map.sprite = sprite;
		}
	}

	public void LoadMap(GameObject mapPrefab)
	{
		if (mapPrefab != null)
		{
			if (map != null)
			{
				DestroyImmediate(map.gameObject);
			}
			GameObject newMap = Instantiate(mapPrefab, transform);
			newMap.name = mapPrefab.name;
			map = newMap.GetComponent<SpriteRenderer>();
			Debug.Assert(map, "Wrong stuff settings");
		}
	}

	/// <summary>
	/// Adds the spawn icon.
	/// </summary>
	/// <returns>The spawn icon.</returns>
	/// <param name="spawnIconPrefab">Spawn icon prefab.</param>
	public GameObject AddSpawnIcon(GameObject spawnIconPrefab)
	{
		GameObject res = Instantiate(spawnIconPrefab, spawnIconFolder);
		res.name = spawnIconPrefab.name;
		return res;
	}

	/// <summary>
	/// Adds the capture icon.
	/// </summary>
	/// <returns>The capture icon.</returns>
	/// <param name="captureIconPrefab">Capture icon prefab.</param>
	public GameObject AddCaptureIcon(GameObject captureIconPrefab)
	{
		GameObject res = Instantiate(captureIconPrefab, captureIconFolder);
		res.name = captureIconPrefab.name;
		return res;
	}
}
#endif
