using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    private void Start() => WaveManager.Instance.ends.Add(this);


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Dummy dummy)) 
        {
            if (dummy.isLeaving)
                dummy.Leave();
        }
    }
}
