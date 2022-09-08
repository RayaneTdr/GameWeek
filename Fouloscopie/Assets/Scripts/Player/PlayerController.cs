using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //  Private Variables

    private Vector2 m_oldMousePos;
    private bool    m_isGrabbing;

    [SerializeField] private UIScriptPauseMenu m_pauseMenu;

    //  Public Variables

    [HideInInspector] public Vector2 cameraGrabMovement;
    [HideInInspector] public float scrollDelta = 0.0f;
    [HideInInspector] public Vector2 directionInput;
    [HideInInspector] public Vector2 mousePosition;
    [HideInInspector] public Vector2 deltaMouse;
    [HideInInspector] public float yawRotation;

    [HideInInspector] public bool leftClickPressed;
    [HideInInspector] public bool leftClickUnpressed;
    [HideInInspector] public bool leftClickHeldDown;

    [HideInInspector] public bool rightClickPressed;
    [HideInInspector] public bool rightClickUnpressed;
    [HideInInspector] public bool rightClickHeldDown;

    [HideInInspector] public bool middleClickPressed;
    [HideInInspector] public bool middleClickUnpressed;
    [HideInInspector] public bool middleClickHeldDown;

    [HideInInspector] public bool rotateButtonHeldDown;
    [HideInInspector] public bool pause;
    [HideInInspector] public bool freezeGrabMovement;

    //public float screenPercent = 0.05f;
    //[HideInInspector] public Vector2 cameraMovement;

    public int obstacleLimit = 5;

    // MonoBehaviour Functions

    void Update()
    {
        UpdateInputs();

        if (pause)
        {
            if (!GameManager.Instance.isPaused)
            {
                m_pauseMenu.Open();
            }
        }

        Vector2 mousePos = Input.mousePosition;

        deltaMouse = m_oldMousePos - mousePos;
        m_oldMousePos = mousePos;
        cameraGrabMovement = Vector2.zero;

        /*
        
        // Camera movement using window borders
        
        cameraMovement = Vector2.zero;

        float windowWidth = Screen.width;
        float windowHeight = Screen.height;

        float halfWidth = windowWidth * 0.5f;
        float halfHeight = windowHeight * 0.5f;
        cameraMovement = (mousePos - new Vector2(halfWidth, halfHeight));

        float ignoredPercent = 1.0f - screenPercent;

        Vector2 scale = new Vector2(
            Mathf.Clamp(Mathf.Abs(cameraMovement.x) / halfWidth - ignoredPercent, 0f, 1.0f),
            Mathf.Clamp(Mathf.Abs(cameraMovement.y) / halfHeight - ignoredPercent, 0f, 1.0f)
         );

        cameraMovement *= scale * 0.05f;
        */

        // WASD directionInput

        directionInput = directionInput.normalized * Mathf.Min(directionInput.magnitude, 1f);

        // Camera movement using grab

        cameraGrabMovement = Vector2.zero;

        // Is not over UI gameObject
        if(!EventSystem.current.IsPointerOverGameObject() && leftClickPressed) 
        {
            m_isGrabbing = true;
        }

        if (freezeGrabMovement) m_isGrabbing = false;

        if (m_isGrabbing)
        {
            cameraGrabMovement = deltaMouse;

            m_isGrabbing = !leftClickUnpressed;
        }

        // Camera zoom controls

        scrollDelta = Mathf.MoveTowards(scrollDelta, 0.0f, 0.1f);
        scrollDelta -= Input.mouseScrollDelta.y;

        // Yaw rotation controls

        if(middleClickHeldDown)
        {
            yawRotation = deltaMouse.x;
        }
        else
        {
            yawRotation = Mathf.Lerp(yawRotation, 0.0f, 0.1f);
        }
    }

    //  Functions

    private  void UpdateInputs()
    {
        directionInput.x = Input.GetAxis("Horizontal");
        directionInput.y = Input.GetAxis("Vertical");

        leftClickPressed   = Input.GetMouseButtonDown(0);
        leftClickUnpressed = Input.GetMouseButtonUp(0);
        leftClickHeldDown  = Input.GetMouseButton(0);

        rightClickPressed   = Input.GetMouseButtonDown(1);
        rightClickUnpressed = Input.GetMouseButtonUp(1);
        rightClickHeldDown  = Input.GetMouseButton(1);

        middleClickPressed = Input.GetMouseButtonDown(2);
        middleClickUnpressed = Input.GetMouseButtonUp(2);
        middleClickHeldDown = Input.GetMouseButton(2);
        
        rotateButtonHeldDown = Input.GetButton("Space");

        mousePosition = Input.mousePosition;

        pause = Input.GetKeyDown(KeyCode.Escape);
    }
}
