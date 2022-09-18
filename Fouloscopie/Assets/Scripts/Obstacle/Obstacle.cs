using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //  Private Variables

    private int m_obstructionCount = 0;
    private bool m_canBePlaced = false;
    private bool m_isGrabbed = false;
    public bool isMovable = true;

    private Collider m_collider;
    private Vector3    m_savedPosition;
    private Quaternion m_savedRotation;

    private float m_elapsedTime = 0.0f;
    private float m_scale = 1f;
    private float m_yawRotation = 0f;

    [SerializeField] string audioName;

    private Vector3 m_grabOffset;
    private bool    m_grabOffsetSet = true;

    private List<Dummy> m_dummiesInPreviewZone = new List<Dummy>();

    private List<MeshRenderer> m_renderer = new List<MeshRenderer>();
    
    private List<Material[]> m_defaultMaterials = new List<Material[]>();
    [SerializeField] private Material m_previewMaterial;

    [SerializeField]
    private GameObject m_model;

    [SerializeField] 
    private float bounceSpeed = 10f;

    private Animator m_animator;

    //  Public Variables

    [HideInInspector] 
    public bool isNew = true;

    public AnimationCurve curve;

    private float m_offsetOrigin;
    private LayerMask m_floorLayerMask;

    //  Monobehaviour Functions

    protected void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponentInChildren<Collider>();

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer r in renderers)
        {
            m_renderer.Add(r);
            m_defaultMaterials.Add(r.materials);
        }

        m_offsetOrigin = transform.lossyScale.y * 0.5f;
        m_floorLayerMask = LayerMask.GetMask("Floor");
    }

    protected void Start()
    {
        m_collider.isTrigger = true;
    }

    protected void Update()
    {
        bool couldBePLaced = m_canBePlaced;

        m_canBePlaced = m_obstructionCount == 0 && Physics.Raycast(transform.position + m_offsetOrigin * Vector3.up, Vector3.down, 100f, m_floorLayerMask);

        if (couldBePLaced != m_canBePlaced)
        {
            if (m_canBePlaced)
            {
                ChangeColor("_EmissionColor", new Color(0, 140, 190).Normalize());
            }
            else
            {
                ChangeColor("_EmissionColor", new Color(255, 0, 0).Normalize());
            }
        }

        if (m_isGrabbed)
        {
            //  Make a bounce effect with the object scale

            m_elapsedTime += Time.deltaTime;

            float delta = Mathf.Sin(m_elapsedTime * bounceSpeed) * 0.5f + 0.5f;
            m_scale = Mathf.Lerp(0.8f, 0.9f, curve.Evaluate(delta));

            m_model.transform.localScale = Vector3.one * m_scale;
        }
        else
        {
            //  Smoothly set the scale to 1 if not grabbed
            m_model.transform.localScale = Vector3.Lerp(m_model.transform.localScale, Vector3.one, 0.05f);
        }
    }

    //  Trigger Events Functions

    protected void OnTriggerEnter(Collider other)
    {
        if (m_isGrabbed)
        {
            if (other.CompareTag("Obstacle") || other.CompareTag("Prop"))
            {
                m_obstructionCount++;
            }
            else if (other.CompareTag("dummy"))
            {
                if (other.gameObject.TryGetComponent(out Dummy dummy))
                {
                    m_dummiesInPreviewZone.Add(dummy);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_isGrabbed)
        {
            if (other.CompareTag("Obstacle") || other.CompareTag("Prop"))
            {
                //  Teleporting a collider which is inside the trigger won't trigger the OnTriggerExit.
                //  To avoid this problem we reset obstruction count to 0 in the Drop() function.
                //  Here, to avoid this variable to go beneath 0, we limit over 0.
                m_obstructionCount = Mathf.Max(0, m_obstructionCount - 1);
            }
            else if (other.CompareTag("dummy"))
            {
                if (other.gameObject.TryGetComponent(out Dummy dummy))
                {
                    m_dummiesInPreviewZone.Remove(dummy);
                }
            }
        }
    }

    //  Functions

    private void ChangeColor(string nameID, Color color)
    {
        foreach (MeshRenderer r in m_renderer)
        {
            for (int i = 0; i < r.materials.Length; i++)
            {
                r.materials[i].SetColor(nameID, color);
            }
        }
    }

    public virtual bool Drop()
    {
        m_grabOffsetSet = false;
        m_grabOffset    = Vector3.zero;

        bool success = false;

        //  If it can be placed wether its new or not
        if (m_canBePlaced)
        {
            foreach (Dummy dummy in m_dummiesInPreviewZone)
            {
                if(dummy) dummy.Kill();
            }

            m_collider.isTrigger = false;
            isNew = false;
            m_isGrabbed = false;
            AstarPath.active.Scan();

            int i = 0;
            foreach (MeshRenderer r in m_renderer)
            {
                r.materials = m_defaultMaterials[i];
                i++;
            }

            GameManager.Instance.audioManager.Play(audioName);
            success = true;
        }
        else
        {
            AbortDrag();
        }

        m_obstructionCount = 0;
        m_dummiesInPreviewZone.Clear();

        return success;
    }


    public void AbortDrag()
    {
        //  If is not new and has saved positions and reset materials
        if (!isNew)
        {
            transform.position = m_savedPosition;
            transform.rotation = m_savedRotation;

            m_collider.isTrigger = false;
            isNew = false;
            m_isGrabbed = false;
            AstarPath.active.Scan();

            int i = 0;
            foreach (MeshRenderer r in m_renderer)
            {
                r.materials = m_defaultMaterials[i];
                i++;
            }
        }
        //  Else it can only be a new obstacle which can't be placed
        else
        {
            Destroy(gameObject);
        }
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

        foreach (MeshRenderer r in m_renderer)
        {
            var mats = r.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = m_previewMaterial;
            }
            r.materials = mats;
        }

        if (m_canBePlaced)
        {
            ChangeColor("_EmissionColor", new Color(0, 140, 190).Normalize());
        }
        else
        {
            ChangeColor("_EmissionColor", new Color(255, 0, 0).Normalize());
        }
    }

    public void FollowCursor(Vector3 position)
    {
        //  If the grab offset was not set (initially true for new obstacle, to avoid big offset because of the UI interference or blank raycast)
        if (!m_grabOffsetSet)
        {
            //  Save  and keep the offset between the mouse and the obstacle to avoid unwanted movement
            m_grabOffset    = transform.position - position;
            m_grabOffset    = new Vector3(m_grabOffset.x, 0f, m_grabOffset.z);
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

    public void StartDestroy()
    {
        //m_animator.SetTrigger("Destroy");
        Destroy(gameObject);
    }
}
