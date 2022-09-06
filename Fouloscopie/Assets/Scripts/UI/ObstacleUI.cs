using UnityEngine;
using UnityEngine.EventSystems;

public class ObstacleUI : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField]
    private GameObject m_originalPrefab;

    [SerializeField]
    private PlayerGrabber m_grabber;

    [SerializeField]
    private PlayerController m_player;

    public void OnDrag(PointerEventData eventData) {}

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject go = Instantiate(m_originalPrefab);

        if(go.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            if (m_player.actionPoint >= obstacle.cost)
            {
                m_grabber.BeginDrag(obstacle);

                return;
            }
        }
        
        Destroy(go);
    }
}
