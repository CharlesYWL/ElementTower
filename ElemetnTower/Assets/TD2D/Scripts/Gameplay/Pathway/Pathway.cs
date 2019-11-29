using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pathway for enemy moving.
/// </summary>
[ExecuteInEditMode]
public class Pathway : MonoBehaviour
{
	#if UNITY_EDITOR
    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        if (waypoints.Length > 1)
        {
            int idx;
            for (idx = 1; idx < waypoints.Length; idx++)
            {
                // Draw blue lines along pathway in edit mode
				Debug.DrawLine(waypoints[idx - 1].transform.position, waypoints[idx].transform.position, new Color(0.7f, 0f, 0f));
            }
        }
    }
	#endif

    /// <summary>
    /// Gets the nearest waypoint for specified position.
    /// </summary>
    /// <returns>The nearest waypoint.</returns>
    /// <param name="position">Position.</param>
    public Waypoint GetNearestWaypoint(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Waypoint nearestWaypoint = null;
        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            // Calculate distance to waypoint
            Vector3 vect = position - waypoint.transform.position;
            float distance = vect.magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWaypoint = waypoint;
            }
        }
		for (;;)
		{
			float waypointPathDistance = GetPathDistance(nearestWaypoint);
			Waypoint nextWaypoint = GetNextWaypoint(nearestWaypoint, false);
			if (nextWaypoint != null)
			{
				float myPathDistance = GetPathDistance(nextWaypoint) + (nextWaypoint.transform.position - position).magnitude;
				if (waypointPathDistance <= myPathDistance)
				{
					break;
				}
				else
				{
					nearestWaypoint = nextWaypoint;
				}
			}
			else
			{
				break;
			}
		}
        return nearestWaypoint;
    }

    /// <summary>
    /// Gets the next waypoint on this pathway.
    /// </summary>
    /// <returns>The next waypoint.</returns>
    /// <param name="currentWaypoint">Current waypoint.</param>
    /// <param name="loop">If set to <c>true</c> loop.</param>
    public Waypoint GetNextWaypoint(Waypoint currentWaypoint, bool loop)
    {
        Waypoint res = null;
        int idx = currentWaypoint.transform.GetSiblingIndex();
        if (idx < (transform.childCount - 1))
        {
            idx += 1;
        }
        else
        {
            idx = 0;
        }
        if (!(loop == false && idx == 0))
        {
            res = transform.GetChild(idx).GetComponent<Waypoint>(); 
        }
        return res;
    }

	/// <summary>
	/// Gets the previous waypoint on this pathway.
	/// </summary>
	/// <returns>The previous waypoint.</returns>
	/// <param name="currentWaypoint">Current waypoint.</param>
	/// <param name="loop">If set to <c>true</c> loop.</param>
	public Waypoint GetPreviousWaypoint(Waypoint currentWaypoint, bool loop)
	{
		Waypoint res = null;
		int idx = currentWaypoint.transform.GetSiblingIndex();
		if (idx > 0)
		{
			idx -= 1;
		}
		else
		{
			idx = transform.childCount - 1;
		}
		if (!(loop == false && idx == transform.childCount - 1))
		{
			res = transform.GetChild(idx).GetComponent<Waypoint>(); 
		}
		return res;
	}

	/// <summary>
	/// Gets the remaining path distance from specified waypoint.
	/// </summary>
	/// <returns>The path distance.</returns>
	/// <param name="fromWaypoint">From waypoint.</param>
    public float GetPathDistance(Waypoint fromWaypoint)
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        bool hitted = false;
        float pathDistance = 0f;
        int idx;
		// Calculate remaining path
        for (idx = 0; idx < waypoints.Length; ++idx)
        {
            if (hitted == true)
            {
				// Add distance between waypoint to result
                Vector2 distance = waypoints[idx].transform.position - waypoints[idx - 1].transform.position;
                pathDistance += distance.magnitude;
            }
            if (waypoints[idx] == fromWaypoint)
            {
                hitted = true;
            }
        }
        return pathDistance;
    }

	/// <summary>
	/// Gets the offset position on pathway.
	/// </summary>
	/// <returns>The offset position.</returns>
	/// <param name="nextWaypoint">Next waypoint. Will be updated after offset</param>
	/// <param name="currentPosition">Current position.</param>
	/// <param name="offsetDistance">Offset distance.</param>
	public Vector2 GetOffsetPosition(ref Waypoint nextWaypoint, Vector2 currentPosition, float offsetDistance)
	{
		Vector2 res = currentPosition;
		if (offsetDistance >= 0f) // Forward offset
		{
			float remainingDistance = offsetDistance;
			Vector2 lastPosition = currentPosition;
			Waypoint waypoint = nextWaypoint;
			Vector2 deltaVector = Vector2.zero;
			// Calculate waypoint nearest to offset position
			for (;;)
			{
				deltaVector = (Vector2)waypoint.transform.position - lastPosition;
				float deltaDistance = deltaVector.magnitude;
				if (remainingDistance > deltaDistance)
				{
					remainingDistance -= deltaDistance;
					lastPosition = waypoint.transform.position;
					waypoint = GetNextWaypoint(waypoint, false);
					if (waypoint == null)
					{
						remainingDistance = 0f;
						break;
					}
					else
					{
						nextWaypoint = waypoint;
					}
				}
				else
				{
					break;
				}
			}
			// Calculate result position
			res = lastPosition + deltaVector.normalized * remainingDistance;
		}
		else // Back offset
		{
			float remainingDistance = -offsetDistance;
			Vector2 lastPosition = currentPosition;
			Waypoint waypoint = GetPreviousWaypoint(nextWaypoint, false);
			if (waypoint == null)
			{
				return res;
			}
			Vector2 deltaVector = Vector2.zero;
			// Calculate waypoint nearest to offset position
			for (;;)
			{
				deltaVector = (Vector2)waypoint.transform.position - lastPosition;
				float deltaDistance = deltaVector.magnitude;
				if (remainingDistance > deltaDistance)
				{
					nextWaypoint = waypoint;
					remainingDistance -= deltaDistance;
					lastPosition = waypoint.transform.position;
					waypoint = GetPreviousWaypoint(waypoint, false);
					if (waypoint == null)
					{
						remainingDistance = 0f;
						break;
					}
				}
				else
				{
					break;
				}
			}
			// Calculate result position
			res = lastPosition + deltaVector.normalized * remainingDistance;
		}
		return res;
	}
}
