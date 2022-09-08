using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] m_sounds;


    private void Awake()
    {
        foreach(Sound s in m_sounds)
        {
            if (!s.spatialized)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.outputAudioMixerGroup = s.group;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
    }

    public void Play(string name)
    {
        Array.Find(m_sounds, sound => sound.name == name)?.Play();
    }

    public AudioClip GetClip(string name) 
    {
        Sound s = Array.Find(m_sounds, sound => sound.name == name);
        return s.clip[UnityEngine.Random.Range(0, s.clip.Count)]; ;
    }
}
