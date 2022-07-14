using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f,10f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    [Range(0, 255)]
    public int priority;
    public bool mute;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
