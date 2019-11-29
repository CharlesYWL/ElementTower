using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tower action for defenders moving.
/// </summary>
public class TowerActionMove : TowerAction
{
	// Defend point of this tower
	private DefendPoint defendPoint;
	// Tower
	private Tower tower;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
		defendPoint = buildingPlace.GetComponentInChildren<DefendPoint>();
		tower = GetComponentInParent<Tower>();
		Debug.Assert(defendPoint && tower, "Wrong initial settings");
	}

	/// <summary>
	/// Clicked this instance.
	/// </summary>
	protected override void Clicked()
	{
		defendPoint.SetVisible(true);
		tower.ShowRange(true);
	}
}
