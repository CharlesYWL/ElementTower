using UnityEngine.Audio;
using UnityEngine;

public enum SoundType { GAME, SOUND };
public class AudioManager : MonoBehaviour
{
    public Sound[] GameSounds;
    public Sound[] GameMusics;
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

    public void Play(string name, SoundType type = SoundType.GAME)
    {
        Debug.Log("hit played");
        Sound sound = new Sound();
        sound.name = "empty";

        if(type == SoundType.GAME)
        {
            Debug.Log("game type");
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
            Debug.Log("music type");
            foreach (Sound s in GameMusics)
            {
                if (s.name == name)
                {
                    sound = s;
                    sound.volume = s.volume;
                    sound.pitch = s.pitch;
                    Debug.Log("found sound in music");
                }
            }
        }

        if (sound.name != "empty")
        {
            Debug.Log("found music");
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
