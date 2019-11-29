using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu operate.
/// </summary>
public class MainMenu : MonoBehaviour
{
	// Name of scene to start on click
    public string startSceneName;
	// Credits menu
	public GameObject creditsMenu;

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

	void Awake()
	{
		Debug.Assert(creditsMenu, "Wrong initial settings");
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
		case "Quit":
			Application.Quit();
			break;
		case "Start":
			SceneManager.LoadScene(startSceneName);
			break;
		case "OpenCredits":
			creditsMenu.SetActive(true);
			break;
		case "CloseCredits":
			creditsMenu.SetActive(false);
			break;
		}
	}
}
