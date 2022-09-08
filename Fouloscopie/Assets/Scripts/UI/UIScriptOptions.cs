using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScriptOptions : MonoBehaviour
{
    [SerializeField] protected Animator m_animator;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_animator.SetTrigger("Close");
        }
    }
}
