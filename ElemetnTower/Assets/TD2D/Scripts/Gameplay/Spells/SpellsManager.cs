using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls spells allowed for this level.
/// </summary>
public class SpellsManager : MonoBehaviour
{
	// Folder for spells
	public Transform spellsFolder;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		LevelManager levelManager = FindObjectOfType<LevelManager>();
		Debug.Assert(spellsFolder && levelManager, "Wrong initial settings");
		foreach (UserActionIcon spell in spellsFolder.GetComponentsInChildren<UserActionIcon>())
		{
			Destroy(spell.gameObject);
		}
		// Add allowed spells in spells UI folder
		foreach (GameObject spell in levelManager.allowedSpells)
		{
			Instantiate(spell, spellsFolder);
		}
	}
}
