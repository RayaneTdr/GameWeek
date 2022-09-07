using UnityEngine;

[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(PlayerController))]
public class PlayerGrabber : MonoBehaviour
{
    //  Private Variables

    private PlayerController m_playerController;
    private PlayerCamera     m_playerCamera;

    private Obstacle m_selected;
    private int floorLayerMask = 0;
    private int obstacleLayerMask = 0;

    // MonoBehaviour Functions

    void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
        m_playerCamera = GetComponent<PlayerCamera>();

        floorLayerMask = LayerMask.GetMask("Floor");
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
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
            else if (m_playerCamera.RaycastToMouse(out RaycastHit hit, floorLayerMask))
            {
                m_selected.FollowCursor(hit.point);
            }

            //  If mous left click is unpressed, place the selected
            if (m_playerController.leftClickUnpressed)
            {
                DropPreview();
            }
        }
        else if(m_playerController.leftClickPressed)
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
            m_playerCamera.freezeGrabMovements = false;
        }
    }

    //  Functions

    private void DropPreview()
    {
        //  Check if is new now since it will be changed in the Drop() function
        bool wasNew = m_selected.isNew;

        // Try to drop the obstacle, if it was successfully dropped, and was a new obstacle, then consume points from the player
        if (m_selected.Drop() && wasNew)
        {
            m_playerController.actionPoint -= m_selected.cost;
        }

        // Remove selected obstacle
        m_selected = null;
    }


    public void BeginDrag(Obstacle selected)
    {
        m_selected = selected;
        m_selected.Grab();

        m_playerCamera.freezeGrabMovements = true;
    }
}
