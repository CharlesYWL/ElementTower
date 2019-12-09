using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustTowerVolume : MonoBehaviour
{
    Sound s = new Sound();
    // Start is called before the first frame update
    void Awake()
    {
        s.source.clip = this.GetComponent<AudioSource>().clip;
        s.source.volume = this.GetComponent<AudioSource>().volume;
        AudioManager.instance.AddSound(s, SoundType.GAME);
    }

    // Update is called once per frame
    void Update()
    { 
    }
}
