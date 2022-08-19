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

    public bool mute;

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

        if(Input.GetKeyDown(KeyCode.M))
        {
            Mute();
        }
    }

    public void Mute()
    {
        foreach (Sound s in sounds)
        {
            s.mute = !s.mute;
            mute = s.mute;
        }
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

    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            if (s.source.isPlaying) return true;
        }

        return false;
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
            Stop("day sounds");
            Stop("night sounds");
        }
        else if (3 <= index && index <= 5)
        {
            Play("dungeon ambience V2");
            Stop("day sounds");
            Stop("night sounds");
        }
        else if (6 <= index && index <= 9)
        {
            Play("dungeon ambience 2");
            Stop("day sounds");
            Stop("night sounds");
        }
    }
}
