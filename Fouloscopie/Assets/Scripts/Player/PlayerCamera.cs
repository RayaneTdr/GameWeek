using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Transform))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField]  
    private float m_pitchRotation = 75f;

    [Header("Zoom / Offset parameters")]

    [SerializeField, Range(0.0f, 1.0f)]
    private float m_minZoom = 1.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_maxZoom = 0.05f;
    [SerializeField, Range(-100.0f, -10.0f)]
    private float m_offset = -50f;

    [SerializeField, Range(1.0f, 50.0f)]
    private float m_zoomStep = 25f;

    [SerializeField, Range(0.005f, 0.5f)]
    private float m_zoomSpeed = 0.005f;

    private float m_zoomLevel = 1f;
    private float m_zoomLevelTarget = 1f;
    private const float m_zoomStepCoef = 0.001f;

    [Header("Camera position boundaries")]

    [SerializeField] private float m_left  =-100f;
    [SerializeField] private float m_right = 100f;
    [SerializeField] private float m_back  =-100f;
    [SerializeField] private float m_front = 100f;

    [Header("Speed parameters")]

    [SerializeField, Range(1.0f, 50.0f)]
    private float m_movementSpeed = 25f;
    [SerializeField, Range(1.0f, 50.0f)]
    private float m_rotationSpeed = 25f;


    [HideInInspector] 
    public Camera cameraComponent;

    private PlayerController m_playerController;

    private float m_yawRotation = 0f;

    // MonoBehaviour Functions

    void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
        cameraComponent = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(m_pitchRotation, m_yawRotation, 0f);
        cameraComponent.transform.localPosition += Vector3.back * m_offset * m_zoomLevel;
    }

    void Update()
    {
        UpdatePosition();

        UpdateZoom();

        UpdateRotation();
    }

    // Functions

    public void UpdatePosition()
    {
        float grabSpeed = cameraComponent.transform.localPosition.z / m_offset;

        /*m_playerController.cameraMovement * m_movementSpeed + */


        //  First apply direction inputs to the movement
        Vector2 movement = m_playerController.directionInput;

        //  If grab movement are nopt frozen, apply it
        movement += m_playerController.cameraGrabMovement * grabSpeed;

        //  Scale the movement
        movement *= m_movementSpeed;

        // Apply Yaw rotation
        movement = new Vector2(
            -Mathf.Sin(-m_yawRotation * Mathf.Deg2Rad) * movement.y + Mathf.Cos(-m_yawRotation * Mathf.Deg2Rad) * movement.x,
             Mathf.Cos(-m_yawRotation * Mathf.Deg2Rad) * movement.y + Mathf.Sin(-m_yawRotation * Mathf.Deg2Rad) * movement.x
            );

        movement = Time.deltaTime * movement;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + movement.x, m_left, m_right),
            0.0f,
            Mathf.Clamp(transform.position.z + movement.y, m_back, m_front)
        );
    }

    private void UpdateZoom()
    {
        m_zoomLevelTarget = Mathf.Clamp(m_zoomLevelTarget + m_playerController.scrollDelta * m_zoomStepCoef * m_zoomStep, m_maxZoom, m_minZoom);
        m_zoomLevel = Mathf.Lerp(m_zoomLevel, m_zoomLevelTarget, m_zoomSpeed);
        float height = m_zoomLevel * m_offset;
        
        cameraComponent.transform.localPosition = new Vector3(
            cameraComponent.transform.localPosition.x,
            cameraComponent.transform.localPosition.y,
            height
        );
    }

    private void UpdateRotation()
    {
        m_yawRotation -= m_playerController.yawRotation * m_rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(m_pitchRotation, m_yawRotation, 0f);
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
        Ray ray = cameraComponent.ScreenPointToRay(m_playerController.mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Abs(m_offset * 2f), layerMask);
    }


    // GIZMO

    private void OnDrawGizmosSelected()
    {
        Vector3 A = new Vector3(m_left, 1f, m_front);
        Vector3 B = new Vector3(m_right, 1f, m_front);
        Vector3 C = new Vector3(m_right, 1f, m_back);
        Vector3 D = new Vector3(m_left, 1f, m_back);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(B, C);
        Gizmos.DrawLine(C, D);
        Gizmos.DrawLine(D, A);

        Gizmos.color = Color.white;
    }
}
