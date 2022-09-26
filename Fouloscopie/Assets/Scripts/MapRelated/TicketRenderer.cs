using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TicketRenderer : MonoBehaviour
{
    [SerializeField] TMP_Text deadEntry;
    [SerializeField] TMP_Text savedEntry;
    [SerializeField] TMP_Text smokeTXT;
    [SerializeField] TMP_Text crushedTXT;
    [SerializeField] TMP_Text attractedTXT;
    [SerializeField] TMP_Text carriedTXT;
    [SerializeField] TMP_Text placedTXT;
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
        deadEntry.text    = "Dead: "                + WaveManager.diedDummies;
        savedEntry.text   = "Satisfied Clients: "   + WaveManager.savedDummies;
        smokeTXT.text     = "Launched Smokes: "     + WaveManager.placedSmokes;
        crushedTXT.text   = "Crushed Clients: "     + WaveManager.crushedDummies;
        attractedTXT.text = "Attracted Clients: "   + WaveManager.attractedDummies;
        carriedTXT.text   = "Carried Articles: "    + WaveManager.carriedArticles;
        placedTXT.text    = "Replaced Buildings: "  + WaveManager.replacedBuildings;
    }
}
