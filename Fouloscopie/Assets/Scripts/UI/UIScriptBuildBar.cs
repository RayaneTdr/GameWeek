using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIScriptBuildBar : MonoBehaviour
{
    [SerializeField] private PlayerGrabber    m_playerGrabber;
    [SerializeField] private PlayerController m_playerController;

    [SerializeField] private Button[]      m_buttons;
    [SerializeField] private GameObject[]  m_obstacles;

    [SerializeField] private TextMeshProUGUI m_counter;
    [SerializeField] private TextMeshProUGUI m_limit;

    private bool cleared = false;


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

        m_counter.text = m_playerGrabber.GetObstacleCount().ToString();
        m_limit.text   = m_playerController.obstacleLimit.ToString();
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

        if (go.TryGetComponent(out Obstacle obstacle))
        {
            m_playerGrabber.BeginDrag(obstacle, false);
        }
        else if(go.TryGetComponent(out Repulsive rep))
        {
            go.SetActive(false);
            m_playerGrabber.LoadSmoke(rep);
        }
        else
        {
            Destroy(go);
        }
    }
}
