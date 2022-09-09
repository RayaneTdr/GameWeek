using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractive : Obstacle
{

    public GameObject FX;
    public GameObject Chrono;

    private new void Awake()
    {
        base.Awake();

        isMovable = false;
    }

    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.TryGetComponent(out Dummy dummy) && dummy.isAttracted) 
        {
            dummy.StandAtPromotion();
        }
    }

    public override bool Drop()
    {
        if(base.Drop())
        {
            WaveManager.Instance.promotionT = transform;
            WaveManager.Instance.LaunchDistraction();

            FX.SetActive(true);
            Chrono.SetActive(true);
            GameManager.Instance.audioManager.Play("BonusAlert");        
            return true;
        }

        return false;
    }
}
