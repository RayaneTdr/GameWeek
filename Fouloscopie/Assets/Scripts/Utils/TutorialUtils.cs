using System;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialUtils : MonoBehaviour
{

    [SerializeField] GameObject player = null;
    [SerializeField] private bool forceTutotial = false;
    [SerializeField] GameObject transition = null;
    
    PlayableDirector director;

    private void Awake()
    {
        gameObject.SetActive(!Convert.ToBoolean(PlayerPrefs.GetInt("Game.IgnoreTutorial", 0)) || forceTutotial);
        director = GetComponent<PlayableDirector>();
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

    public void SkipTutorial() 
    {
        director.Stop();
        WaveManager.Instance.StartGame();
        // activate player
        player.SetActive(true);

        gameObject.SetActive(false);
        
        if (transition)
            transition.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SkipTutorial();
    }
}
