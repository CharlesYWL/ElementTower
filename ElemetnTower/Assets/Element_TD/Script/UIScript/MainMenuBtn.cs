using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBtn : MonoBehaviour
{
    private float GamePlaySilder = 0.5f;
    private float GameMusicSilder = 0.5f;
    public void StartGame()
    {
        AudioManager.instance.Play("clickbtn", SoundType.GAME);
        AdjustVolume.GameMusicVolume = GameMusicSilder;
        AdjustVolume.GamePlayVolume = GameMusicSilder;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartOption()
    {
        AudioManager.instance.Play("clickbtn", SoundType.GAME);
    }

    public void BackOption()
    {
        AudioManager.instance.Play("clickbtn", SoundType.GAME);
    }

    public void setGameVolume( float sliderValue)
    {
        AudioManager.instance.setGameVolume(sliderValue);
        AdjustVolume.GamePlayVolume = sliderValue;
        this.GamePlaySilder = sliderValue;
    }

    public void setMusicVolume(float sliderValue)
    {
        AudioManager.instance.setMusicVolume(sliderValue);
        AdjustVolume.GameMusicVolume = sliderValue;
        this.GameMusicSilder = sliderValue;
    }

}
