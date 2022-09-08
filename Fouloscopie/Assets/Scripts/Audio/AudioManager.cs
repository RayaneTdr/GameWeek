using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] m_sounds;

    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void Play(string name)
    {
        Array.Find(m_sounds, sound => sound.name == name)?.Play(source);
    }

    public AudioClip GetClip(string name) 
    {
        Sound s = Array.Find(m_sounds, sound => sound.name == name);
        return s.clip[UnityEngine.Random.Range(0, s.clip.Count)]; ;
    }
}
