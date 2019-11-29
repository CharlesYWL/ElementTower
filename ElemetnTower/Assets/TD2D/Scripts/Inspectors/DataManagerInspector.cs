using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
/// <summary>
/// Data manager inspector.
/// </summary>
public class DataManagerInspector : MonoBehaviour
{
	// Data manager component
	private DataManager dataManager;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		dataManager = GetComponent<DataManager>();
		Debug.Assert(dataManager, "Wrong initial settings");
	}

	/// <summary>
	/// Resets the game progress.
	/// </summary>
	public void ResetGameProgress()
	{
		dataManager.DeleteGameProgress();
	}

	/// <summary>
	/// Permits the level by its name.
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void PermitLevel(string levelName)
	{
		if (dataManager.progress.openedLevels.Contains(levelName) == false)
		{
			dataManager.progress.openedLevels.Add(levelName);
			dataManager.SaveGameProgress();
		}
	}
}
#endif
