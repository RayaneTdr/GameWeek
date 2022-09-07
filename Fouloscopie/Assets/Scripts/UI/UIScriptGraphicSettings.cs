using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScriptGraphicSettings : UIScriptSubMenu
{
    private Settings settings;

    Resolution[] m_resolutions;

    [SerializeField]
    private TMP_Dropdown m_graphicLevelDropDown;

    [SerializeField]
    private TMP_Dropdown m_resolutionDropDown;

    [SerializeField]
    private Toggle m_fullScreenToggle;


    private void OnEnable()
    {
        settings = GameManager.Instance.settings;

        m_resolutions = Screen.resolutions;

        m_resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolution = 0;

        for (int i = 0; i < m_resolutions.Length; i++)
        {
            options.Add(m_resolutions[i].width + " x " + m_resolutions[i].height);

            if(m_resolutions[i].width == settings.CurrentResolution.width &&
               m_resolutions[i].height == settings.CurrentResolution.height)
            {
                currentResolution = i;
            }
        }

        m_resolutionDropDown.AddOptions(options);
        m_resolutionDropDown.value = currentResolution;

        m_graphicLevelDropDown.value = settings.GraphicsLevel;
        m_fullScreenToggle.isOn      = settings.FullScreen;
    }

    public void SetQuality(int level)
    {
        settings.GraphicsLevel = level;
    }

    public void SetFullScreen(bool value)
    {
        settings.FullScreen = value;
    }
    public void SetResolution(int index)
    {
        settings.CurrentResolution = m_resolutions[index];
    }
}
