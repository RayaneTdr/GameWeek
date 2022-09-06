using UnityEngine;

[RequireComponent(typeof(PlayerCamera))]
public class PlayerGrabber : MonoBehaviour
{
    private Obstacle m_selected;
    private PlayerCamera m_playerCamera;

    private int floorLayerMask = 0;
    private int obstacleLayerMask = 0;

    void Awake()
    {
        m_playerCamera = GetComponent<PlayerCamera>();

        floorLayerMask = LayerMask.GetMask("Floor");
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_selected)
        {
            // Update selected transform
            if (m_playerCamera.RaycastToMouse(out RaycastHit hit, floorLayerMask))
            {
                m_selected.FollowCursor(hit.point);
            }

            // If mous left click is unpressed, place the selected
            if (Input.GetMouseButtonUp(0))
            {
                DropPreview();
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            if (m_playerCamera.RaycastToMouse(out RaycastHit hit, obstacleLayerMask))
            {
                if (hit.transform.gameObject.TryGetComponent<Obstacle>(out Obstacle obstacle))
                {
                    BeginDrag(obstacle);
                }
            }
        }
        else
        {
            m_playerCamera.freeze = false;
        }
    }

    private void DropPreview()
    {
        m_selected.Drop();
        m_selected = null;
    }


    public void BeginDrag(Obstacle selected)
    {
        m_selected = selected;
        m_selected.Grab();

        m_playerCamera.freeze = true;
    }
}
