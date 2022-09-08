using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScriptPauseMenu : UIScriptOptions
{
    [SerializeField] private PlayerController player;

    public void Open()
    {
        if (!m_isOpen)
        {
            GameManager.Instance.Pause();
            m_animator.SetTrigger("Open");
        }
    }

    public void Close()
    {
        if (m_isOpen)
        {
            GameManager.Instance.Resume();
            m_animator.SetTrigger("Close");
        }
    }

    public void Resume()
    {
        Close();
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
