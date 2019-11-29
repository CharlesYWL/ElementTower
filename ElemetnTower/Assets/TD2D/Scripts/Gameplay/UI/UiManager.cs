using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// User interface and events manager.
/// </summary>
public class UiManager : MonoBehaviour
{
    // This scene will loaded after whis level exit
    public string exitSceneName;
	// Start screen canvas
	public GameObject startScreen;
    // Pause menu canvas
    public GameObject pauseMenu;
    // Defeat menu canvas
    public GameObject defeatMenu;
    // Victory menu canvas
    public GameObject victoryMenu;
    // Level interface
    public GameObject levelUI;
    // Avaliable gold amount
    public Text goldAmount;
	// Capture attempts before defeat
	public Text defeatAttempts;
	// Victory and defeat menu display delay
	public float menuDisplayDelay = 1f;

    // Is game paused?
    private bool paused;
    // Camera is dragging now
    private bool cameraIsDragged;
    // Origin point of camera dragging start
    private Vector3 dragOrigin = Vector3.zero;
    // Camera control component
    private CameraControl cameraControl;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		cameraControl = FindObjectOfType<CameraControl>();
		Debug.Assert(cameraControl && startScreen && pauseMenu && defeatMenu && victoryMenu && levelUI && defeatAttempts && goldAmount, "Wrong initial parameters");
	}

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
		EventManager.StartListening("UnitKilled", UnitKilled);
		EventManager.StartListening("ButtonPressed", ButtonPressed);
		EventManager.StartListening("Defeat", Defeat);
		EventManager.StartListening("Victory", Victory);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
		EventManager.StopListening("UnitKilled", UnitKilled);
		EventManager.StopListening("ButtonPressed", ButtonPressed);
		EventManager.StopListening("Defeat", Defeat);
		EventManager.StopListening("Victory", Victory);
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
		PauseGame(true);
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        if (paused == false)
        {
            // User press mouse button
            if (Input.GetMouseButtonDown(0) == true)
            {
                // Check if pointer over UI components
                GameObject hittedObj = null;
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);
				if (results.Count > 0) // UI components on pointer
				{
					// Search for Action Icon hit in results
					foreach (RaycastResult res in results)
					{
						if (res.gameObject.CompareTag("ActionIcon"))
						{
							hittedObj = res.gameObject;
							break;
						}
					}
					// Send message with user click data on UI component
					EventManager.TriggerEvent("UserUiClick", hittedObj, null);
				}
				else // No UI components on pointer
                {
                    // Check if pointer over colliders
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
                    foreach (RaycastHit2D hit in hits)
                    {
                        // If this object has unit info
                        if (hit.collider.CompareTag("UnitInfo"))
                        {
							Tower tower = hit.collider.GetComponentInParent<Tower>();
							if (tower != null)
							{
								hittedObj = tower.gameObject;
								break;
							}
							AiBehavior aiBehavior = hit.collider.GetComponentInParent<AiBehavior>();
							if (aiBehavior != null)
							{
								hittedObj = aiBehavior.gameObject;
								break;
							}
							hittedObj = hit.collider.gameObject;
                            break;
                        }
                    }
					// Send message with user click data on game space
					EventManager.TriggerEvent("UserClick", hittedObj, null);
                }
				// If there is no hitted object - start camera drag
                if (hittedObj == null)
                {
                    cameraIsDragged = true;
                    dragOrigin = Input.mousePosition;
                }
            }
            if (Input.GetMouseButtonUp(0) == true)
            {
				// Stop drag camera on mouse release
                cameraIsDragged = false;
            }
            if (cameraIsDragged == true)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
				// Camera dragging (inverted)
                cameraControl.MoveX(-pos.x);
                cameraControl.MoveY(-pos.y);
            }
        }
    }

    /// <summary>
    /// Stop current scene and load new scene
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    private void LoadScene(string sceneName)
    {
		EventManager.TriggerEvent("SceneQuit", null, null);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
	private void ResumeGame()
    {
        GoToLevel();
        PauseGame(false);
    }

    /// <summary>
    /// Gos to main menu.
    /// </summary>
	private void ExitFromLevel()
    {
        LoadScene(exitSceneName);
    }

    /// <summary>
    /// Closes all UI canvases.
    /// </summary>
    private void CloseAllUI()
    {
		startScreen.SetActive (false);
        pauseMenu.SetActive(false);
        defeatMenu.SetActive(false);
        victoryMenu.SetActive(false);
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    /// <param name="pause">If set to <c>true</c> pause.</param>
    private void PauseGame(bool pause)
    {
        paused = pause;
        // Stop the time on pause
        Time.timeScale = pause ? 0f : 1f;
		EventManager.TriggerEvent("GamePaused", null, pause.ToString());
    }

    /// <summary>
    /// Gos to pause menu.
    /// </summary>
	private void GoToPauseMenu()
    {
        PauseGame(true);
        CloseAllUI();
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// Gos to level.
    /// </summary>
    private void GoToLevel()
    {
        CloseAllUI();
        levelUI.SetActive(true);
        PauseGame(false);
    }

    /// <summary>
    /// Gos to defeat menu.
    /// </summary>
	private void Defeat(GameObject obj, string param)
    {
		StartCoroutine("DefeatCoroutine");
    }

	/// <summary>
	/// Display defeat menu after delay.
	/// </summary>
	/// <returns>The coroutine.</returns>
	private IEnumerator DefeatCoroutine()
	{
		yield return new WaitForSeconds(menuDisplayDelay);
		PauseGame(true);
		CloseAllUI();
		defeatMenu.SetActive(true);
	}

    /// <summary>
    /// Gos to victory menu.
    /// </summary>
	private void Victory(GameObject obj, string param)
    {
		StartCoroutine("VictoryCoroutine");
    }

	/// <summary>
	/// Display victory menu after delay.
	/// </summary>
	/// <returns>The coroutine.</returns>
	private IEnumerator VictoryCoroutine()
	{
		yield return new WaitForSeconds(menuDisplayDelay);
		PauseGame(true);
		CloseAllUI();

		// --- Game progress autosaving ---
		// Get the name of completed level
		DataManager.instance.progress.lastCompetedLevel = SceneManager.GetActiveScene().name;
		// Check if this level have no completed before
		bool hit = false;
		foreach (string level in DataManager.instance.progress.openedLevels)
		{
			if (level == SceneManager.GetActiveScene().name)
			{
				hit = true;
				break;
			}
		}
		if (hit == false)
		{
			DataManager.instance.progress.openedLevels.Add(SceneManager.GetActiveScene().name);
		}
		// Save game progress
		DataManager.instance.SaveGameProgress();

		victoryMenu.SetActive(true);
	}

    /// <summary>
    /// Restarts current level.
    /// </summary>
	private void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Gets current gold amount.
    /// </summary>
    /// <returns>The gold.</returns>
	public int GetGold()
    {
        int gold;
        int.TryParse(goldAmount.text, out gold);
        return gold;
    }

    /// <summary>
    /// Sets gold amount.
    /// </summary>
    /// <param name="gold">Gold.</param>
	public void SetGold(int gold)
    {
        goldAmount.text = gold.ToString();
    }

    /// <summary>
    /// Adds the gold.
    /// </summary>
    /// <param name="gold">Gold.</param>
	public void AddGold(int gold)
    {
        SetGold(GetGold() + gold);
    }

    /// <summary>
    /// Spends the gold if it is.
    /// </summary>
    /// <returns><c>true</c>, if gold was spent, <c>false</c> otherwise.</returns>
    /// <param name="cost">Cost.</param>
    public bool SpendGold(int cost)
    {
        bool res = false;
        int currentGold = GetGold();
        if (currentGold >= cost)
        {
            SetGold(currentGold - cost);
            res = true;
        }
        return res;
    }

	/// <summary>
	/// Sets the defeat attempts.
	/// </summary>
	/// <param name="attempts">Attempts.</param>
	public void SetDefeatAttempts(int attempts)
	{
		defeatAttempts.text = attempts.ToString();
	}

    /// <summary>
    /// On unit killed by other unit.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
	private void UnitKilled(GameObject obj, string param)
    {
        // If this is enemy
		if (obj.CompareTag("Enemy") || obj.CompareTag("FlyingEnemy"))
        {
            Price price = obj.GetComponent<Price>();
            if (price != null)
            {
                // Add gold for enemy kill
                AddGold(price.price);
            }
        }
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
		case "Pause":
			GoToPauseMenu();
			break;
		case "Resume":
			GoToLevel();
			break;
		case "Back":
			ExitFromLevel();
			break;
		case "Restart":
			RestartLevel();
			break;
		}
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
