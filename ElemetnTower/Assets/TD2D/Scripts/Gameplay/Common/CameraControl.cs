using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera moving and autoscaling.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
	/// <summary>
	/// Control type for camera autoscaling.
	/// </summary>
    public enum ControlType
    {
        ConstantWidth,		// Camera will keep constant width
		ConstantHeight,		// Camera will keep constant height
		OriginCameraSize	// Do not scale camera
    }

	// Camera control type
    public ControlType controlType;
	// Camera will autoscale to fit this object
	public SpriteRenderer focusObjectRenderer;
	// Horizontal offset from focus object edges
	public float offsetX = 0f;
	// Vertical offset from focus object edges
	public float offsetY = 0f;
	// Camera speed when moving (dragging)
    public float dragSpeed = 2f;

	// Restrictive points for camera moving
	private float maxX, minX, maxY, minY;
	// Camera dragging at now vector
    private float moveX, moveY;
	// Camera component from this gameobject
	private Camera cam;
	// Origin camera aspect ratio
	private float originAspect;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		cam = GetComponent<Camera>();
		Debug.Assert(focusObjectRenderer && cam, "Wrong initial settings");
		originAspect = cam.aspect;
		// Get restrictive points from focus object's corners
		maxX = focusObjectRenderer.bounds.max.x;
		minX = focusObjectRenderer.bounds.min.x;
		maxY = focusObjectRenderer.bounds.max.y;
		minY = focusObjectRenderer.bounds.min.y;
		UpdateCameraSize();
	}

	/// <summary>
	/// Lates update this instance.
	/// </summary>
    void LateUpdate()
    {
		// Camera aspect ratio is changed
		if (originAspect != cam.aspect)
		{
			UpdateCameraSize();
			originAspect = cam.aspect;
		}
		// Need to move camera horizontally
        if (moveX != 0f)
        {
			bool permit = false;
			// Move to right
			if (moveX > 0f)
			{
				// If restrictive point does not reached
				if (cam.transform.position.x + (cam.orthographicSize * cam.aspect) < maxX - offsetX)
				{
					permit = true;
				}
			}
			// Move to left
			else
			{
				// If restrictive point does not reached
				if (cam.transform.position.x - (cam.orthographicSize * cam.aspect) > minX + offsetX)
				{
					permit = true;
				}
			}
			if (permit == true)
			{
				// Move camera
				transform.Translate(Vector3.right * moveX * dragSpeed, Space.World);
			}
            moveX = 0f;
        }
		// Need to move camera vertically
        if (moveY != 0f)
        {
			bool permit = false;
			// Move up
			if (moveY > 0f)
			{
				// If restrictive point does not reached
				if (cam.transform.position.y + cam.orthographicSize < maxY - offsetY)
				{
					permit = true;
				}
			}
			// Move down
			else
			{
				// If restrictive point does not reached
				if (cam.transform.position.y - cam.orthographicSize > minY + offsetY)
				{
					permit = true;
				}
			}
			if (permit == true)
			{
				// Move camera
				transform.Translate (Vector3.up * moveY * dragSpeed, Space.World);
			}
            moveY = 0f;
        }
    }

	/// <summary>
	/// Need to move camera horizontally.
	/// </summary>
	/// <param name="distance">Distance.</param>
    public void MoveX(float distance)
    {
        moveX = distance;
    }

	/// <summary>
	/// Need to move camera vertically.
	/// </summary>
	/// <param name="distance">Distance.</param>
    public void MoveY(float distance)
    {
        moveY = distance;
    }

	/// <summary>
	/// Updates the size of the camera to fit focus object.
	/// </summary>
	private void UpdateCameraSize()
	{
		switch (controlType)
		{
		case ControlType.ConstantWidth:
			cam.orthographicSize = (maxX - minX - 2 * offsetX) / (2f * cam.aspect);
			break;
		case ControlType.ConstantHeight:
			cam.orthographicSize = (maxY - minY - 2 * offsetY) / 2f;
			break;
		}
	}
}
