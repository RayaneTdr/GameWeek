using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TicketRenderer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deadEntry;
    [SerializeField] TextMeshProUGUI savedEntry;
    [SerializeField] AudioSource mainMusic;

    public void Display() 
    {
        mainMusic.Stop();
        GetComponentInChildren<Animator>().SetTrigger("Activate");
        foreach (AudioSource s in GetComponents<AudioSource>())
        {
            s.Play();
        }
    }

    public void Update()
    {
        deadEntry.text = "Dead: " + WaveManager.diedDummies;
        savedEntry.text = "Satisfied Clients: " + WaveManager.savedDummies;
    }
}
