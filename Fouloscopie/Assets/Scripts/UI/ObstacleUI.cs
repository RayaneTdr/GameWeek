using UnityEngine;

public class ObstacleUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_originalPrefab;

    [SerializeField]
    private PlayerGrabber m_grabber;

    public void OnClick()
    {
        GameObject go = Instantiate(m_originalPrefab);

        if (go.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            m_grabber.BeginDrag(obstacle, false);
        }
        else
        {
            Destroy(go);
        }
    }
}
