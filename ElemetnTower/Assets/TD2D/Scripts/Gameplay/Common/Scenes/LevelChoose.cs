using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Level choose scene manager.
/// </summary>
public class LevelChoose : MonoBehaviour
{
	// Scene to exit
    public string exitSceneName;
	// Visual displaing for number of levels
	public Transform togglesFolder;
	// Active toggle prefab
	public Toggle activeTogglePrefab;
	// Inactive toggle prefab
	public Toggle inactiveTogglePrefab;
	// Next level button
	public Button nextLevelButton;
	// Previous level button
	public Button prevLevelButton;
	// Folder for level visualisation
	public Transform levelFolder;
	// Choosen level
	public GameObject currentLevel;
	// All levels
	public List<GameObject> levelsPrefabs = new List<GameObject>();

	// Index of last allowed level for choosing
	private int maxActiveLevelIdx;
	// Index of current displayed level
	private int currentDisplayedLevelIdx;
	// List with active toggles
	private List<Toggle> activeToggles = new List<Toggle>();

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EventManager.StartListening("ButtonPressed", ButtonPressed);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("ButtonPressed", ButtonPressed);
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		maxActiveLevelIdx = -1;
		Debug.Assert(currentLevel && togglesFolder && activeTogglePrefab && inactiveTogglePrefab && nextLevelButton && prevLevelButton && levelFolder, "Wrong initial settings");
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
    void Start()
    {
		int hitIdx = -1;
		int levelsCount = DataManager.instance.progress.openedLevels.Count;
		if (levelsCount > 0)
		{
			// Get name of last opened level from stored data
			string openedLevelName = DataManager.instance.progress.openedLevels[levelsCount - 1];

	        int idx;
			for (idx = 0; idx < levelsPrefabs.Count; ++idx)
	        {
				// Try to find last opened level in levels list
				if (levelsPrefabs[idx].name == openedLevelName)
	            {
	                hitIdx = idx;
	                break;
	            }
	        }
		}
		// Level found
		if (hitIdx >= 0)
		{
			if (levelsPrefabs.Count > hitIdx + 1)
			{
				maxActiveLevelIdx = hitIdx + 1;
			}
			else
			{
				maxActiveLevelIdx = hitIdx;
			}
		}
		// level does not found
		else
		{
			if (levelsPrefabs.Count > 0)
			{
				maxActiveLevelIdx = 0;
			}
			else
			{
				Debug.LogError("Have no levels prefabs!");
			}
		}
		if (maxActiveLevelIdx >= 0)
		{
			DisplayToggles();
			DisplayLevel(maxActiveLevelIdx);
		}
    }

	/// <summary>
	/// Visual displaing for number of levels
	/// </summary>
	private void DisplayToggles()
	{
		foreach (Toggle toggle in togglesFolder.GetComponentsInChildren<Toggle>())
		{
			Destroy(toggle.gameObject);
		}
		int cnt;
		for (cnt = 0; cnt < maxActiveLevelIdx + 1; cnt++)
		{
			GameObject toggle = Instantiate(activeTogglePrefab.gameObject, togglesFolder);
			activeToggles.Add(toggle.GetComponent<Toggle>());
		}
		if (maxActiveLevelIdx < levelsPrefabs.Count - 1)
		{
			Instantiate(inactiveTogglePrefab.gameObject, togglesFolder);
		}
	}

	/// <summary>
	/// Displaies choosen level.
	/// </summary>
	/// <param name="levelIdx">Level index.</param>
	private void DisplayLevel(int levelIdx)
	{
		Transform parentOfLevel = currentLevel.transform.parent;
		Vector3 levelPosition = currentLevel.transform.position;
		Quaternion levelRotation = currentLevel.transform.rotation;
		Destroy(currentLevel);
		currentLevel = Instantiate(levelsPrefabs[levelIdx], parentOfLevel);
		currentLevel.name = levelsPrefabs[levelIdx].name;
		currentLevel.transform.position = levelPosition;
		currentLevel.transform.rotation = levelRotation;
		currentDisplayedLevelIdx = levelIdx;
		foreach (Toggle toggle in activeToggles)
		{
			toggle.isOn = false;
		}
		activeToggles[levelIdx].isOn = true;
		UpdateButtonsVisible (levelIdx);
	}

	/// <summary>
	/// Updates the buttons visible.
	/// </summary>
	/// <param name="levelIdx">Level index.</param>
	private void UpdateButtonsVisible(int levelIdx)
	{
		prevLevelButton.interactable = levelIdx > 0 ? true : false;
		nextLevelButton.interactable = levelIdx < maxActiveLevelIdx ? true : false;
	}

	/// <summary>
	/// Displaies the next level.
	/// </summary>
	private void DisplayNextLevel()
	{
		if (currentDisplayedLevelIdx < maxActiveLevelIdx)
		{
			DisplayLevel(currentDisplayedLevelIdx + 1);
		}
	}

	/// <summary>
	/// Displaies the previous level.
	/// </summary>
	private void DisplayPrevLevel()
	{
		if (currentDisplayedLevelIdx > 0)
		{
			DisplayLevel (currentDisplayedLevelIdx - 1);
		}
	}

	/// <summary>
	/// Exit scene.
	/// </summary>
	private void Exit()
	{
		SceneManager.LoadScene(exitSceneName);
	}

	/// <summary>
	/// Go to choosen level.
	/// </summary>
	private void GoToLevel()
	{
		SceneManager.LoadScene(currentLevel.name);
	}

	/// <summary>
	/// Buttons pressed handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void ButtonPressed(GameObject obj, string param)
	{
		switch (param)
		{
		case "Start":
			GoToLevel();
			break;
		case "Exit":
			Exit();
			break;
		case "Next":
			DisplayNextLevel();
			break;
		case "Prev":
			DisplayPrevLevel();
			break;
		}
	}
}
