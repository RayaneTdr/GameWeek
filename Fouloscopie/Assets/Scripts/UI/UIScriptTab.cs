using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScriptTab : MonoBehaviour
{
    [SerializeField] private Button[] m_tabs;

    public void OpenTab(int index)
    {
        foreach(Button btn in m_tabs)
        {
            btn.enabled = true;
        }

        m_tabs[index].enabled = false;
    }
}
