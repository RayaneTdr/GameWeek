using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject entityPrefab;

    [SerializeField] int count = 5;
    [SerializeField] float spawnRate = .5f; // in secs

    void Start() => StartCoroutine(InitSpawner());
    void Spawn() => Instantiate(entityPrefab, transform.position, Quaternion.identity);

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
