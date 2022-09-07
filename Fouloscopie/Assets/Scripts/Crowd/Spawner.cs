using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<GameObject> entityPrefabs = new List<GameObject>();

    [SerializeField] int count = 5;
    [SerializeField] float spawnRate = .5f; // in secs

    void Start() => StartCoroutine(InitSpawner());
    void Spawn() 
    {
        int spawnIndex = Random.Range(0, entityPrefabs.Count);
        Dummy go= Instantiate(entityPrefabs[spawnIndex], transform.position, Quaternion.identity).GetComponent<Dummy>();
        go.spawnPos = transform;
    }

    IEnumerator InitSpawner()
    {
        Timer timer = new Timer(spawnRate);
        timer.Start();

        while (count > 0)
        {
            if (timer.Tick(Time.deltaTime)) 
            {
                Spawn();
                timer.Reset();
                count--;
            }
            yield return null;
        }
    }

}
