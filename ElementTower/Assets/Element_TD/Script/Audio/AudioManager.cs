using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

public enum SoundType { GAME, SOUND };
public class AudioManager : MonoBehaviour
{
    public List<Sound> GameSounds = new List<Sound>();
    public List<Sound> GameMusics = new List<Sound>();
    static public AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in GameSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }

        foreach (Sound s in GameMusics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }
        instance = this;
    }

    public void AddSound(Sound s, SoundType type)
    {
        if (type == SoundType.GAME)
        {
            GameSounds.Add(s);
        }else
        {
            GameMusics.Add(s);
        }
    }

    public void Play(string name, SoundType type = SoundType.GAME)
    {
        Sound sound = new Sound();
        sound.name = "empty";

        if(type == SoundType.GAME)
        {
            foreach (Sound s in GameSounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    sound.volume = s.volume;
                    sound.pitch = s.pitch;
                    Debug.Log("found sound in game");
                }
            }
        }
        else
        {
            foreach (Sound s in GameMusics)
            {
                if (s.name == name)
                {
                    sound = s;
                    sound.volume = s.volume;
                    sound.pitch = s.pitch;
                }
            }
        }

        if (sound.name != "empty")
        {
            sound.source.Play();
        }
    }

    public void setGameVolume(float value)
    {
        foreach (Sound s in GameSounds)
        {
            s.source.volume = value;
        }
    }

    public void setMusicVolume(float value)
    {
        foreach (Sound s in GameMusics)
        {
            s.source.volume = value;
        }
    }
}
