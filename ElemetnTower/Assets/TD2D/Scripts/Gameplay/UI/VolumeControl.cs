using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the sound effects and soundtrack volume via sliders.
/// </summary>
public class VolumeControl : MonoBehaviour
{
	// Slider for sound effects volume
	public Slider sound;
	// Slider for soundtrack volume
	public Slider music;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Debug.Assert(sound && music, "Wrong initial settings");
		sound.value = DataManager.instance.configs.soundVolume;
		music.value = DataManager.instance.configs.musicVolume;
		sound.onValueChanged.AddListener(delegate {OnVolumeChanged();});
		music.onValueChanged.AddListener(delegate {OnVolumeChanged();});
	}

	/// <summary>
	/// Raises the volume changed event.
	/// </summary>
	private void OnVolumeChanged()
	{
		// Store new settings
		DataManager.instance.configs.soundVolume = sound.value;
		DataManager.instance.configs.musicVolume = music.value;
		DataManager.instance.SaveGameConfigs();
		// Apply new settings
		AudioManager.instance.SetVolume(DataManager.instance.configs.soundVolume, DataManager.instance.configs.musicVolume);
	}
}
