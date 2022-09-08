using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUtils : MonoBehaviour
{

    [SerializeField] GameObject player = null;

    public void StartTutorial() 
    {
        player.SetActive(false);
    }

    public void StartGame() 
    {
        // start the wave system
        WaveManager.Instance.StartGame();
        // activate player
        player.SetActive(true);

        gameObject.SetActive(false);
    }
}
