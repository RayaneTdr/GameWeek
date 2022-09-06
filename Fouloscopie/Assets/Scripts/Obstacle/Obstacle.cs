using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private int obstructionCount = 0;
    private bool m_canBePlaced = false;
    private bool m_isNew = true;
    private bool m_isGrabbed = false;

    /*[SerializeField]
    private Material previewMaterial;
    private Material material;
    */
    private Collider m_collider;
    private Vector3    m_savedPosition;
    private Quaternion m_savedRotation;

    private float m_elapsedTime = 0.0f;
    private float m_scale = 1f;

    private Vector3 m_grabOffset;
    private bool    m_grabOffsetSet = false;
    
    [SerializeField] private float bounceSpeed = 10f;

    public AnimationCurve curve;

    private void Awake()
    {
        m_collider = GetComponentInChildren<Collider>();
    }

    private void Start()
    {
        m_collider.isTrigger = true;
    }

    void Update()
    {
        m_canBePlaced = obstructionCount == 0;

        if (m_isGrabbed)
        {
            m_elapsedTime += Time.deltaTime;

            float delta = Mathf.Sin(m_elapsedTime * bounceSpeed) * 0.5f + 0.5f;
            m_scale = Mathf.Lerp(0.8f, 0.9f, curve.Evaluate(delta));

            transform.localScale = Vector3.one * m_scale;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle") || other.CompareTag("Prop"))
        {
            obstructionCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Prop"))
        {
            obstructionCount = Mathf.Max(0, obstructionCount - 1);
        }
    }

    public void Drop()
    {
        obstructionCount = 0;
        m_grabOffsetSet  = false;
        m_grabOffset = Vector3.zero;

        if (m_canBePlaced)
        {
            m_collider.isTrigger = false;
            m_isNew = false;
            m_isGrabbed = false;
        }
        else if (!m_isNew)
        {
            transform.position = m_savedPosition;
            transform.rotation = m_savedRotation;

            m_collider.isTrigger = false;
            m_isNew = false;
            m_isGrabbed = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Grab()
    {
        m_collider.isTrigger = true;
        m_isGrabbed = true;

        if (!m_isNew)
        {
            m_savedPosition = transform.position;
            m_savedRotation = transform.rotation;
        }
    }

    public void FollowCursor(Vector3 position)
    {
        if(!m_isNew && !m_grabOffsetSet)
        {
            m_grabOffset    = transform.position - position;
            m_grabOffsetSet = true;
        }

        transform.position = position + m_grabOffset;
    }
}
