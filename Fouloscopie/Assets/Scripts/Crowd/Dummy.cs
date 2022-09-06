using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;


    public static int maxCollapsing = 5; // if this number is reached, start DESTRUCTION 

    public static int deathRatio = 5;         // 5 = 1/5 chance, the nominator increase by time 
    public static float deathRatioTime = 1f; // time before the chance of die increase

    public bool alive = true;

    Vector3 spawnPos = Vector3.zero;

    float timeBeforeDeathChanceIncrease = 1f;

    public int deathIncreaseChance = 0;
    public int currentCollapsing = 0;

    public bool IsOvercollapsing => currentCollapsing >= maxCollapsing;

    void Start()
    {
        agent    = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        spawnPos = transform.position;
        SetMarketDestination();
        animator.SetTrigger("Walk");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("dummy"))
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
        Vector3 destination = GameObject.FindGameObjectWithTag("target").transform.position;
        agent.SetDestination(destination);
    }

    public void SetEntryAsDestination()
    {
        agent.SetDestination(spawnPos);
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
        agent.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject, 1f);
    }
}

