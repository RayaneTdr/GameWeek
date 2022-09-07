using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScriptPauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    public void Resume()
    {
        GameManager.Instance.Resume();
        player.ActivatePauseMenu(false);
    }

    public void Restart()
    {
        GameManager.Instance.Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        GameManager.Instance.Resume();
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }
}
