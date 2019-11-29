using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Level control script.
/// </summary>
public class LevelManager : MonoBehaviour
{
	// UI scene. Load on level start
	public string levelUiSceneName;
	// Gold amount for this level
	public int goldAmount = 20;
	// How many times enemies can reach capture point before defeat
	public int defeatAttempts = 1;
	// List with allowed randomly generated enemy for this level
	public List<GameObject> allowedEnemies = new List<GameObject>();
	// List with allowed towers for this level
	public List<GameObject> allowedTowers = new List<GameObject>();
	// List with allowed spells for this level
	public List<GameObject> allowedSpells = new List<GameObject>();

    // User interface manager
    private UiManager uiManager;
    // Nymbers of enemy spawners in this level
    private int spawnNumbers;
	// Current loose counter
	private int beforeLooseCounter;
	// Victory or defeat condition already triggered
	private bool triggered = false;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
		// Load UI scene
		SceneManager.LoadScene(levelUiSceneName, LoadSceneMode.Additive);
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		uiManager = FindObjectOfType<UiManager>();
		SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
		spawnNumbers = spawnPoints.Length;
		if (spawnNumbers <= 0)
		{
			Debug.LogError("Have no spawners");
		}
		// Set random enemies list for each spawner
		foreach (SpawnPoint spawnPoint in spawnPoints)
		{
			spawnPoint.randomEnemiesList = allowedEnemies;
		}
		Debug.Assert(uiManager, "Wrong initial parameters");
		// Set gold amount for this level
		uiManager.SetGold(goldAmount);
		beforeLooseCounter = defeatAttempts;
		uiManager.SetDefeatAttempts(beforeLooseCounter);
	}

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("Captured", Captured);
        EventManager.StartListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("Captured", Captured);
        EventManager.StopListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    /// <summary>
    /// Enemy reached capture point.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void Captured(GameObject obj, string param)
    {
		if (beforeLooseCounter > 0)
		{
			beforeLooseCounter--;
			uiManager.SetDefeatAttempts(beforeLooseCounter);
			if (beforeLooseCounter <= 0)
			{
				triggered = true;
				// Defeat
				EventManager.TriggerEvent("Defeat", null, null);
			}
		}
    }

    /// <summary>
    /// All enemies are dead.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void AllEnemiesAreDead(GameObject obj, string param)
    {
        spawnNumbers--;
        // Enemies dead at all spawners
        if (spawnNumbers <= 0)
        {
			// Check if loose condition was not triggered before
			if (triggered == false)
			{
	            // Victory
				EventManager.TriggerEvent("Victory", null, null);
			}
        }
    }
}
