using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Build the tower.
/// </summary>
public class TowerActionBuild : TowerAction
{
    // Tower prefab for this icon
    public GameObject towerPrefab;
	// Icon for disabled state
	public GameObject disabledIcon;
	// Icon for blocked state while player has not anough gold
	public GameObject blockedIcon;

    // Text field for tower price
	private Text priceText;
	// Price of tower in gold
	private int price = 0;
	// Level manger has a list with allowed tower upgrades for this level.
	private LevelManager levelManager;
	// User interface manager allows to check current gold amount
	private UiManager uiManager;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        priceText = GetComponentInChildren<Text>();
		levelManager = FindObjectOfType<LevelManager>();
		uiManager = FindObjectOfType<UiManager>();
		Debug.Assert(priceText && towerPrefab && enabledIcon && disabledIcon && levelManager && uiManager, "Wrong initial parameters");
        // Display tower price
		price = towerPrefab.GetComponent<Price>().price;
		priceText.text = price.ToString();
		if (levelManager.allowedTowers.Contains(towerPrefab) == true)
		{
			enabledIcon.SetActive(true);
			disabledIcon.SetActive(false);
		}
		else
		{
			enabledIcon.SetActive(false);
			disabledIcon.SetActive(true);
		}
    }

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// Mask build icon wich blocking icon if player has not anough gold
		if (enabledIcon == true && blockedIcon != null)
		{
			if (uiManager.GetGold() >= price)
			{
				blockedIcon.SetActive(false);
			}
			else
			{
				blockedIcon.SetActive(true);
			}
		}
	}

	/// <summary>
	/// Clicked this instance.
	/// </summary>
	protected override void Clicked()
	{
		// If player has anough gold
		if (blockedIcon == null || blockedIcon.activeSelf == false)
		{
			// Build the tower
			Tower tower = GetComponentInParent<Tower>();
			if (tower != null)
			{
				tower.BuildTower(towerPrefab);
			}
		}
	}
}
