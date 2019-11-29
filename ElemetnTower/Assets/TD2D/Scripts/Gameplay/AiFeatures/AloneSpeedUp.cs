using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit will speed up if there is no any units in close.
/// </summary>
public class AloneSpeedUp : AiFeature
{
	// Speed up amount when alone
	public float speedUpAmount = 2f;

	// EffectControl of this instance
	private EffectControl effectControl;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		effectControl = GetComponentInParent<EffectControl>();
		Debug.Assert(effectControl, "Wrong initial settings");
	}

	public void OnTriggerAloneStart()
	{
		effectControl.AddConstantEffect("Speed", speedUpAmount, null);
	}

	public void OnTriggerAloneEnd()
	{
		effectControl.RemoveConstantEffect("Speed", speedUpAmount);
	}
}
