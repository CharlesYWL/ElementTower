using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
/// <summary>
/// Waves info inspector.
/// </summary>
public class WavesInfoInspector : MonoBehaviour
{
	[HideInInspector]
	// Timeouts between waves
	public List<float> timeouts
	{
		get
		{
			return wavesInfo.wavesTimeouts;
		}
		set
		{
			wavesInfo.wavesTimeouts = value;
		}
	}

	// Waves info component
	private WavesInfo wavesInfo;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		wavesInfo = GetComponent<WavesInfo>();
		Debug.Assert(wavesInfo, "Wrong stuff settings");
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	public void Update()
	{
		wavesInfo.Update();
	}
}
#endif
