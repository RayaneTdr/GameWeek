using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public List<AudioClip> clip = new List<AudioClip>(); 
    public AudioMixerGroup group;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;


    [HideInInspector] public AudioSource source;

    public void Play()
    {
        if(source && clip.Count > 0)
        {
            source.clip = clip[Random.Range(0, clip.Count)];
            source.Play();
        }
    }
}
