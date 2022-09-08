using System;
using UnityEngine;

public class TutorialUtils : MonoBehaviour
{

    [SerializeField] GameObject player = null;
    [SerializeField] private bool forceTutotial = false;

    private void Awake()
    {
        gameObject.SetActive(!Convert.ToBoolean(PlayerPrefs.GetInt("Game.IgnoreTutorial", 0)) || forceTutotial);
    }

    public void StartTutorial() 
    {
        player.SetActive(false);
        WaveManager.Instance.Pause();

    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("Game.IgnoreTutorial", 1);

        // start the wave system
        WaveManager.Instance.StartGame();
        // activate player
        player.SetActive(true);

        gameObject.SetActive(false);
    }
}
