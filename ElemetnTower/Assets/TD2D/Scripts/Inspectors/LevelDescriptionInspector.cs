using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[ExecuteInEditMode]
/// <summary>
/// Level description inspector.
/// </summary>
public class LevelDescriptionInspector : MonoBehaviour
{
	// Level icon
	public Image icon;
	// Level header
	public Text header;
	// Level description
	public Text description;
	// Level attention
	public Text attention;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		Debug.Assert(icon && header && description && attention, "Wrong level description stuff settings");
	}
}
#endif
