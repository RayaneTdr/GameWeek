using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public List<AudioClip> clip = new List<AudioClip>(); 
    public AudioMixerGroup group;

    public bool spatialized = false;

    public void Play(AudioSource source)
    {
        if(source && clip.Count > 0)
        {
            source.outputAudioMixerGroup = group;
            source.PlayOneShot(clip[Random.Range(0, clip.Count)]);
        }
    }
}
