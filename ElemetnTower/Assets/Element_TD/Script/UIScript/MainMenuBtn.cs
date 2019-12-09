using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBtn : MonoBehaviour
{
    public GameObject options;
    private bool clicked = false;
    public void StartGame()
    {
        AudioManager.instance.Play("clickbtn", SoundType.GAME);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartOption()
    {
        AudioManager.instance.Play("clickbtn", SoundType.GAME);
        if (!clicked)
        {
            this.options.SetActive(true);
            swtichToggle();
        }
        else
        {
            this.options.SetActive(false);
            swtichToggle();
        }
    }

    public void BackOption()
    {
        AudioManager.instance.Play("clickbtn", SoundType.GAME);
    }

    private void swtichToggle()
    {
        if (clicked)
        {
            clicked = false;
        }
        else
        {
            clicked = true;
        }
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
