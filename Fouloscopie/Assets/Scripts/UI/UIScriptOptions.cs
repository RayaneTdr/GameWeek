using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScriptOptions : MonoBehaviour
{
    [SerializeField] protected Animator m_animator;

    protected bool m_isOpen = false;

    public void SetOpened()
    {
        m_isOpen = true;
    }
    public void SetClose()
    {
        m_isOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_animator.SetTrigger("Close");
        }
    }
}
