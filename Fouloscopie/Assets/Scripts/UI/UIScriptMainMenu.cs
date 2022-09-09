using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScriptMainMenu : MonoBehaviour
{
    [SerializeField] private string m_scene;
    [SerializeField] private string m_mainMenu;
    public void OnPlayButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(m_scene);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnMainMenuButtonPressed() 
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(m_mainMenu);

    }
}
