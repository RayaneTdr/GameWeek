using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpatializedSource : MonoBehaviour
{
    AudioSource source;
    public string soundName = "";
    
    public void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = GameManager.Instance.audioManager.GetClip(soundName);
    }

    public void Play() 
    {
        source.Play();
    }


}
