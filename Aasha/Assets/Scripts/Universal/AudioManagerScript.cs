using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;

    public Sound[] sounds;

    private int index;
    private int cataloguedIndex;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.mute = s.mute;
            s.source.priority = s.priority;
            s.source.loop = s.loop;
        }

        cataloguedIndex = 0;
    }

    private void Update()
    {
        index = SceneManager.GetActiveScene().buildIndex;

        if (cataloguedIndex != index) ChangeAmbience();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null && !s.source.isPlaying)
        {
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.mute = s.mute;
            s.source.priority = s.priority;
            s.source.loop = s.loop;
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Pause();
        }
    }

    public void End(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    public void ChangeAmbience()
    {
        Stop("dungeon ambience");
        Stop("dungeon ambience 2");
        Stop("dungeon ambience V2");
        Stop("solemn ambience");

        if (index == 1)
        {
            Play("dungeon ambience");
        }
        else if (3 <= index && index <= 5)
        {
            Play("dungeon ambience V2");
        }
        else if (6 <= index && index <= 9)
        {
            Play("dungeon ambience 2");
        }
    }
}
