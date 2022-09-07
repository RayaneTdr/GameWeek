using System;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // Sound

    private float m_musicVolume;
    private float m_globalVolume;

    // Graphics

    private Resolution m_resolution;
    private bool       m_fullScreen;
    private int        m_level;


    //  Properties

    public float GlobalVolume
    {
        get { return m_globalVolume; }

        set
        {
            if (m_globalVolume != value)
            {
                m_globalVolume = value;
                GameManager.Instance.audioMixer.SetFloat("GlobalVolume", value);
                PlayerPrefs.SetFloat("Settings.Sound.GlobalVolume", value);
            }
        }
    }

    public float MusicVolume   
    { 
        get { return m_musicVolume; } 

        set 
        { 
            if (m_musicVolume != value)
            {
                m_musicVolume = value;

                GameManager.Instance.audioMixer.SetFloat("MusicVolume", value);
                PlayerPrefs.SetFloat("Settings.Sound.MusicVolume", value);
            }
        }
    }

    public Resolution CurrentResolution 
    { 
        get { return m_resolution; }

        set
        {
            if (m_resolution.width != value.width || m_resolution.height != value.height)
            {
                m_resolution = value;

                Screen.SetResolution(value.width, value.height, Screen.fullScreen);
                PlayerPrefs.SetInt("Settings.Graphics.Resolution.Width",  value.width);
                PlayerPrefs.SetInt("Settings.Graphics.Resolution.Height", value.height);
            }
        }
    }
    public bool FullScreen   
    { 
        get { return m_fullScreen; }

        set
        {
            if (m_fullScreen != value)
            {
                m_fullScreen = value;

                Screen.fullScreen = value;

                PlayerPrefs.SetInt("Settings.Graphics.FullScreen", Convert.ToInt32(value));
            }
        }
    }
    public int GraphicsLevel { 
        get { return m_level; }

        set
        {
            if (m_level != value)
            {
                m_level = value;

                QualitySettings.SetQualityLevel(value);
                PlayerPrefs.SetInt("Settings.Graphics.Level", value);
            }
        }
    }


    private void Awake()
    {
        MusicVolume  = PlayerPrefs.GetFloat("Settings.Sound.MusicVolume", 0.0f);
        GlobalVolume = PlayerPrefs.GetFloat("Settings.Sound.GlobalVolume", 0.0f);

        FullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("Settings.Graphics.FullScreen", 1));
        GraphicsLevel = PlayerPrefs.GetInt("Settings.Graphics.Level", 1);

        Resolution res = new Resolution();
        res.width = PlayerPrefs.GetInt("Settings.Graphics.Resolution.Width", Screen.currentResolution.width);
        res.height = PlayerPrefs.GetInt("Settings.Graphics.Resolution.Height", Screen.currentResolution.height);
        CurrentResolution = res;
    }

    private void Start()
    {
        //  Audio mixer overrride values by default one after the awake
        //  So reset here audio mixer values
        GameManager.Instance.audioMixer.SetFloat("GlobalVolume", m_globalVolume);
        GameManager.Instance.audioMixer.SetFloat("MusicVolume", m_musicVolume);

    }
}
