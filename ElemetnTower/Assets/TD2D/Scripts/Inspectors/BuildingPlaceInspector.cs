using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
/// <summary>
/// Building place inspector.
/// </summary>
public class BuildingPlaceInspector : MonoBehaviour
{
	// Tower of this building place
	private GameObject myTower;
	// Defend point of this building place
	private GameObject defendPoint;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		defendPoint = GetComponentInChildren<DefendPoint>().gameObject;
		myTower = GetComponentInChildren<Tower>().gameObject;
		Debug.Assert(myTower && defendPoint, "Wrong stuff settings");
	}

	/// <summary>
	/// Gets the defend point.
	/// </summary>
	/// <returns>The defend point.</returns>
	public GameObject GetDefendPoint()
	{
		return defendPoint;
	}

	/// <summary>
	/// Chooses the tower.
	/// </summary>
	/// <returns>The tower.</returns>
	/// <param name="towerPrefab">Tower prefab.</param>
	public GameObject ChooseTower(GameObject towerPrefab)
	{
		// Destroy old tower
		if (myTower != null)
		{
			DestroyImmediate(myTower);
		}
		// Create new tower
		myTower = Instantiate(towerPrefab, transform);
		myTower.name = towerPrefab.name;
		myTower.transform.SetAsLastSibling();
		return myTower;
	}
}
#endif
