using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScriptMainMenu : MonoBehaviour
{
    [SerializeField] private string m_scene;
    [SerializeField] private string m_mainMenu;

    [SerializeField] GameObject transition;
    public void OnPlayButtonPressed()
    {
        Time.timeScale = 1f;
        transition.GetComponentInChildren<Animator>().SetTrigger("Play");
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

    public void LoadMap() 
    {
        SceneManager.LoadScene(m_scene);
    }
}
