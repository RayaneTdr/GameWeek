using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScriptMainMenu : MonoBehaviour
{
    [SerializeField] private string m_scene;
    [SerializeField] private string m_mainMenu;
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(m_scene);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnMainMenuButtonPressed() 
    {
        SceneManager.LoadScene(m_mainMenu);

    }
}
