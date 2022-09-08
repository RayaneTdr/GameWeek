using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //  Private Variables

    private int obstructionCount = 0;
    private bool m_canBePlaced = false;
    private bool m_isGrabbed = false;

    private Collider m_collider;
    private Vector3    m_savedPosition;
    private Quaternion m_savedRotation;

    private float m_elapsedTime = 0.0f;
    private float m_scale = 1f;
    private float m_yawRotation = 0f;

    private Vector3 m_grabOffset;
    private bool    m_grabOffsetSet = true;
    
    [SerializeField] 
    private float bounceSpeed = 10f;

    //  Public Variables

    public int cost = 25;

    [HideInInspector] 
    public bool isNew = true;

    public AnimationCurve curve;

    //  Monobehaviour Functions

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
            //  Make a bounce effect with the object scale

            m_elapsedTime += Time.deltaTime;

            float delta = Mathf.Sin(m_elapsedTime * bounceSpeed) * 0.5f + 0.5f;
            m_scale = Mathf.Lerp(0.8f, 0.9f, curve.Evaluate(delta));

            transform.localScale = Vector3.one * m_scale;
        }
        else
        {
            //  Smoothly set the scale to 1 if not grabbed
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.05f);
        }
    }

    //  Trigger Events Functions

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
            //  Teleporting a collider which is inside the trigger won't trigger the OnTriggerExit.
            //  To avoid this problem we reset obstruction count to 0 in the Drop() function.
            //  Here, to avoid this variable to go beneath 0, we limit over 0.
            obstructionCount = Mathf.Max(0, obstructionCount - 1);
        }
    }

    //  Functions

    public bool Drop()
    {
        obstructionCount = 0;
        m_grabOffsetSet  = false;
        m_grabOffset = Vector3.zero;


        //  If it can be placed wether its new or not
        if (m_canBePlaced)
        {
            m_collider.isTrigger = false;
            isNew = false;
            m_isGrabbed = false;
            AstarPath.active.Scan();

            return true;
        }

        //  If is not new and has saved positions
        if (!isNew)
        {
            transform.position = m_savedPosition;
            transform.rotation = m_savedRotation;

            m_collider.isTrigger = false;
            isNew = false;
            m_isGrabbed = false;
            AstarPath.active.Scan();

            return false;
        }

        //  Else it can only be a new obstacle which can't be placed
        Destroy(gameObject);

        return false;
    }

    public void Grab()
    {
        //  If the obstacle is not new, then save its position before being grabbed
        if (!isNew)
        {
            m_savedPosition = transform.position;
            m_savedRotation = transform.rotation;
        }

        //  Set to trigger for grab and drop checks
        m_collider.isTrigger = true;

        //  The obstacle is now considered as grabbed
        m_isGrabbed = true;
    }

    public void FollowCursor(Vector3 position)
    {
        //  If the grab offset was not set (initially true for new obstacle, to avoid big offset because of the UI interference or blank raycast)
        if(!m_grabOffsetSet)
        {
            //  Save  and keep the offset between the mouse and the obstacle to avoid unwanted movement
            m_grabOffset    = transform.position - position;
            m_grabOffsetSet = true;
        }

        //  Apply the position with the offset
        transform.position = position + m_grabOffset;
    }
    public void Rotate(float rotation)
    {
        //  Apply rotation to yaw axis
        m_yawRotation += rotation;

        transform.rotation = Quaternion.Euler(0f, m_yawRotation, 0f);

        //  Disable grabOffsetSet to avoid unwanted change in this obstacle position after the rotation
        m_grabOffsetSet = false;
    }
}
