using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Timer to display current enemy wave.
/// </summary>
[RequireComponent(typeof(Image))]
public class WavesTimer : MonoBehaviour
{
	// Visualisation of remaining TO
	public Image timeBar;
    // Current wave text field
    public Text currentWaveText;
	// Max wave text field
    public Text maxWaveNumberText;
	// Effect of highlighted timer
	public GameObject highlightedFX;
	// Duration for highlighted effect
	public float highlightedTO = 0.2f;

	// Waves descriptor for this game level
	private WavesInfo wavesInfo;
    // Waves list
	private List<float> waves = new List<float>();
    // Current wave
    private int currentWave;
    // TO before next wave
    private float currentTimeout;
    // Time counter
    private float counter;
    // Timer stopped
    private bool finished;

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		StopAllCoroutines ();
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
    void Awake()
    {
		wavesInfo = FindObjectOfType<WavesInfo>();
		Debug.Assert(timeBar && highlightedFX && wavesInfo && timeBar && currentWaveText && maxWaveNumberText, "Wrong initial settings");
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
    {
		highlightedFX.SetActive(false);
		waves = wavesInfo.wavesTimeouts;
        currentWave = 0;
        counter = 0f;
        finished = false;
        GetCurrentWaveCounter();
        maxWaveNumberText.text = waves.Count.ToString();
        currentWaveText.text = "0";
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void FixedUpdate()
    {
        if (finished == false)
        {
			// Timeout expired
            if (counter <= 0f)
            {
				// Send event about next wave start
				EventManager.TriggerEvent("WaveStart", null, currentWave.ToString());
                currentWave++;
                currentWaveText.text = currentWave.ToString();
				// Highlight the timer for short time
				StartCoroutine("HighlightTimer");
				// All waves are sended
                if (GetCurrentWaveCounter() == false)
                {
                    finished = true;
					// Send event about timer stop
					EventManager.TriggerEvent("TimerEnd", null, null);
                    return;
                }
            }
			counter -= Time.fixedDeltaTime;
            if (currentTimeout > 0f)
            {
                timeBar.fillAmount = counter / currentTimeout;
            }
            else
            {
                timeBar.fillAmount = 0f;
            }
        }
	}

	/// <summary>
	/// Gets the current wave timeout.
	/// </summary>
	/// <returns><c>true</c>, if current wave timeout was gotten, <c>false</c> otherwise.</returns>
    private bool GetCurrentWaveCounter()
    {
        bool res = false;
        if (waves.Count > currentWave)
        {
            counter = currentTimeout = waves[currentWave];
            res = true;
        }
        return res;
    }

	/// <summary>
	/// Highlights the timer coroutine.
	/// </summary>
	/// <returns>The timer.</returns>
	private IEnumerator HighlightTimer()
	{
		highlightedFX.SetActive(true);
		yield return new WaitForSeconds(highlightedTO);
		highlightedFX.SetActive(false);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
