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

    public bool spatialized = false;

    public void Play(AudioSource source)
    {
        if(source && clip.Count > 0)
        {
            source.outputAudioMixerGroup = group;
            source.pitch = pitch;
            source.volume = volume;
            source.PlayOneShot(clip[Random.Range(0, clip.Count)]);
        }
    }
}
