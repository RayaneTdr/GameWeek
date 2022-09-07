using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScriptSubMenu : MonoBehaviour
{
    [SerializeField] protected GameObject previousMenu;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void Back()
    {
        gameObject.SetActive(false);
        previousMenu.SetActive(true);
    }
}
