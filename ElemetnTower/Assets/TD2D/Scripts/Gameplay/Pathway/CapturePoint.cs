using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If enemy rise this point - player will be defeated.
/// </summary>
public class CapturePoint : MonoBehaviour
{
    /// <summary>
    /// Raises the trigger enter2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
		Destroy(other.gameObject);
		EventManager.TriggerEvent("Captured", other.gameObject, null);
    }
}
