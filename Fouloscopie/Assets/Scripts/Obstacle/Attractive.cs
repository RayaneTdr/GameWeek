using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractive : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Dummy dummy)) 
        {
            dummy.StandAtPromotion();
        }
    }
}
