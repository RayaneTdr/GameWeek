using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    #region SINGLETON
    static WaveManager instance = null;
    public static WaveManager Instance => instance;

    public Transform promotionT;
    public float distractionTime = 10f;

    // ----- TO REMOVE
    public PlayerCamera cam;
    public GameObject smokePrefab;

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

    [SerializeField] Vector2 currentTime = new Vector2(8f, 0f); // in game time
    public List<int> waves = new List<int>(); // number of dummies to spawn on waves

    public List<Dummy> dummies = new List<Dummy>();

    public List<int> focusedSpawner = new List<int>(); // main spawner used
    public List<Spawner> spawners;
    public List<Target> targets;
    public List<End> ends;

    int waveIndex = -1;


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


        //------ TO REMOVE
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cam.RaycastToMouse(out RaycastHit hit, LayerMask.GetMask("Floor")))
                Instantiate(smokePrefab, new Vector3(hit.point.x, hit.point.y+ 0.855f, hit.point.z), Quaternion.identity);
        }
        
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            LaunchDistraction();
        }
    }

    public void UpdateHour()
    {
        currentTime.y += Time.deltaTime * 30f / timeBetweenWaves; // tick

        if (currentTime.y > 30f && !dirtyFlag)    //wave system
            LaunchWave();


        if (currentTime.y >= 60f)   //clock system
        {
            currentTime.x += 1f;
            currentTime.y -= 60f;

            if (!dirtyFlag)
                LaunchWave();
        }
    }

    public void LaunchWave()
    {
        waveIndex++;
        dirtyFlag = true;
        StartCoroutine(TickWave());
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
        int index = Random.Range(0, 10);
        if (index < 5)
        {
            return focusedSpawner[waveIndex];
        }
        else
        {
            index = Random.Range(0, 2);

            if (index == 0)
                return (int)Mathf.Repeat(focusedSpawner[waveIndex] - 1, focusedSpawner.Count - 1);
            else
                return (int)Mathf.Repeat(focusedSpawner[waveIndex] + 1, focusedSpawner.Count - 1);
        }
    }

    IEnumerator TickWave()
    {
        Timer wave = new Timer(30f);
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

        dirtyFlag = false;
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
        Invoke("ResetDistraction", distractionTime);
    }

    public void ResetDistraction() 
    {
        // may optimize this
        foreach (Dummy dummy in dummies)
            dummy.ResetPromotionDestination();
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
