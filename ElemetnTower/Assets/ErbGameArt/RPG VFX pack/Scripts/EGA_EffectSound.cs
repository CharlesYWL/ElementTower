using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGA_EffectSound : MonoBehaviour
{
    public bool Repeating = true;
    public float RepeatTime = 2.0f;
    public float StartTime = 0.0f;
    public bool RandomVolume;
    public float minVolume = .4f;
    public float maxVolume = 1f;
    public float LifeTime = 5f;
    private float TimeCount=0;
    private AudioSource soundComponent;

    void Start ()
    {
        soundComponent = GetComponent<AudioSource>();
        if (RandomVolume == true)
        {
            soundComponent.volume = Random.Range(minVolume, maxVolume);
            RepeatSound();
        }
        if (Repeating == true)
        {
            InvokeRepeating("RepeatSound", StartTime, RepeatTime);
        }
    }

    void Update()
    {
        TimeCount += Time.deltaTime;
        if (TimeCount >= LifeTime)
        {
            Destroy(gameObject);
        }
    }
    void RepeatSound()
    {
        soundComponent.Play(0);

    }
}
