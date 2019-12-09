using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustVolume : MonoBehaviour
{
    public static float GamePlayVolume;
    public static float GameMusicVolume;
    // Start is called before the first frame update
    void Awake()
    {
        AudioManager.instance.Play("battlebgm", SoundType.SOUND);
    }

    void setGameVolume()
    {
        AudioManager.instance.setGameVolume(GamePlayVolume);
    }

    void setMusicVolume()
    {
        AudioManager.instance.setMusicVolume(GameMusicVolume);
    }

    void Update()
    {
        setMusicVolume();
        setGameVolume();
    }

}
