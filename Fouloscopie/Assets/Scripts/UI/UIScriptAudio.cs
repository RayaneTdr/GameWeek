using UnityEngine;

public class UIScriptAudio : MonoBehaviour
{
    public void PlayButtonHovered()
    {
        //Debug.Log("PLACEHOLDER : Make HOVER sound");
        //GameManager.Instance.audioManager.Play("");
    }


    public void PlayButtonClicked()
    {
        Debug.Log("PLACEHOLDER : Make CLICK sound");
        GameManager.Instance.audioManager.Play("ButtonClick");
    }
}
