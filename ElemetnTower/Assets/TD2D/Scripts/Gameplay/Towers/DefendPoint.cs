using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position for defenders.
/// </summary>
public class DefendPoint : MonoBehaviour
{
	// Prefab for defend point
	public GameObject defendPointPrefab;
	// Prefab for flag
	public GameObject defendFlagPrefab;
	// Pathway direction near this tower (for defenders orientation)
	public bool clockwiseDirection = false;
	[HideInInspector]
	// Active defenders list
	public Dictionary<GameObject, Transform> activeDefenders = new Dictionary<GameObject, Transform>();

	// Active flag
	private GameObject activeDefendFlag;
	// List with defend places for this defend point
	private List<Transform> defendPlaces = new List<Transform>();
	// Machine state
	private enum MyState
	{
		Inactive,
		Active
	}
	// My current state
	private MyState myState = MyState.Inactive;
	// Building place
	private BuildingPlace buildingPlace;
	// Tower
	private Tower tower;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EventManager.StartListening("UserClick", UserClick);
		EventManager.StartListening("UserUiClick", UserUiClick);
		EventManager.StartListening("UnitDie", UnitDie);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("UserClick", UserClick);
		EventManager.StopListening("UserUiClick", UserUiClick);
		EventManager.StopListening("UnitDie", UnitDie);
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Debug.Assert(defendPointPrefab && defendFlagPrefab, "Wrong initial settings");
		// Get defend places from defend point prefab and place it on scene
		foreach (Transform defendPlace in defendPointPrefab.transform)
		{
			Instantiate(defendPlace.gameObject, transform);
		}
		// Create defend places list
		foreach (Transform child in transform)
		{
			defendPlaces.Add(child);
		}
		BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
		LookAtDirection2D((Vector2)(buildingPlace.transform.position - transform.position));
	}

    /// <summary>
    /// Gets the defend points list.
    /// </summary>
    /// <returns>The defend points.</returns>
    public List<Transform> GetDefendPoints()
    {
		return defendPlaces;
    }

	/// <summary>
	/// Sets visible defend flag.
	/// </summary>
	/// <param name="enabled">If set to <c>true</c> enabled.</param>
	public void SetVisible(bool enabled)
	{
		if (enabled == true)
		{
			if (myState == MyState.Inactive)
			{
				buildingPlace = GetComponentInParent<BuildingPlace>();
				tower = buildingPlace.GetComponentInChildren<Tower>();
				// Show defend flag
				activeDefendFlag = Instantiate(defendFlagPrefab);
				activeDefendFlag.transform.position = transform.position;
				myState = MyState.Active;
			}
		}
		else
		{
			if (myState == MyState.Active)
			{
				myState = MyState.Inactive;
				// Hide defense range
				tower.ShowRange(false);
				// Hide flag
				Destroy(activeDefendFlag);
			}
		}
	}

	/// <summary>
	/// On user UI click.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UserUiClick(GameObject obj, string param)
	{
		if (myState == MyState.Active)
		{
			SetVisible(false);
		}
	}

	/// <summary>
	/// User click handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UserClick(GameObject obj, string param)
	{
		if (myState == MyState.Active)
		{
			myState = MyState.Inactive;
			Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 delta = position - (Vector2)tower.transform.position;
			delta = Vector2.ClampMagnitude(delta, tower.range.transform.localScale.x);
			transform.position = tower.transform.position + (Vector3)delta;
			LookAtDirection2D((Vector2)(tower.transform.position - transform.position));
			SetVisible(false);
			// Move defenders to new flag position
			foreach (KeyValuePair<GameObject, Transform> pair in activeDefenders)
			{
				AiBehavior aiBehavior = pair.Key.GetComponent<AiBehavior>();
				aiBehavior.ChangeState(aiBehavior.GetComponent<AiStateMove>());
			}
			Destroy(activeDefendFlag);
		}
	}

	/// <summary>
	/// Raises on every unit die.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UnitDie(GameObject obj, string param)
	{
		// If this is object from my list
		if (activeDefenders.ContainsKey(obj) == true)
		{
			// Remove it from list
			activeDefenders.Remove(obj);
		}
	}

	/// <summary>
	/// Gets the defender list.
	/// </summary>
	/// <returns>The defender list.</returns>
	public List<GameObject> GetDefenderList()
	{
		List<GameObject> res = new List<GameObject>();
		foreach (KeyValuePair<GameObject, Transform> pair in activeDefenders)
		{
			res.Add(pair.Key);
		}
		return res;
	}

	/// <summary>
	/// Gets the free defend position if it is.
	/// </summary>
	/// <returns>The free defend position.</returns>
	/// <param name="index">Index.</param>
	public Transform GetFreeDefendPosition()
	{
		Transform res = null;
		List<Transform> points = GetDefendPoints();
		foreach (Transform point in points)
		{
			// If this point not busy already
			if (activeDefenders.ContainsValue(point) == false)
			{
				res = point;
				break;
			}
		}
		return res;
	}

	/// <summary>
	/// Looks at direction2d.
	/// </summary>
	/// <param name="direction">Direction.</param>
	private void LookAtDirection2D(Vector2 direction)
	{
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		float offset = clockwiseDirection == false ? 90f : -90f;
		transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
	}
}
