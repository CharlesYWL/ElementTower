using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays soundtrack and sound effects. Limits number of simultaneously played sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
	// Singleton
	public static AudioManager instance;

	// Sound source for sound effects
	public AudioSource soundSource;
	// Sound source for soundtrack
	public AudioSource musicSource;
	// Soundtrack
	public AudioClip track;
	// Wave start sfx
	public AudioClip waveStart;
	// Enemy reached capture point sfx
	public AudioClip captured;
	// Player click on tower sfx
	public AudioClip towerClick;
	// Player click on unit sfx
	public AudioClip unitClick;
	// Player click UI sfx
	public AudioClip uiClick;
	// Tower build sfx
	public AudioClip towerBuild;
	// Tower sell sfx
	public AudioClip towerSell;
	// Defeat sfx
	public AudioClip defeat;
	// Victory sfx
	public AudioClip victory;

	// Attack sfx is played now
	private bool attackCoroutine = false;
	// Die sfx is played now
	private bool dieCoroutine = false;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		instance = this;
		EventManager.StartListening("GamePaused", GamePaused);
		EventManager.StartListening("WaveStart", WaveStart);
		EventManager.StartListening("Captured", Captured);
		EventManager.StartListening("UserClick", UserClick);
		EventManager.StartListening("UserUiClick", UserUiClick);
		EventManager.StartListening("TowerBuild", TowerBuild);
		EventManager.StartListening("TowerSell", TowerSell);
		EventManager.StartListening("Defeat", Defeat);
		EventManager.StartListening("Victory", Victory);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("GamePaused", GamePaused);
		EventManager.StopListening("WaveStart", WaveStart);
		EventManager.StopListening("Captured", Captured);
		EventManager.StopListening("UserClick", UserClick);
		EventManager.StopListening("UserUiClick", UserUiClick);
		EventManager.StopListening("TowerBuild", TowerBuild);
		EventManager.StopListening("TowerSell", TowerSell);
		EventManager.StopListening("Defeat", Defeat);
		EventManager.StopListening("Victory", Victory);
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Debug.Assert(soundSource && musicSource, "Wrong initial settings");
		// Set volume from stored configurations
		SetVolume(DataManager.instance.configs.soundVolume, DataManager.instance.configs.musicVolume);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		StopAllCoroutines();
		if (instance == this)
		{
			instance = null;
		}
	}

	/// <summary>
	/// Games the paused.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void GamePaused(GameObject obj, string param)
	{
		if (param == bool.TrueString) // Paused
		{
			// Pause soundtrack
			musicSource.Pause();
		}
		else // Unpaused
		{
			// Play soundtrack
			if (track != null)
			{
				musicSource.clip = track;
				musicSource.Play();
			}
		}
	}

	/// <summary>
	/// Sets the volume.
	/// </summary>
	/// <param name="sound">Sound.</param>
	/// <param name="music">Music.</param>
	public void SetVolume(float sound, float music)
	{
		soundSource.volume = sound;
		musicSource.volume = music;
	}

	/// <summary>
	/// Plaies the sfx (without limit).
	/// </summary>
	/// <param name="audioClip">Audio clip.</param>
	public void PlaySound(AudioClip audioClip)
	{
		soundSource.PlayOneShot(audioClip, soundSource.volume);
	}

	/// <summary>
	/// Plaies the attack sfx (one sfx at the same time).
	/// </summary>
	/// <param name="audioClip">Audio clip.</param>
	public void PlayAttack(AudioClip audioClip)
	{
		if (attackCoroutine == false)
		{
			StartCoroutine(AttackCoroutine(audioClip));
		}
	}

	/// <summary>
	/// Attacks coroutine.
	/// </summary>
	/// <returns>The coroutine.</returns>
	/// <param name="audioClip">Audio clip.</param>
	private IEnumerator AttackCoroutine(AudioClip audioClip)
	{
		attackCoroutine = true;
		PlaySound(audioClip);
		// Wait for clip end
		yield return new WaitForSeconds(audioClip.length);
		attackCoroutine = false;
	}

	/// <summary>
	/// Plaies the die sfx (one sfx at the same time).
	/// </summary>
	/// <param name="audioClip">Audio clip.</param>
	public void PlayDie(AudioClip audioClip)
	{
		if (dieCoroutine == false)
		{
			StartCoroutine(DieCoroutine(audioClip));
		}
	}

	/// <summary>
	/// Dies coroutine.
	/// </summary>
	/// <returns>The coroutine.</returns>
	/// <param name="audioClip">Audio clip.</param>
	private IEnumerator DieCoroutine(AudioClip audioClip)
	{
		dieCoroutine = true;
		PlaySound(audioClip);
		// Wait for clip end
		yield return new WaitForSeconds(audioClip.length);
		dieCoroutine = false;
	}

	/// <summary>
	/// Waves started.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void WaveStart(GameObject obj, string param)
	{
		if (waveStart != null)
		{
			PlaySound(waveStart);
		}
	}

	/// <summary>
	/// Enemy reached capture point.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void Captured(GameObject obj, string param)
	{
		if (captured != null)
		{
			PlaySound(captured);
		}
	}

	/// <summary>
	/// On user UI click.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UserUiClick(GameObject obj, string param)
	{
		if (obj != null)
		{
			PlaySound(uiClick);
		}
	}

	/// <summary>
	/// User click handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void UserClick(GameObject obj, string param)
	{
		if (obj != null)
		{
			Tower tower = obj.GetComponent<Tower>();
			if (tower != null)
			{
				PlaySound(towerClick);
			}
			else
			{
				UnitInfo unitInfo = obj.GetComponent<UnitInfo>();
				if (unitInfo != null)
				{
					PlaySound(unitClick);
				}
			}
		}
	}

	/// <summary>
	/// Towers build handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void TowerBuild(GameObject obj, string param)
	{
		if (towerBuild != null)
		{
			PlaySound(towerBuild);
		}
	}

	/// <summary>
	/// Towers sell handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void TowerSell(GameObject obj, string param)
	{
		if (towerSell != null)
		{
			PlaySound(towerSell);
		}
	}

	/// <summary>
	/// Defeat handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void Defeat(GameObject obj, string param)
	{
		if (defeat != null)
		{
			PlaySound(defeat);
		}
	}

	/// <summary>
	/// Victory handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void Victory(GameObject obj, string param)
	{
		if (victory != null)
		{
			PlaySound(victory);
		}
	}
}
