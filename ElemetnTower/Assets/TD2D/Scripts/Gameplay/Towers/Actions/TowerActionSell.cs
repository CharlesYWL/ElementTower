using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sell the tower.
/// </summary>
public class TowerActionSell : TowerAction
{
	// Prefab for empty building place
	public GameObject emptyPlacePrefab;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Debug.Assert(emptyPlacePrefab, "Wrong initial parameters");
	}

	protected override void Clicked()
	{
		// Sell the tower
		Tower tower = GetComponentInParent<Tower>();
		if (tower != null)
		{
			tower.SellTower(emptyPlacePrefab);
		}
	}
}
