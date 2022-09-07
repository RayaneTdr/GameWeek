using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Settings settings;
    [HideInInspector] public AudioMixer audioMixer;
    [HideInInspector] public bool isPaused = false;

    #region SINGLETON
    static GameManager instance = null;
    static public GameManager Instance => instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            audioMixer = Resources.Load<AudioMixer>("MainMixer");
            settings = gameObject.AddComponent<Settings>();

            DontDestroyOnLoad(gameObject);

            return;
        }
        Destroy(this);
    }
    #endregion

    public void Pause()
    {
        Time.timeScale = 0.0f;
        isPaused = true;
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
    }

}
