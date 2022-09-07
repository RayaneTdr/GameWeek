using UnityEngine;
using UnityEngine.UI;

public class UIScriptSoundSettings : UIScriptSubMenu
{
    private Settings settings;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;


    private void OnEnable()
    {
        settings = GameManager.Instance.settings;

        globalVolumeSlider.value = settings.GlobalVolume;
        musicVolumeSlider.value = settings.MusicVolume;
    }

    public void SetGlobalVolume(float volume)
    {
        if(settings) settings.GlobalVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        if(settings) settings.MusicVolume = volume;
    }
}
