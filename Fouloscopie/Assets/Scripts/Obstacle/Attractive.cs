using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractive : Obstacle
{
    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.TryGetComponent(out Dummy dummy) && dummy.isAttracted) 
        {
            dummy.StandAtPromotion();
        }
    }
}
