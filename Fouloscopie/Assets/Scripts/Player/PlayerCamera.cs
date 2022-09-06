using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Transform))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField]  
    float m_rotation = 75f;

    [Header("Zoom / Offset parameters")]

    [SerializeField, Range(0.0f, 1.0f)]
    private float m_minZoom = 1.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_maxZoom = 0.05f;
    [SerializeField, Range(-100.0f, -10.0f)]
    private float m_offset = -50f;

    [Header("Speed parameters")]

    [SerializeField, Range(5.0f, 50.0f)]
    private float m_movementSpeed = 25f;
    [SerializeField, Range(0.01f, 1.0f)] 
    private float m_zoomSpeed = 0.05f;

    [HideInInspector] 
    public Camera cameraComponent;
    [HideInInspector] 
    public bool freeze = false;

    private PlayerController m_controller;
    private float m_zoomLevel = 1f;

    // MonoBehaviour Functions

    void Awake()
    {
        m_controller = GetComponent<PlayerController>();
        cameraComponent = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(m_rotation, 0f, 0f);
        cameraComponent.transform.localPosition += Vector3.back * m_offset * m_zoomLevel;
    }

    void Update()
    {
        if (!freeze) UpdatePosition();

        UpdateZoom();

        if (Input.GetMouseButtonDown(1)) StartShaking(0.25f, 0.1f);
    }

    // Functions

    public void UpdatePosition()
    {
        Vector2 movement = /*m_controller.cameraMovement * m_movementSpeed + */m_controller.cameraGrabMovement * m_zoomLevel * m_movementSpeed;

        Vector3 horizontalMovement = new Vector3(movement.x, 0.0f, movement.y);
        transform.position += Time.deltaTime * horizontalMovement;
    }

    private void UpdateZoom()
    {
        m_zoomLevel = Mathf.Clamp(m_zoomLevel + m_controller.scrollDelta * m_zoomSpeed, m_maxZoom, m_minZoom);
        float height = Mathf.Lerp(cameraComponent.transform.localPosition.z, m_offset * m_zoomLevel, 0.01f);
        
        cameraComponent.transform.localPosition = new Vector3(
            cameraComponent.transform.localPosition.x,
            cameraComponent.transform.localPosition.y,
            height
        );
    }


    // Camera shake functions

    private IEnumerator ShakeCoroutine(float magnitude, float duration)
    {
        Vector3 initialPosition = cameraComponent.transform.localPosition;
        float elapsedTime = 0.0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float x = initialPosition.x + Random.Range(-1f, 1f) * magnitude;
            float y = initialPosition.y + Random.Range(-1f, 1f) * magnitude;

            cameraComponent.transform.localPosition = new Vector3(x, y, cameraComponent.transform.localPosition.z);

            yield return null;
        }

        cameraComponent.transform.localPosition = initialPosition;
    }   

    public void StartShaking(float magnitude = 0.1f, float duration = 0.1f) => StartCoroutine(ShakeCoroutine(magnitude, duration));

    // Camera raycast

    public bool RaycastToMouse(out RaycastHit hit, int layerMask = -1)
    {
        Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Abs(m_offset * 2f), layerMask);
    }
}
