using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustVolume : MonoBehaviour
{
    public static float GamePlayVolume = .5f;
    public static float GameMusicVolume = .5f;

    // Start is called before the first frame update
    void Start()
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

    public void setGameVolume(float sliderValue)
    {
        GamePlayVolume = sliderValue;
        AudioManager.instance.setGameVolume(GamePlayVolume);
    }

    public void setMusicVolume(float sliderValue)
    {
        GameMusicVolume = sliderValue;
        AudioManager.instance.setMusicVolume(GameMusicVolume);
    }

    void Update()
    {
        setMusicVolume();
        setGameVolume();
    }

}
