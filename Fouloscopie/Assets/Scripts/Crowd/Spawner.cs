using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<GameObject> entityPrefabs = new List<GameObject>();

    [SerializeField] float spawnRate = .5f; // in secs
    public int spawnerIndex = 0;

    [SerializeField] int countToSpawn = 0;

    Timer timer = new Timer();

    public void AddDummy() 
    {
        countToSpawn++;
    }

    private void Start()
    {
        WaveManager.Instance.spawners.Add(this);
        timer.max = spawnRate;
        timer.Start();
    }

    void Spawn() 
    {
        int spawnIndex = Random.Range(0, entityPrefabs.Count);
        Dummy go= Instantiate(entityPrefabs[spawnIndex], transform.position, Quaternion.identity).GetComponent<Dummy>();
        go.spawnPos = transform;
        countToSpawn--;

    }

    private void Update()
    {
        if (countToSpawn > 0) 
        {
            if (timer.Tick(Time.deltaTime))
            {
                Spawn();
                timer.Reset();
            }
        }
    }
}
