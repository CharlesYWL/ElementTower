using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBtn : MonoBehaviour
{
    private bool clicked = false;
    public void StartGame()
    {
        AudioManager.instance.Play("clickbtn", SoundType.GAME);
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
    }

    public void setMusicVolume(float sliderValue)
    {
        AudioManager.instance.setMusicVolume(sliderValue);
    }

}
