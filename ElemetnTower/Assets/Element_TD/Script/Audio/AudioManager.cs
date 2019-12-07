using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
        }
    }

    public void Play (string name)
    {
        Sound sound = new Sound();
        sound.name = "empty";
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                sound = s;
            }
        }

        if (sound.name != "empty")
        {
            sound.source.Play();
        }
    }
}
