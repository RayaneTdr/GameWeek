using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIScriptBuildBar : MonoBehaviour
{
    [SerializeField] private PlayerGrabber m_playerGrabber;
    [SerializeField] private Button[]      m_buttons;
    [SerializeField] private GameObject[]  m_obstacles;

    private bool cleared = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!cleared && m_playerGrabber.IsFree())
        {
            foreach (Button btn in m_buttons)
            {
                btn.interactable = true;
            }

            cleared = true;
        }
    }

    public void OnClick(int index)
    {
        foreach(Button btn in m_buttons)
        {
            btn.interactable = true;
        }

        m_buttons[index].interactable = false;

        cleared = false;

        GameObject go = Instantiate(m_obstacles[index]);

        if (go.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            m_playerGrabber.BeginDrag(obstacle, false);
        }
        else
        {
            Destroy(go);
        }
    }
}
