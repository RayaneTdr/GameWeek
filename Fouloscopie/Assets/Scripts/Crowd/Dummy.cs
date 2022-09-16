using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Events;

public class Dummy : MonoBehaviour
{
    AIDestinationSetter agent;
    Animator animator;


    public static int maxCollapsing = 4; // if this number is reached, start DESTRUCTION 

    public static int deathRatio = 5;         // 5 = 1/5 chance, the nominator increase by time 
    public static float deathRatioTime = 1f; // time before the chance of die increase

    public bool alive = true;

    [HideInInspector] internal Transform spawnPos = null;

    [SerializeField] float corpseDuration = 1.5f;

    [SerializeField] GameObject willDieSystem = null;

    [SerializeField] AudioSource source;

    SpatializedSource sound;

    public GameObject decal;

    public bool isLeaving = false;

    float timeBeforeDeathChanceIncrease = 1f;

    public int deathIncreaseChance = 0;
    public int currentCollapsing = 0;

    public UnityEvent onDieBroadcast; // usefull to tell collapsed dummies that 'this' died

    public bool IsOvercollapsing => currentCollapsing >= maxCollapsing;

    [HideInInspector] public bool isAttracted = false;
    void Start()
    {
        agent = GetComponent<AIDestinationSetter>();
        animator = GetComponentInChildren<Animator>();
        sound = GetComponent<SpatializedSource>();

        GetComponent<AIPath>().maxSpeed = 3f + Random.value * 4f;

        SetMarketDestination();
    }

    // Auto Register
    private void OnEnable() => WaveManager.Instance.dummies.Add(this);
    private void OnDisable() => WaveManager.Instance.dummies.Remove(this);


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("dummy"))
            return;

        if (other.TryGetComponent(out Dummy d) && d.alive)
        {
            currentCollapsing++;
            d.onDieBroadcast.AddListener(DecrementCollapsing);

            if (IsOvercollapsing)
            {
                GetComponentInChildren<Outline>().enabled = true;
                willDieSystem.SetActive(true);
                StartCoroutine(InitializeDeathProtocol());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("dummy"))
            return;

        currentCollapsing--;
        if (!IsOvercollapsing)
            ResetDieChance();
    }

    void SetMarketDestination()
    {
        animator.SetTrigger("Walk");
        Transform destination = WaveManager.Instance.GetNearestTarget(transform.position);
        agent.target = destination;
    }

    public void SetPromotionDestination() 
    {
        if (WaveManager.Instance.promoT) 
        {
            agent.target = WaveManager.Instance.promoT;
            isAttracted = true;
            // activate particles
        }
    }

    public void ResetPromotionDestination() 
    {
        GetComponent<AIPath>().isStopped = false;
        isAttracted = false;
        SetMarketDestination();
    }

    public void SetEntryAsDestination()
    {
        isLeaving = true;
        agent.target = WaveManager.Instance.GetNearestEnd(transform.position);
    }

    IEnumerator InitializeDeathProtocol()
    {
        //random chance to die, increase every second
        while (IsOvercollapsing && alive)
        {
            UpdateDieChance();
            if (TryDeath())
                Kill();

            yield return null;
        }
    }

    bool TryDeath()
    {
        int random = Random.Range(1, deathRatio);
        return deathIncreaseChance + random >= deathRatio;
    }


    public void StandAtPromotion() 
    {
        agent.target = null;
        animator.SetTrigger("Stand");
        GetComponent<AIPath>().isStopped = true;
    }

    void UpdateDieChance()
    {
        timeBeforeDeathChanceIncrease -= Time.deltaTime;

        if (timeBeforeDeathChanceIncrease <= 0f)
        {
            deathIncreaseChance++;
            timeBeforeDeathChanceIncrease = deathRatioTime;
        }
    }

    void ResetDieChance()
    {
        deathIncreaseChance = 0;
        willDieSystem.SetActive(false);
        GetComponentInChildren<Outline>().enabled = false;
    }

    public void Kill()
    {
        animator.SetTrigger("Death");
        onDieBroadcast.Invoke();
        alive = false;
        
        //Disable AI
        GetComponent<AIPath>().enabled = false;
        GetComponent<Pathfinding.RVO.RVOController>().enabled = false;
        agent.enabled = false;

        sound.Play("CustomerDeath");
        source.clip = GameManager.Instance.audioManager.GetClip("CustomerScream");
        source.Play();

        WaveManager.diedDummies++;
        StartCoroutine(CorpseAnimation());

        // sortie 
        decal.SetActive(true); 
        decal.transform.parent = null;
    }

    IEnumerator CorpseAnimation()
    {
        Timer timer = new Timer(corpseDuration);
        timer.Start();

        while (timer.Remaining >= 0f) 
        {
            timer.Tick(Time.deltaTime);
            yield return null;
        }

        timer.Reset();
        timer.max = 1f;

        // Fade Out
        Renderer[] rdrs = GetComponentsInChildren<Renderer>();

        while (timer.Remaining >= 0f) 
        {
            
            foreach (Renderer rdr in rdrs) 
            {
                if (rdr is ParticleSystemRenderer)
                    continue;

                Color clr = rdr.material.color;
                rdr.material.color = new Color(clr.r, clr.g, clr.b, timer.Remaining);
            }

            timer.Tick(Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject, .5f);
    }

    void DecrementCollapsing() 
    {
        currentCollapsing--;
        if (currentCollapsing < 0) currentCollapsing = 0;
    }

    public void Leave() 
    {
        GameManager.Instance.audioManager.Play("CashMachine");
        WaveManager.savedDummies++;
        // play saved animation
        Destroy(gameObject);
    }   
}

