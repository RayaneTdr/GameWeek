using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //public float screenPercent = 0.05f;

    [HideInInspector] public Vector2 cameraMovement;
    [HideInInspector] public Vector2 cameraGrabMovement;
    [HideInInspector] public float scrollDelta = 0.0f;

    private Vector2 m_deltaMouse;
    private Vector2 m_oldMousePos;
    private bool    m_isGrabbing;

    public int actionPoint = 100;


    void Update()
    {
        cameraMovement = Vector2.zero;
        
        Vector2 mousePos = Input.mousePosition;

        m_deltaMouse = m_oldMousePos - mousePos;
        m_oldMousePos = mousePos;

        // Camera movement using window borders

        /*
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

        // Camera movement using grab

        cameraGrabMovement = Vector2.zero;

        // Is not over UI gameObject
        if(!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0)) 
        {
            m_isGrabbing = true;
        }

        if (m_isGrabbing)
        {
            cameraGrabMovement = m_deltaMouse;
            cameraMovement = Vector2.zero;

            m_isGrabbing = !Input.GetMouseButtonUp(0);
        }

        // Camera zoom controls

        scrollDelta = Mathf.MoveTowards(scrollDelta, 0.0f, 0.1f);
        scrollDelta -= Input.mouseScrollDelta.y;

    }

}
