using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    #region SINGLETON
    static WaveManager instance = null;
    public static WaveManager Instance => instance;

    public TextMeshProUGUI timeRenderer;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    // x = hour, y = min
    [SerializeField] Vector2 startingTime = new Vector2(8f, 0f);
    [SerializeField] float timeBetweenWaves = 10f;  // time in seconds between each wave ( every 30 min in game)
    [SerializeField] int spawnChance = 10; //       1 / spawnchance

    [SerializeField] int attractedPercentage = 10; // x% of the crowd will be attracted

    [SerializeField] PlayerGrabber grabber;

    [SerializeField] Vector2 currentTime = new Vector2(8f, 0f); // in game time
    public List<int> waves = new List<int>(); // number of dummies to spawn on waves
 
    public List<Dummy> dummies = new List<Dummy>();

    public List<int> focusedSpawner = new List<int>(); // main spawner used
    public List<Spawner> spawners;
    public List<Target> targets;
    public List<End> ends;

    public GameObject endOutro;
    public int waveIndex = -1;

    [HideInInspector] public Transform promoT;

    public static int diedDummies = 0;
    public static int savedDummies = 0;

    bool dirtyFlag = false; // gros gros ratio
    
    bool isActive = true;

    private void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        if (isActive)
            UpdateHour();
    }

    public void UpdateHour()
    {
        currentTime.y += Time.deltaTime * 30f / timeBetweenWaves; // tick

        if (currentTime.y > 30f && !dirtyFlag && isActive)    //wave system
            LaunchWave();


        if (currentTime.y >= 60f)   //clock system
        {
            currentTime.x += 1f;
            currentTime.y -= 60f;
            
            LaunchWave();
            dirtyFlag = false;
        }

        // to modify
        if (currentTime.y < 10f)
            timeRenderer.text = currentTime.x + ":" +0+ (int)currentTime.y + "am!";
        else
            timeRenderer.text = currentTime.x + ":" + (int)currentTime.y + "am!";
    }

    public void LaunchWave()
    {
        if (!isActive) return;
        waveIndex++;

        if (waves.Count > waveIndex)
        {
            dirtyFlag = true;
            StartCoroutine(TickWave());
        }
        else if (waveIndex > waves.Count + 2) 
        {
            // start ending animation
            endOutro.SetActive(true);
            Time.timeScale = 0f;
            isActive = false;
        }
    }

    public void TrySpawn(bool force = false)
    {
        int countToDispatch = waves[waveIndex];
        for (int i = countToDispatch; i >= 0; i--)
        {
            if (!force)
            {
                if (CanSpawn())
                {
                    int spawnerIndex = SelectSpawner();
                    spawners[spawnerIndex].AddDummy();
                    countToDispatch--;
                    waves[waveIndex]--;
                }
            }
            else
            {
                spawners[focusedSpawner[waveIndex]].AddDummy();
                countToDispatch--;
                waves[waveIndex]--;
            }
        }
    }


    public bool CanSpawn()
    {
        return Random.Range(1, spawnChance + 1) >= spawnChance;
    }

    public int SelectSpawner()
    {
        int index = Random.Range(0, 2);
        
        if (index == 1)
            return focusedSpawner[waveIndex];

        index = Random.Range(0, 2);

        Debug.Log((int)Mathf.Repeat(focusedSpawner[waveIndex] - 1, focusedSpawner.Count - 1));
        Debug.Log((int)Mathf.Repeat(focusedSpawner[waveIndex] + 1, focusedSpawner.Count - 1));
        
        if (index == 0)
            return (int)Mathf.Repeat(focusedSpawner[waveIndex] - 1, focusedSpawner.Count - 1);
        else
            return (int)Mathf.Repeat(focusedSpawner[waveIndex] + 1, focusedSpawner.Count - 1);
       
    }

    IEnumerator TickWave()
    {
        //Timer wave = new Timer(30f); // - de 10 de qi pour coder ca
        Timer wave = new Timer(timeBetweenWaves); // - de 10 de qi pour coder ca
        Timer spawnBuffer = new Timer(1f);

        wave.Start();
        spawnBuffer.Start();
        while (wave.Remaining > 5f)
        {
            wave.Tick(Time.deltaTime);
            if (spawnBuffer.Tick(Time.deltaTime))
            {
                TrySpawn();
                spawnBuffer.Reset();
            }

            yield return null;
        }

        if (waves[waveIndex] > 0) // force spawn the remaining
            TrySpawn(true);

    }

    public void StartGame() => isActive = true;
    public void Pause() => isActive = false;

    public void LaunchDistraction() 
    {
        // Select x percent of the dummies and attract them to the promotion
        int countToAttract = Mathf.FloorToInt(attractedPercentage / 100f * dummies.Count);
        while (countToAttract > 0) 
        {
            dummies[Random.Range(0, dummies.Count)].SetPromotionDestination();
            countToAttract--;
        }
        GameManager.Instance.audioManager.Play("BonusAlert");
    }

    public void ResetDistraction(GameObject promotionGO) 
    {
        // may optimize this
        foreach (Dummy dummy in dummies)
            dummy.ResetPromotionDestination();

        Destroy(promotionGO);
    }

    public Transform GetRandomTarget()
    {
        return targets[Random.Range(0, targets.Count)].transform;
    }

    public Transform GetNearestTarget(Vector3 position) 
    {
        float nearestDistance = float.MaxValue;
        Transform nearest = null;
        foreach (Target target in targets) 
        {
            float distance = (target.transform.position - position).magnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = target.transform;
            }
        }
        return nearest;
    }

    public Transform GetRandomEnd()
    {
        return ends[Random.Range(0, ends.Count)].transform;
    }

    public Transform GetNearestEnd(Vector3 position)
    {
        float nearestDistance = float.MaxValue;
        Transform nearest = null;
        foreach (End end in ends)
        {
            float distance = (end.transform.position - position).magnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = end.transform;
            }
        }
        return nearest;
    }
}
