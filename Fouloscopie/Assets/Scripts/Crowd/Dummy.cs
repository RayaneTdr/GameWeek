using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class Dummy : MonoBehaviour
{
    AIDestinationSetter agent;
    Animator animator;


    public static int maxCollapsing = 5; // if this number is reached, start DESTRUCTION 

    public static int deathRatio = 5;         // 5 = 1/5 chance, the nominator increase by time 
    public static float deathRatioTime = 1f; // time before the chance of die increase

    public bool alive = true;

    [HideInInspector] internal Transform spawnPos = null;

    [SerializeField] float corpseDuration = 1.5f;


    float timeBeforeDeathChanceIncrease = 1f;

    public int deathIncreaseChance = 0;
    public int currentCollapsing = 0;

    public bool IsOvercollapsing => currentCollapsing >= maxCollapsing;

    void Start()
    {
        agent    = GetComponent<AIDestinationSetter>();
        animator = GetComponentInChildren<Animator>();
        SetMarketDestination();
        animator.SetTrigger("Walk");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("dummy"))
            return;

        if (!other.GetComponent<Dummy>().alive)
            return;

        currentCollapsing++;
        if (IsOvercollapsing)
            StartCoroutine(InitializeDeathProtocol());
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
        Transform destination = GameObject.FindGameObjectWithTag("target").transform;
        agent.target = destination;
    }

    public void SetEntryAsDestination()
    {
        if (spawnPos)
            agent.target = spawnPos;
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
    }

    void Kill()
    {
        animator.SetTrigger("Death");
        alive = false;
        GetComponent<AIPath>().enabled = false;
        GetComponent<Pathfinding.RVO.RVOController>().enabled = false;
        agent.enabled = false;
        StartCoroutine(CorpseAnimation());
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
}

