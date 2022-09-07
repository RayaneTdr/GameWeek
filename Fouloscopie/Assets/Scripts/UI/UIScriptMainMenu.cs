using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScriptMainMenu : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("Zoo_Noe");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
