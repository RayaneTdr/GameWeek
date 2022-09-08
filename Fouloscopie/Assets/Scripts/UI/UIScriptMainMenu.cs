using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScriptMainMenu : MonoBehaviour
{
    [SerializeField] private string m_scene;
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(m_scene);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
