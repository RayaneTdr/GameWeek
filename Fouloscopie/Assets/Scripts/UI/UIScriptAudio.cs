using UnityEngine;

public class UIScriptAudio : MonoBehaviour
{
    public void PlayButtonClicked(string name)
    {
        GameManager.Instance.audioManager.Play(name);
    }
}
