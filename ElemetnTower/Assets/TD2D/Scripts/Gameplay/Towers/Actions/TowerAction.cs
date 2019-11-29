using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic class for tower action.
/// </summary>
public class TowerAction : MonoBehaviour
{
	// Icon for enabled state
	public GameObject enabledIcon;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EventManager.StartListening("UserUiClick", UserUiClick);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("UserUiClick", UserUiClick);
	}

	/// <summary>
	/// On user UI click.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UserUiClick(GameObject obj, string param)
	{
		// If clicked on this icon
		if (obj == gameObject)
		{
			if (enabledIcon.activeSelf == true)
			{
				Clicked();
			}
		}
	}

	/// <summary>
	/// Clicked this instance.
	/// </summary>
	protected virtual void Clicked()
	{

	}
}
