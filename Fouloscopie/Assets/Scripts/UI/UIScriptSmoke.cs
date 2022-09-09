using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScriptSmoke : MonoBehaviour
{
    [SerializeField]
    private GameObject m_originalPrefab;

    [SerializeField]
    private PlayerGrabber m_grabber;

    [SerializeField]
    private PlayerController m_player;

    public void OnClick()
    {
        GameObject go = Instantiate(m_originalPrefab);

        if (go.TryGetComponent(out Repulsive repulsive))
        {
            go.SetActive(false);
            m_grabber.LoadSmoke(repulsive);
        }
        else
        {
            Destroy(go);
        }
    }
}
