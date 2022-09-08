using UnityEngine;

[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(PlayerController))]
public class PlayerGrabber : MonoBehaviour
{
    //  Private Variables

    private PlayerController m_playerController;
    private PlayerCamera     m_playerCamera;

    private Obstacle m_selected;
    private int m_floorLayerMask = 0;
    private int m_obstacleLayerMask = 0;
    private int m_raycastTargetLayerMask = 0;

    private bool m_holdDrag = true;

    // MonoBehaviour Functions

    void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
        m_playerCamera = GetComponent<PlayerCamera>();

        m_floorLayerMask = LayerMask.GetMask("Floor");
        m_obstacleLayerMask = LayerMask.GetMask("Obstacle");
        m_raycastTargetLayerMask = LayerMask.GetMask("RaycastTarget");
    }


    void Update()
    {
        if (m_selected)
        {
            //  Rotate if rotation button is pressed
            if(m_playerController.rotateButtonHeldDown)
            {
                m_selected.Rotate(m_playerController.deltaMouse.x);
            }

            //  Else follow the cursor
            else if (m_playerCamera.RaycastToMouse(out RaycastHit hit, m_floorLayerMask))
            {
                m_selected.FollowCursor(hit.point);
            }
            else if(m_playerCamera.RaycastToMouse(out RaycastHit hitVoid, m_raycastTargetLayerMask))
            {
                m_selected.FollowCursor(hitVoid.point);
            }

            //  If mous left click is unpressed, place the selected
            if (m_holdDrag && m_playerController.leftClickUnpressed)
            {
                DropPreview();
            }
            
            if (!m_holdDrag && m_playerController.leftClickPressed)
            {
                DropPreview();
            }
        }
        else if(m_playerController.leftClickPressed)
        {
            if (m_playerCamera.RaycastToMouse(out RaycastHit hit, m_obstacleLayerMask))
            {
                if (hit.transform.gameObject.TryGetComponent<Obstacle>(out Obstacle obstacle))
                {
                    BeginDrag(obstacle);
                }
            }
        }
        else
        {
            m_playerController.freezeGrabMovement = false;
        }
    }

    //  Functions

    private void DropPreview()
    {
        bool wasNew = m_selected.isNew;

        // Try to drop the obstacle
        bool dropped = m_selected.Drop();

        //  if it was successfully dropped, and was a new obstacle, then consume points from the player
        if (dropped && wasNew)
        {
            m_playerController.obstacleLimit--;
        }

        //  Remove selected obstacle
        m_selected = null;
    }


    public void BeginDrag(Obstacle selected, bool holdDrag = true)
    {
        m_selected = selected;
        m_selected.Grab();

        m_playerController.freezeGrabMovement = true;
        m_holdDrag = holdDrag;
    }
}
