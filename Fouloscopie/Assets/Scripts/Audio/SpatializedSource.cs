using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpatializedSource : MonoBehaviour
{
    AudioSource source;

    public void Start()
    {
        source = GetComponent<AudioSource>();    
    }

    public void Play(string clip) 
    {
        source = GetComponent<AudioSource>();
        source.clip = GameManager.Instance.audioManager.GetClip(clip);

        source.Play();
    }


}
