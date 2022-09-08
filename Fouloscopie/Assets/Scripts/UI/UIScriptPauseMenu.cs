using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScriptPauseMenu : UIScriptOptions
{
    [SerializeField] private PlayerController player;

    private bool m_isOpen = false;
    private bool m_wasOpen = false;

    public void Open()
    {
        m_animator.SetTrigger("Open");
        m_isOpen = true;
    }

    public void Close()
    {
        m_animator.SetTrigger("Close");
        m_isOpen = false;
    }

    public void Resume()
    {
        GameManager.Instance.Resume();
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
        if(Input.GetKeyDown(KeyCode.Escape) && m_wasOpen && m_isOpen)
        {
            Resume();
        }

        m_wasOpen = m_isOpen;
    }
}
